using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Validations;

/// <summary>
///     Builds EnsureNotNull extension method for Result types.
///     Note: EnsureNotNull only applies to Result{T} (arity 1).
/// </summary>
internal sealed class EnsureNotNullMethodBuilder
{
  

    /// <summary>
    ///     Builds EnsureNotNull for Result{T}.
    /// </summary>
    public MethodWriter BuildStandaloneMethod()
    {
        var body = """
                   return result.Then(value => {
                       var inner = selector(value);
                       if (inner is not null) {
                           return Result.Success(value);
                       }

                       var finalMessage = field is null
                                              ? message
                                              : $"{field}: {message}";
                       return Result.Failure<T>(new ValidationError([finalMessage]));
                   });
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Ensures a projected inner reference value is not null. If null, returns a Failure Result with a ValidationError.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("selector", "Function to select the inner value to check.")
                                            .WithParameter("message", "Validation error message.")
                                            .WithParameter("field", "Optional field name for the error message.")
                                            .WithTypeParameter("T", "The result value type.")
                                            .WithTypeParameter("TInner", "The inner reference type to check for null.")
                                            .WithReturns("The original result if the inner value is not null; otherwise a failure with ValidationError.");

        return new MethodWriter(
            "EnsureNotNull",
            "Result<T>",
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter("this Result<T>",   "result"),
                new MethodParameter("Func<T, TInner?>", "selector"),
                new MethodParameter("string",           "message"),
                new MethodParameter("string?",          "field = null")
            ],
            [
                new GenericParameter("T",      "notnull"),
                new GenericParameter("TInner", "class")
            ],
            docBuilder.Build()
        );
    }
}
