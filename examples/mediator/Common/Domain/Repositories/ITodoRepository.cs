using Common.Domain.Entities;
using UnambitiousFx.Core;

namespace Common.Domain.Repositories;

public interface ITodoRepository {
    ValueTask CreateAsync(Todo              todo,
                          CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(Todo              todo,
                          CancellationToken cancellationToken = default);

    ValueTask<Option<Todo>> GetAsync(Guid              id,
                                     CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Guid              id,
                          CancellationToken cancellationToken);
}
