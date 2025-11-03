using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultFilterErrorExtensions class.
///     Generates FilterError extension methods for all Result arities.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultFilterErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFilterErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultFilterErrorExtensions",
                   FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateSyncMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate FilterErrorCore method only for arity 0 to avoid duplication
        if (arity == 0) {
            classWriter.AddMethod(GenerateFilterErrorCoreMethod());
        }

        // Generate FilterError method
        classWriter.AddMethod(GenerateFilterErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private ClassWriter GenerateAsyncMethods(ushort arity,
                                             bool   isValueTask) {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        var classWriter = new ClassWriter(
            "ResultExtensions",
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate FilterErrorAsync method for Task/ValueTask<Result> -> Task/ValueTask<Result>
        classWriter.AddMethod(GenerateFilterErrorAsyncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateFilterErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "FilterError";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Filters errors from the result based on the specified predicate.")
                                                      .WithParameter("result",    "The result to filter errors from.")
                                                      .WithParameter("predicate", "The predicate to determine which errors to keep.")
                                                      .WithReturns("A result with only the errors that match the predicate. If no errors match, returns a success result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal   = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateFilterErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IError, bool>", "predicate")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private (string resultType, string[] genericParams, GenericConstraint[] constraints) GetResultTypeInfo(ushort arity) {
        if (arity == 0) {
            return ("Result", [], []);
        }

        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => $"TValue{i}")
                                      .ToArray();

        var constraints = genericParams
                         .Select(param => GenericConstraint.NotNull(param))
                         .ToArray();

        var resultType = arity == 1
                             ? "Result<TValue1>"
                             : $"Result<{string.Join(", ", genericParams)}>";

        return (resultType, genericParams, constraints);
    }

    private string GenerateFilterErrorBody(ushort arity) {
        var successFactoryCall = GenerateSuccessFactoryCall(arity);
        var failureFactoryCall = GenerateFailureFactoryCall(arity);

        return $"""
                return FilterErrorCore(result,
                                       predicate,
                                       {successFactoryCall},
                                       {failureFactoryCall});
                """;
    }

    private string GenerateSuccessFactoryCall(ushort arity) {
        if (arity == 0) {
            return "Result.Success";
        }

        var defaultValues = Enumerable.Range(1, arity)
                                      .Select(i => $"default(TValue{i})!")
                                      .ToArray();

        return $"() => Result.Success({string.Join(", ", defaultValues)})";
    }

    private string GenerateFailureFactoryCall(ushort arity) {
        if (arity == 0) {
            return "Result.Failure";
        }

        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => $"TValue{i}")
                                      .ToArray();

        return $"Result.Failure<{string.Join(", ", genericParams)}>";
    }

    private MethodWriter GenerateFilterErrorCoreMethod() {
        var body = """
                   ArgumentNullException.ThrowIfNull(predicate);

                   if (original.IsSuccess)
                   {
                       return (TRes)original;
                   }

                   var successes = original.Reasons.Where(r => r is not IError)
                                         .ToList();
                   var errorsKept = original.Reasons.OfType<IError>()
                                          .Where(predicate)
                                          .Cast<IReason>()
                                          .ToList();
                   if (errorsKept.Count == 0)
                   {
                       var success = successFactory();
                       if (successes.Count != 0)
                       {
                           success.WithReasons(successes);
                       }

                       if (original.Metadata.Count != 0)
                       {
                           success.WithMetadata(original.Metadata);
                       }

                       return success;
                   }

                   var firstErr = (IError)errorsKept[0];
                   var failure  = failureFactory(firstErr);
                   foreach (var reason in errorsKept.Skip(1))
                   {
                       failure.WithReason(reason);
                   }

                   if (successes.Count != 0)
                   {
                       failure.WithReasons(successes);
                   }

                   if (original.Metadata.Count != 0)
                   {
                       failure.WithMetadata(original.Metadata);
                   }

                   return failure;
                   """;

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Core method for filtering errors from a result based on a predicate.")
                                               .WithTypeParameter("TRes", "The result type to return.")
                                               .WithParameter("original",       "The original result to filter.")
                                               .WithParameter("predicate",      "The predicate to determine which errors to keep.")
                                               .WithParameter("successFactory", "Factory function to create success results.")
                                               .WithParameter("failureFactory", "Factory function to create failure results.")
                                               .WithReturns("A result with only the errors that match the predicate.")
                                               .Build();

        return MethodWriter.Create("FilterErrorCore", "TRes", body)
                           .WithModifier(MethodModifier.Static)
                           .WithVisibility(Visibility.Private)
                           .WithGenericParameter("TRes")
                           .WithGenericConstraint(GenericConstraint.BaseClass("TRes", "BaseResult"))
                           .WithParameter("BaseResult",         "original")
                           .WithParameter("Func<IError, bool>", "predicate")
                           .WithParameter("Func<TRes>",         "successFactory")
                           .WithParameter("Func<IError, TRes>", "failureFactory")
                           .WithDocumentation(documentation)
                           .Build();
    }

    private MethodWriter GenerateFilterErrorAsyncMethod(ushort arity,
                                                        bool   isValueTask) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "FilterErrorAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType    = $"{taskType}<{resultType}>";
        var parameterType = $"{taskType}<{resultType}>";
        var predicateType = "Func<IError, bool>";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Asynchronously filters errors from the result based on the specified predicate.")
                                                      .WithParameter("awaitableResult", "The awaitable result to filter errors from.")
                                                      .WithParameter("predicate",       "The async predicate to determine which errors to keep.")
                                                      .WithReturns(
                                                           "A task with a result containing only the errors that match the predicate. If no errors match, returns a success result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal   = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateFilterErrorAsyncBody();

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(MethodModifier.Static | MethodModifier.Async)
                                  .WithExtensionMethod(parameterType, "awaitableResult")
                                  .WithParameter(predicateType, "predicate")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private string GenerateFilterErrorAsyncBody() {
        return """
               var result = await awaitableResult;
               return result.FilterError(predicate);
               """;
    }

    private static string GetOrdinalString(int number) {
        return number switch {
            1 => "first",
            2 => "second",
            3 => "third",
            4 => "fourth",
            5 => "fifth",
            6 => "sixth",
            7 => "seventh",
            8 => "eighth",
            _ => $"{number}th"
        };
    }
}
