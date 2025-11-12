namespace UnambitiousFx.Mediator.Transports.AwsSns;

/// <summary>
///     Configuration options for AWS SNS/SQS transport.
/// </summary>
public sealed class AwsSnsOptions
{
    /// <summary>
    ///     Gets or sets the AWS region (e.g., "us-east-1").
    /// </summary>
    public string Region { get; set; } = "us-east-1";

    /// <summary>
    ///     Gets or sets the SNS topic ARN for publishing messages.
    /// </summary>
    public string? TopicArn { get; set; }

    /// <summary>
    ///     Gets or sets the SQS queue URL for receiving messages.
    /// </summary>
    public string? QueueUrl { get; set; }

    /// <summary>
    ///     Gets or sets the optional transport name. If null, defaults to "awssns".
    /// </summary>
    public string? TransportName { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of messages to receive in a single batch (1-10).
    /// </summary>
    public int MaxNumberOfMessages { get; set; } = 10;

    /// <summary>
    ///     Gets or sets the long polling wait time in seconds (0-20).
    /// </summary>
    public int WaitTimeSeconds { get; set; } = 20;

    /// <summary>
    ///     Gets or sets the visibility timeout in seconds for received messages.
    /// </summary>
    public int VisibilityTimeout { get; set; } = 30;

    /// <summary>
    ///     Gets or sets the AWS access key ID. If null, uses default credential chain.
    /// </summary>
    public string? AccessKeyId { get; set; }

    /// <summary>
    ///     Gets or sets the AWS secret access key. If null, uses default credential chain.
    /// </summary>
    public string? SecretAccessKey { get; set; }
}
