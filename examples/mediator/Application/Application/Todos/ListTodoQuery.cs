using Application.Domain.Entities;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public record ListTodoQuery : IRequest<IEnumerable<Todo>>;
