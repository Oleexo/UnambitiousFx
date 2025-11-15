using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Security;

namespace UnambitiousFx.Mediator.Transports.Pipeline;

/// <summary>
///     Pipeline behavior that encrypts sensitive data before external dispatch and decrypts on inbound processing.
///     Uses <see cref="ISensitiveDataRegistry" /> for NativeAOT-compatible field access through delegates.
/// </summary>
public sealed class SensitiveDataEncryptionBehavior : IRequestPipelineBehavior
{
    private readonly ISensitiveDataRegistry _sensitiveDataRegistry;
    private readonly IDataProtector _dataProtector;
    private readonly ILogger<SensitiveDataEncryptionBehavior> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SensitiveDataEncryptionBehavior" /> class.
    /// </summary>
    /// <param name="sensitiveDataRegistry">The registry containing sensitive field descriptors.</param>
    /// <param name="dataProtectionProvider">The data protection provider for encryption/decryption.</param>
    /// <param name="logger">The logger.</param>
    public SensitiveDataEncryptionBehavior(
        ISensitiveDataRegistry sensitiveDataRegistry,
        IDataProtectionProvider dataProtectionProvider,
        ILogger<SensitiveDataEncryptionBehavior> logger)
    {
        _sensitiveDataRegistry = sensitiveDataRegistry;
        _dataProtector = dataProtectionProvider.CreateProtector("UnambitiousFx.Mediator.Transports.SensitiveData");
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<Result> HandleAsync<TRequest>(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        // Get sensitive field descriptors from registry
        var sensitiveFields = _sensitiveDataRegistry.GetSensitiveFields(typeof(TRequest));

        if (sensitiveFields.Count > 0)
        {
            _logger.LogDebug(
                "Found {Count} sensitive fields on request type {RequestType}",
                sensitiveFields.Count,
                typeof(TRequest).Name);

            // Encrypt sensitive fields before processing
            EncryptSensitiveData(request, sensitiveFields);
        }

        return await next();
    }

    /// <inheritdoc />
    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
    {
        // Get sensitive field descriptors from registry
        var sensitiveFields = _sensitiveDataRegistry.GetSensitiveFields(typeof(TRequest));

        if (sensitiveFields.Count > 0)
        {
            _logger.LogDebug(
                "Found {Count} sensitive fields on request type {RequestType}",
                sensitiveFields.Count,
                typeof(TRequest).Name);

            // Encrypt sensitive fields before processing
            EncryptSensitiveData(request, sensitiveFields);
        }

        return await next();
    }

    private void EncryptSensitiveData<TRequest>(TRequest request, IReadOnlyList<SensitiveFieldDescriptor> fields)
    {
        foreach (var field in fields)
        {
            if (!field.RequireEncryption)
            {
                continue;
            }

            try
            {
                var value = field.Getter(request!);
                if (!string.IsNullOrEmpty(value))
                {
                    var encrypted = _dataProtector.Protect(value);
                    field.Setter(request!, encrypted);

                    _logger.LogDebug(
                        "Encrypted sensitive field {FieldName} on request type {RequestType}",
                        field.FieldName,
                        typeof(TRequest).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to encrypt sensitive field {FieldName} on request type {RequestType}",
                    field.FieldName,
                    typeof(TRequest).Name);
                throw;
            }
        }
    }

    /// <summary>
    ///     Decrypts sensitive data on inbound messages.
    ///     This method should be called when processing inbound messages from external transports.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="request">The request instance with encrypted data.</param>
    public void DecryptSensitiveData<TRequest>(TRequest request)
    {
        var sensitiveFields = _sensitiveDataRegistry.GetSensitiveFields(typeof(TRequest));

        if (sensitiveFields.Count == 0) return;

        _logger.LogDebug(
            "Decrypting {Count} sensitive fields on request type {RequestType}",
            sensitiveFields.Count,
            typeof(TRequest).Name);

        foreach (var field in sensitiveFields)
        {
            if (!field.RequireEncryption)
            {
                continue;
            }

            try
            {
                var encryptedValue = field.Getter(request!);
                if (!string.IsNullOrEmpty(encryptedValue))
                {
                    var decrypted = _dataProtector.Unprotect(encryptedValue);
                    field.Setter(request!, decrypted);

                    _logger.LogDebug(
                        "Decrypted sensitive field {FieldName} on request type {RequestType}",
                        field.FieldName,
                        typeof(TRequest).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to decrypt sensitive field {FieldName} on request type {RequestType}",
                    field.FieldName,
                    typeof(TRequest).Name);
                throw;
            }
        }
    }
}
