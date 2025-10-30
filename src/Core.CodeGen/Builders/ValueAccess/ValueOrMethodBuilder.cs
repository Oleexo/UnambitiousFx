using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.ValueAccess;

/// <summary>
/// Builds ValueOr extension methods for Result types.
/// </summary>
internal sealed class ValueOrMethodBuilder
{
    private readonly string _baseNamespace;

    public ValueOrMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    /// Builds ValueOr method with default fallback values.
    /// </summary>
    public MethodWriter BuildWithDefaultFallback(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        var fallbackParameters = string.Join(", ",
            Enumerable.Range(1, arity).Select(n => $"TValue{n} fallback{n}"));
        var fallbackArgs = string.Join(", ",
            Enumerable.Range(1, arity).Select(n => $"fallback{n}"));

        string body;
        if (arity == 1)
        {
            body = "return result.Match<TValue1>(value1 => value1, _ => fallback1);";
        }
        else
        {
            var valueParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));
            body = $"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => ({fallbackArgs}));";
        }

        var parameters = new List<MethodParameter>
        {
            new MethodParameter($"this {resultType}", "result")
        };
        parameters.AddRange(Enumerable.Range(1, arity)
            .Select(n => new MethodParameter($"TValue{n}", $"fallback{n}")));

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained values when successful; otherwise provided fallback(s).")
            .WithParameter("result", "The result instance.")
            .WithReturns("The value(s) or fallback(s).");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
            docBuilder.WithParameter($"fallback{i}", $"Fallback value {i}.");
        }

        return new MethodWriter(
            name: "ValueOr",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds ValueOr method with factory function.
    /// </summary>
    public MethodWriter BuildWithFactory(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        string body;
        if (arity == 1)
        {
            body = "return result.Match<TValue1>(value1 => value1, _ => fallbackFactory());";
        }
        else
        {
            var valueParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));
            body = $"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => fallbackFactory());";
        }

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained values when successful; otherwise value(s) from factory.")
            .WithParameter("result", "The result instance.")
            .WithParameter("fallbackFactory", "Factory producing fallback value(s).")
            .WithReturns("The value(s) or factory value(s).");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "ValueOr",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter($"Func<{returnType}>", "fallbackFactory")
            ],
            genericParameters: genericParams,
            usings: ["System"],
            documentation: docBuilder.Build()
        );
    }
}
