using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class QueryExampleHandler : IQueryHandler<QueryExample, int> {
    public ValueTask<IResult<int>> HandleAsync(QueryExample      request,
                                               CancellationToken cancellationToken = default) {
        return new ValueTask<IResult<int>>(Result<int>.Success(0));
    }
}
