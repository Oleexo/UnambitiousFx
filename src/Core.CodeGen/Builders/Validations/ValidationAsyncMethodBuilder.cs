using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Validations;

/// <summary>
///     Builds async validation extension methods for Result types (Task and ValueTask variants).
/// </summary>
internal sealed class ValidationAsyncMethodBuilder {
    private readonly string _baseNamespace;

    public ValidationAsyncMethodBuilder(string baseNamespace) {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    #region EnsureNotNull Async

    /// <summary>
    ///     Builds EnsureNotNullAsync for Result{T} from Task/ValueTask.
    /// </summary>
    public MethodWriter BuildEnsureNotNullAsync(bool isValueTask) {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";

        var body = """
                   var result = await awaitableResult;
                   return result.EnsureNotNull(selector, message, field);
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary($"Ensures a projected inner reference value (from {taskType}) is not null.")
                                            .WithParameter("awaitableResult", $"The {taskType} of result to await.")
                                            .WithParameter("selector",        "Function to select the inner value to check.")
                                            .WithParameter("message",         "Validation error message.")
                                            .WithParameter("field",           "Optional field name for the error message.")
                                            .WithTypeParameter("T",      "The result value type.")
                                            .WithTypeParameter("TInner", "The inner reference type to check for null.")
                                            .WithReturns($"A {taskType} with the validated result.");

        return new MethodWriter(
            "EnsureNotNullAsync",
            $"{taskType}<Result<T>>",
            body,
            Visibility.Public,
            MethodModifier.Static | MethodModifier.Async,
            [
                new MethodParameter($"this {taskType}<Result<T>>", "awaitableResult"),
                new MethodParameter("Func<T, TInner?>",            "selector"),
                new MethodParameter("string",                      "message"),
                new MethodParameter("string?",                     "field = null")
            ],
            [
                new GenericParameter("T",      "notnull"),
                new GenericParameter("TInner", "class")
            ],
            docBuilder.Build()
        );
    }

    #endregion

    #region Ensure Async

    /// <summary>
    ///     Builds EnsureAsync method - operates on Result{TValue...}.
    /// </summary>
    public MethodWriter BuildEnsureAsync(ushort arity,
                                         bool   isValueTask) {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var genericTypes  = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType    = $"Result<{genericTypes}>";

        var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(n => $"value{n}"));
        var predicateType    = $"Func<{genericTypes}, {taskType}<bool>>";
        var errorFactoryType = $"Func<{genericTypes}, {taskType}<Exception>>";

        string body;
        if (arity == 1) {
            body = """
                   return result.ThenAsync(async value => {
                       if (await predicate(value)) {
                           return Result.Success(value);
                       }

                       var ex = await errorFactory(value);
                       return Result.Failure<TValue1>(ex);
                   });
                   """;
        }
        else {
            body = $$"""
                     return result.ThenAsync(async ({{valueParams}}) => {
                         if (await predicate({{valueParams}})) {
                             return Result.Success({{valueParams}});
                         }

                         var ex = await errorFactory({{valueParams}});
                         return Result.Failure<{{genericTypes}}>(ex);
                     });
                     """;
        }

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary(
                                                 "Validates the result values asynchronously with a predicate and returns a failure with the provided exception if validation fails.")
                                            .WithParameter("result",       "The result instance.")
                                            .WithParameter("predicate",    "The async validation predicate.")
                                            .WithParameter("errorFactory", "Factory function to create an exception when validation fails.")
                                            .WithReturns($"A {taskType} representing the async operation with the result.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "EnsureAsync",
            $"{taskType}<{resultType}>",
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(predicateType,        "predicate"),
                new MethodParameter(errorFactoryType,     "errorFactory")
            ],
            genericParams,
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds EnsureAsync method for awaitable results - operates on Task/ValueTask{Result{TValue...}}.
    /// </summary>
    public MethodWriter BuildEnsureAwaitableAsync(ushort arity,
                                                  bool   isValueTask) {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var genericTypes  = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType    = $"Result<{genericTypes}>";

        var predicateType    = $"Func<{genericTypes}, {taskType}<bool>>";
        var errorFactoryType = $"Func<{genericTypes}, {taskType}<Exception>>";

        var body = """
                   var result = await awaitableResult;
                   return await result.EnsureAsync(predicate, errorFactory);
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Validates the awaitable result values asynchronously with a predicate.")
                                            .WithParameter("awaitableResult", $"The {taskType} of result to await.")
                                            .WithParameter("predicate",       "The async validation predicate.")
                                            .WithParameter("errorFactory",    "Factory function to create an exception when validation fails.")
                                            .WithReturns($"A {taskType} representing the async operation with the result.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "EnsureAsync",
            $"{taskType}<{resultType}>",
            body,
            Visibility.Public,
            MethodModifier.Static | MethodModifier.Async,
            [
                new MethodParameter($"this {taskType}<{resultType}>", "awaitableResult"),
                new MethodParameter(predicateType,                    "predicate"),
                new MethodParameter(errorFactoryType,                 "errorFactory")
            ],
            genericParams,
            docBuilder.Build(),
            usings: [$"UnambitiousFx.Core.Results.Extensions.Transformations.{taskType}s"]
        );
    }

    #endregion

    #region EnsureNotEmpty Async

    /// <summary>
    ///     Builds EnsureNotEmptyAsync for string results from Task/ValueTask.
    /// </summary>
    public MethodWriter BuildEnsureNotEmptyStringAsync(bool isValueTask) {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";

        var body = """
                   var result = await awaitableResult;
                   return result.EnsureNotEmpty(message, field);
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary($"Ensures a successful string result value (from {taskType}) is neither null nor empty.")
                                            .WithParameter("awaitableResult", $"The {taskType} of result to await.")
                                            .WithParameter("message",         "Validation error message.")
                                            .WithParameter("field",           "Optional field name for the error message.")
                                            .WithReturns($"A {taskType} with the validated result.");

        return new MethodWriter(
            "EnsureNotEmptyAsync",
            $"{taskType}<Result<string>>",
            body,
            Visibility.Public,
            MethodModifier.Static | MethodModifier.Async,
            [
                new MethodParameter($"this {taskType}<Result<string>>", "awaitableResult"),
                new MethodParameter("string",                           "message = \"Value must not be empty.\""),
                new MethodParameter("string?",                          "field = null")
            ],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds EnsureNotEmptyAsync for collection results from Task/ValueTask.
    /// </summary>
    public MethodWriter BuildEnsureNotEmptyCollectionAsync(bool isValueTask) {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";

        var body = """
                   var result = await awaitableResult;
                   return result.EnsureNotEmpty<TCollection, TItem>(message, field);
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary($"Ensures a successful enumerable result (from {taskType}) is not empty.")
                                            .WithParameter("awaitableResult", $"The {taskType} of result to await.")
                                            .WithParameter("message",         "Validation error message.")
                                            .WithParameter("field",           "Optional field name for the error message.")
                                            .WithTypeParameter("TCollection", "The collection type implementing IEnumerable<TItem>.")
                                            .WithTypeParameter("TItem",       "The item type in the collection.")
                                            .WithReturns($"A {taskType} with the validated result.");

        return new MethodWriter(
            "EnsureNotEmptyAsync",
            $"{taskType}<Result<TCollection>>",
            body,
            Visibility.Public,
            MethodModifier.Static | MethodModifier.Async,
            [
                new MethodParameter($"this {taskType}<Result<TCollection>>", "awaitableResult"),
                new MethodParameter("string",                                "message = \"Collection must not be empty.\""),
                new MethodParameter("string?",                               "field = null")
            ],
            [
                new GenericParameter("TCollection", "IEnumerable<TItem>"),
                new GenericParameter("TItem",       "")
            ],
            docBuilder.Build()
        );
    }

    #endregion
}
