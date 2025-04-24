using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Pipelines;

public sealed class LoggingBehavior : IRequestPipelineBehavior, IEventPipelineBehavior {
    private readonly ILogger<LoggingBehavior> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior> logger) {
        _logger = logger;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(IContext             context,
                                                       TEvent               @event,
                                                       EventHandlerDelegate next,
                                                       CancellationToken    cancellationToken = default) {
        var eventName = typeof(TEvent).Name;
        _logger.LogInformation("Handling {EventName}", eventName);

        var stopwatch = Stopwatch.StartNew();

        var result = await next();

        stopwatch.Stop();

        _logger.LogInformation("Handled {EventName} in {ElapsedMilliseconds}ms", eventName,
                               stopwatch.ElapsedMilliseconds);

        return result;
    }

    public async ValueTask<Result> HandleAsync<TRequest>(IContext               context,
                                                         TRequest               request,
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

    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(IContext                          context,
                                                                               TRequest                          request,
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
