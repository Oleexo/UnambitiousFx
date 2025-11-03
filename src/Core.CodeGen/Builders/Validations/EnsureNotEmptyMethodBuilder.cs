using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Validations;

/// <summary>
///     Builds EnsureNotEmpty extension methods for Result types.
///     Note: EnsureNotEmpty only applies to Result{string} (arity 1).
/// </summary>
internal sealed class EnsureNotEmptyMethodBuilder {
    private readonly string _baseNamespace;

    public EnsureNotEmptyMethodBuilder(string baseNamespace) {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    ///     Builds EnsureNotEmpty for Result{string}.
    /// </summary>
    public MethodWriter BuildStringMethod() {
        var body = """
                   return result.Then(value => {
                       if (string.IsNullOrEmpty(value)) {
                           var finalMessage = field is null
                                                  ? message
                                                  : $"{field}: {message}";
                           return Result.Failure<string>(new ValidationError(new[] { finalMessage }));
                       }

                       return Result.Success(value);
                   });
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Ensures a successful string result value is neither null nor empty.")
                                            .WithParameter("result",  "The result instance.")
                                            .WithParameter("message", "Validation error message.")
                                            .WithParameter("field",   "Optional field name for the error message.")
                                            .WithReturns("The original result if the string is not empty; otherwise a failure with ValidationError.");

        return new MethodWriter(
            "EnsureNotEmpty",
            "Result<string>",
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter("this Result<string>", "result"),
                new MethodParameter("string",              "message = \"Value must not be empty.\""),
                new MethodParameter("string?",             "field = null")
            ],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds EnsureNotEmpty for Result{TCollection} where TCollection is IEnumerable{TItem}.
    /// </summary>
    public MethodWriter BuildCollectionMethod() {
        var body = """
                   return result.Then(collection => {
                       if (!collection.Any()) {
                           var finalMessage = field is null
                                                  ? message
                                                  : $"{field}: {message}";
                           return Result.Failure<TCollection>(new ValidationError([finalMessage]));
                       }

                       return Result.Success(collection);
                   });
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Ensures a successful enumerable result is not empty.")
                                            .WithParameter("result",  "The result instance.")
                                            .WithParameter("message", "Validation error message.")
                                            .WithParameter("field",   "Optional field name for the error message.")
                                            .WithTypeParameter("TCollection", "The collection type implementing IEnumerable<TItem>.")
                                            .WithTypeParameter("TItem",       "The item type in the collection.")
                                            .WithReturns("The original result if the collection is not empty; otherwise a failure with ValidationError.");

        return new MethodWriter(
            "EnsureNotEmpty",
            "Result<TCollection>",
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter("this Result<TCollection>", "result"),
                new MethodParameter("string",                   "message = \"Collection must not be empty.\""),
                new MethodParameter("string?",                  "field = null")
            ],
            [
                new GenericParameter("TCollection", "IEnumerable<TItem>"),
                new GenericParameter("TItem",       "")
            ],
            docBuilder.Build()
        );
    }
}
