using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Validations;

/// <summary>
///     Builds Ensure extension methods for Result types.
/// </summary>
internal sealed class EnsureMethodBuilder {
    private readonly string _baseNamespace;

    public EnsureMethodBuilder(string baseNamespace) {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    ///     Builds a standalone Ensure method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity) {
        var genericTypes  = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType    = $"Result<{genericTypes}>";

        var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(n => $"value{n}"));
        var predicateType    = $"Func<{genericTypes}, bool>";
        var errorFactoryType = $"Func<{genericTypes}, IError>";

        string body;
        if (arity == 1) {
            body = """
                   return result.Then(value => predicate(value)
                                                   ? Result.Success(value)
                                                   : Result.Failure<TValue1>(errorFactory(value)));
                   """;
        }
        else {
            body = $$"""
                     return result.Then(({{valueParams}}) => predicate({{valueParams}})
                                                     ? Result.Success({{valueParams}})
                                                     : Result.Failure<{{genericTypes}}>(errorFactory({{valueParams}})));
                     """;
        }

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Validates the result values with a predicate and returns a failure with the provided exception if validation fails.")
                                            .WithParameter("result",       "The result instance.")
                                            .WithParameter("predicate",    "The validation predicate.")
                                            .WithParameter("errorFactory", "Factory function to create an exception when validation fails.")
                                            .WithReturns("The original result if validation succeeds; otherwise a failure result.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "Ensure",
            resultType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(predicateType,        "predicate"),
                new MethodParameter(errorFactoryType,     "errorFactory")
            ],
            genericParams,
            docBuilder.Build(),
            usings: [
                "UnambitiousFx.Core.Results.Extensions.Transformations",
                "UnambitiousFx.Core.Results.Reasons"
            ]
        );
    }
}
