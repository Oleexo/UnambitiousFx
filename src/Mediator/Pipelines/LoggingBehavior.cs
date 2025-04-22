using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Pipelines;

public sealed class LoggingBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) {
        _logger = logger;
    }

    public async ValueTask<IResult<TResponse>> HandleAsync(TRequest                          request,
                                                           RequestHandlerDelegate<TResponse> next,
                                                           CancellationToken                 cancellationToken = default) {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        var result = await next();

        stopwatch.Stop();

        _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName,
                               stopwatch.ElapsedMilliseconds);

        return result;
    }
}
