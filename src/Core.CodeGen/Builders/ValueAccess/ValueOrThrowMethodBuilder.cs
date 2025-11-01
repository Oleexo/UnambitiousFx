using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.ValueAccess;

/// <summary>
/// Builds ValueOrThrow extension methods for Result types.
/// </summary>
internal sealed class ValueOrThrowMethodBuilder
{
    private readonly string _baseNamespace;

    public ValueOrThrowMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    /// Builds ValueOrThrow method with default exception.
    /// </summary>
    public MethodWriter BuildWithDefaultException(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        var body = "return result.ValueOrThrow(errors => throw errors.ToException());";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained value(s); throws aggregated exception when failure.")
            .WithParameter("result", "The result instance.")
            .WithReturns("The value(s) or throws.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "ValueOrThrow",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds ValueOrThrow method with custom exception factory.
    /// </summary>
    public MethodWriter BuildWithExceptionFactory(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        string body;
        if (arity == 1)
        {
            body = "return result.Match<TValue1>(value1 => value1, e => throw exceptionFactory(e));";
        }
        else
        {
            var valueParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));
            body = $"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), e => throw exceptionFactory(e));";
        }

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained value(s); otherwise throws exception from factory.")
            .WithParameter("result", "The result instance.")
            .WithParameter("exceptionFactory", "Factory creating exception from errors.")
            .WithReturns("The value(s) or throws custom exception.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "ValueOrThrow",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter("Func<IEnumerable<IError>, Exception>", "exceptionFactory")
            ],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }
}
