using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WebApiAot.Models;

[JsonSerializable(typeof(UpdateTodoModel))]
[JsonSerializable(typeof(CreateTodoModel))]
[SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
internal partial class AppJsonSerializerContext : JsonSerializerContext {
}
