using Application.Domain.Entities;
using Oleexo.UnambitiousFx.Core;

namespace Application.Domain.Repositories;

public interface ITodoRepository {
    ValueTask CreateAsync(Todo              todo,
                          CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(Todo              todo,
                          CancellationToken cancellationToken = default);

    ValueTask<IOption<Todo>> GetAsync(Guid              id,
                                      CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Guid              id,
                          CancellationToken cancellationToken);
}
