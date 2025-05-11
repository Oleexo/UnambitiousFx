using Common.Domain.Entities;
using Common.Domain.Events;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

public sealed class ManualRegisterGroup : IRegisterGroup {
    public void Register(IDependencyInjectionBuilder builder) {
        builder.RegisterRequestHandler<TodoQueryHandler, AppContext, TodoQuery, Todo>();
        builder.RegisterRequestHandler<DeleteTodoCommandHandler, AppContext, DeleteTodoCommand>();
        builder.RegisterEventHandler<TodoCreatedHandler, AppContext, TodoCreated>();
    }
}
