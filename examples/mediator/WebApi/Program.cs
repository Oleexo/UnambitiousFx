using Common;
using Common.Application;
using Common.Application.Todos;
using Common.Domain.Entities;
using Common.Domain.Repositories;
using Common.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Pipelines;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediator<IAppContext>(cfg => {
    cfg.AddRegisterGroup(new ManualRegisterGroup());
    cfg.AddRegisterGroup(new RegisterGroup());

    cfg.RegisterRequestPipelineBehavior<SimpleLoggingBehavior<IAppContext>>();
    cfg.RegisterEventPipelineBehavior<SimpleLoggingBehavior<IAppContext>>();
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

todoEndpoints.MapGet("/", async ([FromServices] IRequestHandler<IAppContext, ListTodoQuery, IEnumerable<Todo>> handler,
                                 [FromServices] IContextFactory<IAppContext>                                   contextFactory,
                                 CancellationToken                                                             cancellationToken) => {
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

todoEndpoints.MapPut("/{id:guid}", async ([FromServices] IRequestHandler<IAppContext, UpdateTodoCommand> handler,
                                          [FromServices] IContextFactory<IAppContext>                    contextFactory,
                                          [FromRoute]    Guid                                            id,
                                          [FromBody]     UpdateTodoModel                                 input,
                                          CancellationToken                                              cancellationToken) => {
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

namespace WebApi {
    public class Program;
}
