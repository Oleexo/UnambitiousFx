using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Notifications;

/// <summary>
/// Local handler - sends notifications within the same process
/// </summary>
[EventHandler<NotificationRequested>]
public sealed class NotificationRequestedHandler : IEventHandler<NotificationRequested>
{
    private readonly ILogger<NotificationRequestedHandler> _logger;

    public NotificationRequestedHandler(ILogger<NotificationRequestedHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(NotificationRequested @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Sending notification to {RecipientEmail}: {Subject}",
            @event.RecipientEmail,
            @event.Subject);

        // Simulate sending email
        _logger.LogDebug("Email content: {Message}", @event.Message);

        return ValueTask.FromResult(Result.Success());
    }
}
