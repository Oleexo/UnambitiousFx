namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines retry behavior for message processing failures.
/// </summary>
public sealed class RetryPolicy
{
    /// <summary>
    ///     Gets or initializes the maximum number of retry attempts.
    /// </summary>
    public int MaxAttempts { get; init; } = 3;

    /// <summary>
    ///     Gets or initializes the initial delay before the first retry.
    /// </summary>
    public TimeSpan InitialDelay { get; init; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Gets or initializes the maximum delay between retries.
    /// </summary>
    public TimeSpan MaxDelay { get; init; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     Gets or initializes the backoff multiplier for exponential backoff.
    /// </summary>
    public double BackoffMultiplier { get; init; } = 2.0;

    /// <summary>
    ///     Gets or initializes whether to use jitter to randomize retry delays.
    /// </summary>
    public bool UseJitter { get; init; } = true;

    /// <summary>
    ///     Calculates the delay for a specific retry attempt.
    /// </summary>
    /// <param name="attemptNumber">The attempt number (1-based).</param>
    /// <returns>The calculated delay duration.</returns>
    public TimeSpan CalculateDelay(int attemptNumber)
    {
        var delay = InitialDelay * Math.Pow(BackoffMultiplier, attemptNumber - 1);
        delay = TimeSpan.FromMilliseconds(Math.Min(delay.TotalMilliseconds, MaxDelay.TotalMilliseconds));

        if (UseJitter)
        {
            var jitter = Random.Shared.NextDouble() * 0.3; // ±30%
            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * (1 + jitter - 0.15));
        }

        return delay;
    }
}
