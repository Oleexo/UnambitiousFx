using Application.Application.Todos;
using Application.Domain.Entities;
using Application.Domain.Repositories;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Oleexo.UnambitiousFx.Mediator;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Pipelines;
using WebApiAot.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });
builder.Services.AddMediator(cfg => {
    cfg.RegisterHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>()
       .RegisterHandler<DeleteTodoCommandHandler, DeleteTodoCommand>()
       .RegisterHandler<ListTodoQueryHandler, ListTodoQuery, IEnumerable<Todo>>()
       .RegisterHandler<TodoQueryHandler, TodoQuery, Todo>()
       .RegisterHandler<UpdateTodoCommandHandler, UpdateTodoCommand>();
    cfg.RegisterRequestPipelineBehavior<LoggingBehavior>();
});
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var todoEndpoints = app.MapGroup("/todo");

todoEndpoints.MapGet("/{id:guid}", async ([FromRoute]    Guid    id,
                                          [FromServices] ISender sender,
                                          CancellationToken      cancellationToken) => {
    var query  = new TodoQuery { Id = id };
    var result = await sender.SendAsync<TodoQuery, Todo>(query, cancellationToken);
    return result.Match(Results.Ok,
                        error => Results.BadRequest(error.Message));
});

todoEndpoints.MapGet("/", async ([FromServices] IRequestHandler<ListTodoQuery, IEnumerable<Todo>> handler,
                                 [FromServices] IContextFactory                                   contextFactory,
                                 CancellationToken                                                cancellationToken) => {
    var query = new ListTodoQuery();

    var ctx    = contextFactory.Create();
    var result = await handler.HandleAsync(ctx, query, cancellationToken);

    return result.Match(Results.Ok,
                        error => Results.BadRequest(error.Message));
});

todoEndpoints.MapPost("/", async ([FromServices] ISender         sender,
                                  [FromBody]     CreateTodoModel input,
                                  CancellationToken              cancellationToken) => {
    var command = new CreateTodoCommand { Name = input.Name };

    var result = await sender.SendAsync<CreateTodoCommand, Guid>(command, cancellationToken);

    return result.Match(id => Results.Created("/todo/" + id, id),
                        error => Results.BadRequest(error.Message));
});

todoEndpoints.MapPut("/{id:guid}", async ([FromServices] IRequestHandler<UpdateTodoCommand> handler,
                                          [FromServices] IContextFactory                    contextFactory,
                                          [FromRoute]    Guid                               id,
                                          [FromBody]     UpdateTodoModel                    input,
                                          CancellationToken                                 cancellationToken) => {
    var command = new UpdateTodoCommand {
        Id   = id,
        Name = input.Name
    };

    var ctx    = contextFactory.Create();
    var result = await handler.HandleAsync(ctx, command, cancellationToken);

    return result.Match(() => Results.Ok(),
                        error => Results.BadRequest(error.Message));
});

todoEndpoints.MapDelete("/{id:guid}", async ([FromServices] ISender sender,
                                             [FromRoute]    Guid    id,
                                             CancellationToken      cancellationToken) => {
    var command = new DeleteTodoCommand { Id = id };

    var result = await sender.SendAsync(command, cancellationToken);

    return result.Match(() => Results.Ok(),
                        error => Results.BadRequest(error.Message));
});

app.Run();

namespace WebApiAot {
    public partial class Program;
}
