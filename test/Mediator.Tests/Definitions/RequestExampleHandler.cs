using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestExampleHandler : IRequestHandler<RequestExample, int> {
    public bool            Executed        { get; private set; }
    public RequestExample? RequestExecuted { get; private set; }
    public int             ExecutionCount  { get; private set; }
    public Action?         OnExecuted      { get; set; }

    public ValueTask<IResult<int>> HandleAsync(IContext          ctx,
                                               RequestExample    request,
                                               CancellationToken cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<IResult<int>>(Result<int>.Success(0));
    }
}
