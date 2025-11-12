using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Serialization;

/// <summary>
///     JSON-based message serializer using System.Text.Json.
///     Supports both reflection-based and source-generated serialization for NativeAOT compatibility.
/// </summary>
/// <remarks>
///     <para>
///         For reflection-based serialization (default), use the parameterless constructor.
///         This mode is not compatible with NativeAOT trimming.
///     </para>
///     <para>
///         For NativeAOT scenarios, use the constructor that accepts a JsonSerializerContext
///         and provide a source-generated context that includes all your message types.
///     </para>
///     <example>
///         <code>
/// // Source-generated context for NativeAOT
/// [JsonSerializable(typeof(OrderCreatedEvent))]
/// [JsonSerializable(typeof(PaymentProcessedEvent))]
/// [JsonSourceGenerationOptions(
///     PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
///     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
/// public partial class MessageSerializerContext : JsonSerializerContext
/// {
/// }
///
/// // Register with DI
/// services.AddSingleton&lt;IMessageSerializer&gt;(
///     new JsonMessageSerializer(MessageSerializerContext.Default));
///         </code>
///     </example>
/// </remarks>
public sealed class JsonMessageSerializer : IMessageSerializer
{
    private readonly JsonSerializerOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonMessageSerializer" /> class
    ///     using source-generated serialization for NativeAOT compatibility.
    /// </summary>
    /// <param name="context">
    ///     The source-generated JsonSerializerContext that includes all message types.
    ///     This context should be generated using [JsonSerializable] attributes.
    /// </param>
    /// <remarks>
    ///     This constructor is NativeAOT compatible. The provided context must include
    ///     all message types that will be serialized/deserialized.
    /// </remarks>
    public JsonMessageSerializer(JsonSerializerContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _options = new JsonSerializerOptions(context.Options)
        {
            TypeInfoResolver = context
        };
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonMessageSerializer" /> class
    ///     using custom JsonSerializerOptions.
    /// </summary>
    /// <param name="options">The custom serializer options to use.</param>
    /// <remarks>
    ///     If the options include a TypeInfoResolver, source generation will be used.
    ///     Otherwise, reflection-based serialization will be used.
    /// </remarks>
    public JsonMessageSerializer(JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.TypeInfoResolver);

        _options = options;
    }

    /// <inheritdoc />
    public string ContentType => "application/json";

    /// <inheritdoc />
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Suppressed when using source generation via constructor.")]
    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Suppressed when using source generation via constructor.")]
    public byte[] Serialize(object message)
    {
        // Use source-generated serialization
        return JsonSerializer.SerializeToUtf8Bytes(message, message.GetType(), _options);
    }

    /// <inheritdoc />
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Suppressed when using source generation via constructor.")]
    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Suppressed when using source generation via constructor.")]
    public object Deserialize(byte[] data, Type messageType)
    {
        // Use source-generated deserialization
        return JsonSerializer.Deserialize(data, messageType, _options)
               ?? throw new InvalidOperationException($"Failed to deserialize message of type {messageType.Name}");
    }
}
