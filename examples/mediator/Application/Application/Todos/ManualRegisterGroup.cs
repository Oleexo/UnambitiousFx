using Application.Domain.Entities;
using Application.Domain.Events;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class ManualRegisterGroup : IRegisterGroup
{
    public void Register(IDependencyInjectionBuilder builder)
    {
        builder.RegisterRequestHandler<TodoQueryHandler, TodoQuery, Todo>();
        builder.RegisterRequestHandler<DeleteTodoCommandHandler, DeleteTodoCommand>();
        builder.RegisterEventHandler<TodoCreatedHandler, TodoCreated>();
    }
}
