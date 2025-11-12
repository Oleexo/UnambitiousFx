using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UnambitiousFx.Mediator.Transports.AwsSns;

/// <summary>
///     Health check for AWS SNS/SQS transport connectivity.
/// </summary>
public sealed class AwsSnsHealthCheck : IHealthCheck
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly IAmazonSQS _sqsClient;
    private readonly AwsSnsOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AwsSnsHealthCheck"/> class.
    /// </summary>
    /// <param name="snsClient">The AWS SNS client.</param>
    /// <param name="sqsClient">The AWS SQS client.</param>
    /// <param name="options">The AWS SNS/SQS configuration options.</param>
    public AwsSnsHealthCheck(
        IAmazonSimpleNotificationService snsClient,
        IAmazonSQS sqsClient,
        AwsSnsOptions options)
    {
        _snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
        _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var healthData = new Dictionary<string, object>
        {
            ["region"] = _options.Region
        };

        try
        {
            var checks = new List<Task<bool>>();

            // Check SNS connectivity if TopicArn is configured
            if (!string.IsNullOrEmpty(_options.TopicArn))
            {
                checks.Add(CheckSnsHealthAsync(healthData, cancellationToken));
            }

            // Check SQS connectivity if QueueUrl is configured
            if (!string.IsNullOrEmpty(_options.QueueUrl))
            {
                checks.Add(CheckSqsHealthAsync(healthData, cancellationToken));
            }

            if (checks.Count == 0)
            {
                return HealthCheckResult.Degraded(
                    "No SNS topic or SQS queue configured",
                    data: healthData);
            }

            var results = await Task.WhenAll(checks);

            if (results.All(r => r))
            {
                return HealthCheckResult.Healthy(
                    "AWS SNS/SQS connection is healthy",
                    healthData);
            }

            if (results.Any(r => r))
            {
                return HealthCheckResult.Degraded(
                    "Some AWS SNS/SQS services are unhealthy",
                    data: healthData);
            }

            return HealthCheckResult.Unhealthy(
                "AWS SNS/SQS connection is unhealthy",
                data: healthData);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Failed to check AWS SNS/SQS health",
                exception: ex,
                data: healthData);
        }
    }

    private async Task<bool> CheckSnsHealthAsync(
        Dictionary<string, object> healthData,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify SNS topic exists and is accessible
            var getTopicAttributesRequest = new GetTopicAttributesRequest
            {
                TopicArn = _options.TopicArn
            };

            var response = await _snsClient.GetTopicAttributesAsync(
                getTopicAttributesRequest,
                cancellationToken);

            healthData["snsTopicArn"] = _options.TopicArn!;
            healthData["snsStatus"] = "healthy";

            return true;
        }
        catch (Exception ex)
        {
            healthData["snsTopicArn"] = _options.TopicArn!;
            healthData["snsStatus"] = "unhealthy";
            healthData["snsError"] = ex.Message;

            return false;
        }
    }

    private async Task<bool> CheckSqsHealthAsync(
        Dictionary<string, object> healthData,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify SQS queue exists and is accessible
            var getQueueAttributesRequest = new GetQueueAttributesRequest
            {
                QueueUrl = _options.QueueUrl,
                AttributeNames = new List<string> { "ApproximateNumberOfMessages" }
            };

            var response = await _sqsClient.GetQueueAttributesAsync(
                getQueueAttributesRequest,
                cancellationToken);

            healthData["sqsQueueUrl"] = _options.QueueUrl!;
            healthData["sqsStatus"] = "healthy";

            if (response.Attributes.TryGetValue("ApproximateNumberOfMessages", out var messageCount))
            {
                healthData["sqsApproximateMessageCount"] = messageCount;
            }

            return true;
        }
        catch (Exception ex)
        {
            healthData["sqsQueueUrl"] = _options.QueueUrl!;
            healthData["sqsStatus"] = "unhealthy";
            healthData["sqsError"] = ex.Message;

            return false;
        }
    }
}
