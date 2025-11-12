using Application.Application.Inventory;
using Application.Application.Notifications;
using Application.Application.Orders;
using Application.Application.Payments;
using Application.Application.Todos;
using Application.Domain.Entities;
using Application.Domain.Events;
using Application.Domain.Repositories;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Pipelines;
using UnambitiousFx.Mediator.Transports.Core;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediator(cfg =>
{
    cfg.AddRegisterGroup(new ManualRegisterGroup());

    // Todo handlers
    cfg.RegisterRequestHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>()
       .RegisterRequestHandler<ListTodoQueryHandler, ListTodoQuery, IEnumerable<Todo>>()
       .RegisterRequestHandler<UpdateTodoCommandHandler, UpdateTodoCommand>()
       .RegisterEventHandler<TodoUpdatedHandler, TodoUpdated>()
       .RegisterEventHandler<TodoDeletedHandler, TodoDeleted>();

    // Order handlers
    cfg.RegisterRequestHandler<CreateOrderCommandHandler, CreateOrderCommand, Guid>()
       .RegisterRequestHandler<ShipOrderCommandHandler, ShipOrderCommand>()
       .RegisterEventHandler<OrderCreatedHandler, OrderCreated>()
       .RegisterEventHandler<OrderShippedHandler, OrderShipped>()
       .RegisterEventHandler<OrderShippedNotificationHandler, OrderShipped>();

    // Payment handlers
    cfg.RegisterRequestHandler<ProcessPaymentCommandHandler, ProcessPaymentCommand>()
       .RegisterEventHandler<PaymentProcessedHandler, PaymentProcessed>()
       .RegisterEventHandler<PaymentAnalyticsHandler, PaymentProcessed>();

    // Inventory handlers
    cfg.RegisterRequestHandler<UpdateInventoryCommandHandler, UpdateInventoryCommand>()
       .RegisterEventHandler<InventoryUpdatedHandler, InventoryUpdated>();

    // Notification handlers
    cfg.RegisterEventHandler<NotificationRequestedHandler, NotificationRequested>();

    cfg.RegisterRequestPipelineBehavior<SimpleLoggingBehavior>();
    cfg.RegisterEventPipelineBehavior<SimpleLoggingBehavior>();

    // Enable distributed messaging with transports
    cfg.EnableDistributedMessaging(messaging =>
    {
        // Configure which events are external (sent through transport)
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));

        messaging.ConfigureEvent<PaymentProcessed>(opts => opts
            .AsExternal()
            .WithTopic("payments.processed"));

        messaging.ConfigureEvent<InventoryUpdated>(opts => opts
            .AsExternal()
            .WithTopic("inventory.updated"));

        // Local events (not sent through transport)
        messaging.ConfigureEvent<OrderCreated>(opts => opts.AsLocal());
        messaging.ConfigureEvent<NotificationRequested>(opts => opts.AsLocal());
        messaging.ConfigureEvent<TodoUpdated>(opts => opts.AsLocal());
        messaging.ConfigureEvent<TodoDeleted>(opts => opts.AsLocal());

        // Use in-memory transport for this example (simulates external transport)
        // In production, you would use: messaging.AddRabbitMq(opts => { ... })
        messaging.UseTransport<InMemoryTransport>();

        // Subscribe to external events (consume from transport)
        messaging.Subscribe<OrderShipped>();
        messaging.Subscribe<PaymentProcessed>();
        messaging.Subscribe<InventoryUpdated>();
    });
});
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");

var todoEndpoints = app.MapGroup("/todos");

todoEndpoints.MapGet("/{id:guid}", async ([FromRoute] Guid id,
                                          [FromServices] ISender sender,
                                          CancellationToken cancellationToken) =>
{
    var query = new TodoQuery { Id = id };
    var result = await sender.SendAsync<TodoQuery, Todo>(query, cancellationToken);
    return result.Match(Results.Ok,
                        error => Results.BadRequest(error.ToDisplayString()));
});

todoEndpoints.MapGet("/", async ([FromServices] IRequestHandler<ListTodoQuery, IEnumerable<Todo>> handler,
                                 [FromServices] IContextFactory contextFactory,
                                 CancellationToken cancellationToken) =>
{
    var query = new ListTodoQuery();

    var result = await handler.HandleAsync(query, cancellationToken);

    return result.Match(Results.Ok,
                        error => Results.BadRequest(error.ToDisplayString()));
});

todoEndpoints.MapPost("/", async ([FromServices] ISender sender,
                                  [FromBody] CreateTodoModel input,
                                  CancellationToken cancellationToken) =>
{
    var command = new CreateTodoCommand { Name = input.Name };

    var result = await sender.SendAsync<CreateTodoCommand, Guid>(command, cancellationToken);

    return result.Match(id => Results.Created("/todo/" + id, id),
                        error => Results.BadRequest(error.ToDisplayString()));
});

todoEndpoints.MapPut("/{id:guid}", async ([FromServices] IRequestHandler<UpdateTodoCommand> handler,
                                          [FromServices] IContextFactory contextFactory,
                                          [FromRoute] Guid id,
                                          [FromBody] UpdateTodoModel input,
                                          CancellationToken cancellationToken) =>
{
    var command = new UpdateTodoCommand
    {
        Id = id,
        Name = input.Name
    };

    var result = await handler.HandleAsync(command, cancellationToken);

    return result.Match(() => Results.Ok(),
                        error => Results.BadRequest(error.ToDisplayString()));
});

todoEndpoints.MapDelete("/{id:guid}", async ([FromServices] ISender sender,
                                             [FromRoute] Guid id,
                                             CancellationToken cancellationToken) =>
{
    var command = new DeleteTodoCommand { Id = id };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(() => Results.Ok(),
                        error => Results.BadRequest(error.ToDisplayString()));
});

// ===== Transport Examples =====

var orderEndpoints = app.MapGroup("/orders");

orderEndpoints.MapPost("/", async ([FromServices] ISender sender,
                                   [FromBody] CreateOrderModel input,
                                   CancellationToken cancellationToken) =>
{
    var command = new CreateOrderCommand
    {
        CustomerName = input.CustomerName,
        TotalAmount = input.TotalAmount
    };

    var result = await sender.SendAsync<CreateOrderCommand, Guid>(command, cancellationToken);

    return result.Match(
        orderId => Results.Created($"/orders/{orderId}", new { orderId }),
        error => Results.BadRequest(error.ToDisplayString()));
});

orderEndpoints.MapPost("/{id:guid}/ship", async ([FromServices] ISender sender,
                                                  [FromRoute] Guid id,
                                                  CancellationToken cancellationToken) =>
{
    var command = new ShipOrderCommand { OrderId = id };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(
        () => Results.Ok(new { message = "Order shipped successfully (event sent to transport)" }),
        error => Results.BadRequest(error.ToDisplayString()));
});

var paymentEndpoints = app.MapGroup("/payments");

paymentEndpoints.MapPost("/", async ([FromServices] ISender sender,
                                     [FromBody] ProcessPaymentModel input,
                                     CancellationToken cancellationToken) =>
{
    var command = new ProcessPaymentCommand
    {
        OrderId = input.OrderId,
        Amount = input.Amount,
        PaymentMethod = input.PaymentMethod
    };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(
        () => Results.Ok(new { message = "Payment processed (event sent to transport)" }),
        error => Results.BadRequest(error.ToDisplayString()));
});

var inventoryEndpoints = app.MapGroup("/inventory");

inventoryEndpoints.MapPost("/{productId:guid}/update", async ([FromServices] ISender sender,
                                                               [FromRoute] Guid productId,
                                                               [FromBody] UpdateInventoryModel input,
                                                               CancellationToken cancellationToken) =>
{
    var command = new UpdateInventoryCommand
    {
        ProductId = productId,
        QuantityChange = input.QuantityChange
    };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(
        () => Results.Ok(new { message = "Inventory updated (event sent to transport)" }),
        error => Results.BadRequest(error.ToDisplayString()));
});

app.Run();

namespace WebApi
{
    public class Program;
}
