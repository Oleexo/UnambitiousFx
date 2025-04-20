using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestExampleHandler : IRequestHandler<RequestExample, int> {
    public ValueTask<IResult<int>> HandleAsync(RequestExample    request,
                                               CancellationToken cancellationToken = default) {
        return new ValueTask<IResult<int>>(Result<int>.Success(0));
    }
}
