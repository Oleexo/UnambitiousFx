using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Pipelines;

public sealed class LoggingBehavior : IRequestPipelineBehavior {
    private readonly ILogger<LoggingBehavior> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior> logger) {
        _logger = logger;
    }

    public async ValueTask<IResult> HandleAsync<TRequest>(TRequest               request,
                                                          RequestHandlerDelegate next,
                                                          CancellationToken      cancellationToken = default)
        where TRequest : IRequest {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        var result = await next();

        stopwatch.Stop();

        _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName,
                               stopwatch.ElapsedMilliseconds);

        return result;
    }

    public async ValueTask<IResult<TResponse>> HandleAsync<TRequest, TResponse>(TRequest                          request,
                                                                                RequestHandlerDelegate<TResponse> next,
                                                                                CancellationToken                 cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
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
