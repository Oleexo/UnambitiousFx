namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for message serialization and deserialization.
/// </summary>
public interface IMessageSerializer
{
    /// <summary>
    ///     Gets the content type produced by this serializer (e.g., "application/json").
    /// </summary>
    string ContentType { get; }

    /// <summary>
    ///     Serializes a message to a byte array.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>The serialized message as a byte array.</returns>
    byte[] Serialize(object message);

    /// <summary>
    ///     Deserializes a byte array to a message of the specified type.
    /// </summary>
    /// <param name="data">The serialized message data.</param>
    /// <param name="messageType">The type of message to deserialize to.</param>
    /// <returns>The deserialized message.</returns>
    object Deserialize(byte[] data, Type messageType);
}
