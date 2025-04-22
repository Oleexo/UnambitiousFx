using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Entities;

namespace WebApi.Application.Todos;

public record ListTodoQuery : IRequest<IEnumerable<Todo>>;
