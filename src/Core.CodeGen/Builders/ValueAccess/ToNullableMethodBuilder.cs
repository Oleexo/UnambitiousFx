using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.ValueAccess;

/// <summary>
/// Builds ToNullable extension methods for Result types.
/// </summary>
internal sealed class ToNullableMethodBuilder
{
    private readonly string _baseNamespace;

    public ToNullableMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    /// Builds a standalone ToNullable method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";

        string returnType;
        string body;

        if (arity == 1)
        {
            returnType = "TValue1?";
            body = """
                   return result.TryGet(out var value)
                              ? (TValue1?)value
                              : default;
                   """;
        }
        else
        {
            var tupleType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");
            returnType = $"{tupleType}?";

            var tryGetParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => n == 1 ? "out var value1" : $"out var value{n}"));
            var valueParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));

            body = $$"""
                     if (!result.IsSuccess) {
                         return null;
                     }

                     return result.TryGet({{tryGetParams}})
                                ? ({{valueParams}})
                                : default;
                     """;
        }

        var docBuilder = DocumentationWriter.Create()
            .WithSummary(arity == 1
                ? "Returns the value as nullable if success; otherwise default."
                : "Returns the tuple of values as nullable if success; otherwise default.")
            .WithParameter("result", "The result instance.")
            .WithReturns("The nullable value or null/default.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "ToNullable",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            documentation: docBuilder.Build()
        );
    }
}
