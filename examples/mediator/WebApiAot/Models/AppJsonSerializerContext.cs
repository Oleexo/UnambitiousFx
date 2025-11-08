using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Application.Domain.Entities;

namespace WebApiAot.Models;

[JsonSerializable(typeof(UpdateTodoModel))]
[JsonSerializable(typeof(CreateTodoModel))]
[JsonSerializable(typeof(IEnumerable<Todo>))]
[SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
