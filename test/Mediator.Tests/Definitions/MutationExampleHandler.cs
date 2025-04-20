using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class MutationExampleHandler : IMutationHandler<MutationExample> {
    public ValueTask<IResult<Unit>> HandleAsync(MutationExample   request,
                                                CancellationToken cancellationToken = default) {
        return new ValueTask<IResult<Unit>>(Result<Unit>.Success(Unit.Value));
    }
}
