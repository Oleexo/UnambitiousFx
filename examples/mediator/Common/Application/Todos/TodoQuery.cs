using Common.Domain.Entities;

namespace Common.Application.Todos;

public sealed record TodoQuery : IQuery<Todo> {
    public required Guid Id { get; init; }
}
