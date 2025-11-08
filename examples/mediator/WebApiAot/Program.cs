using Application.Application.Todos;
using Application.Domain.Entities;
using Application.Domain.Repositories;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Pipelines;
using WebApiAot.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });
builder.Services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>()
       .RegisterRequestHandler<DeleteTodoCommandHandler, DeleteTodoCommand>()
       .RegisterRequestHandler<ListTodoQueryHandler, ListTodoQuery, IEnumerable<Todo>>()
       .RegisterRequestHandler<TodoQueryHandler, TodoQuery, Todo>()
       .RegisterRequestHandler<UpdateTodoCommandHandler, UpdateTodoCommand>();
    cfg.RegisterRequestPipelineBehavior<SimpleLoggingBehavior>();
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
                        error => Results.BadRequest(error.ToDisplayString));
});

todoEndpoints.MapPost("/", async ([FromServices] ISender sender,
                                  [FromBody] CreateTodoModel input,
                                  CancellationToken cancellationToken) =>
{
    var command = new CreateTodoCommand { Name = input.Name };

    var result = await sender.SendAsync<CreateTodoCommand, Guid>(command, cancellationToken);

    return result.Match(id => Results.Created("/todo/" + id, id),
                        error => Results.BadRequest(error.ToDisplayString));
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
                        error => Results.BadRequest(error.ToDisplayString));
});

todoEndpoints.MapDelete("/{id:guid}", async ([FromServices] ISender sender,
                                             [FromRoute] Guid id,
                                             CancellationToken cancellationToken) =>
{
    var command = new DeleteTodoCommand { Id = id };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(() => Results.Ok(),
                        error => Results.BadRequest(error.ToDisplayString));
});

app.Run();

namespace WebApiAot
{
    public class Program;
}
