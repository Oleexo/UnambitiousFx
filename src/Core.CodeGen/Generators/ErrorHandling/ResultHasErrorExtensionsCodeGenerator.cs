using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultHasErrorExtensions class.
///     Generates HasError extension methods for all Result arities with both error type and exception type checking.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultHasErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultHasErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultHasErrorExtensions",
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

        // Generate HasError method
        classWriter.AddMethod(GenerateHasErrorMethod(arity));

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

        // Generate HasErrorAsync method for Task/ValueTask<Result> -> Task/ValueTask<bool>
        classWriter.AddMethod(GenerateHasErrorAsyncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateHasErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "HasError";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Determines whether the result contains an error of the specified type.")
                                                      .WithTypeParameter("TError", "The type of error to check for. Can be an error type or exception type.")
                                                      .WithParameter("result", "The result to check for errors.")
                                                      .WithReturns("true if the result contains an error of the specified type; otherwise, false.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal   = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateHasErrorBody(arity);

        var builder = MethodWriter.Create(methodName, "bool", body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithGenericParameter("TError")
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

    private string GenerateHasErrorBody(ushort arity) {
        return """
               if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
                   return result.Reasons.OfType<ExceptionalError>()
                                .Any(e => e.Exception is TError);
               }
               return result.Reasons.OfType<TError>()
                            .Any();
               """;
    }

    private MethodWriter GenerateHasErrorAsyncMethod(ushort arity,
                                                     bool   isValueTask) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "HasErrorAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType    = $"{taskType}<bool>";
        var parameterType = $"{taskType}<{resultType}>";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Asynchronously determines whether the result contains an error of the specified type.")
                                                      .WithTypeParameter("TError", "The type of error to check for. Can be an error type or exception type.")
                                                      .WithParameter("awaitableResult", "The awaitable result to check for errors.")
                                                      .WithReturns("A task with true if the result contains an error of the specified type; otherwise, false.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal   = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateHasErrorAsyncBody(arity, genericParams);

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(MethodModifier.Static | MethodModifier.Async)
                                  .WithExtensionMethod(parameterType, "awaitableResult")
                                  .WithGenericParameter("TError")
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

    private string GenerateHasErrorAsyncBody(int      arity,
                                             string[] genericParams) {
        if (arity == 0) {
            return """
                   var result = await awaitableResult;
                   return result.HasError<TError>();
                   """;
        }

        return $"""
                var result = await awaitableResult;
                return result.HasError<TError,{string.Join(", ", genericParams)}>();
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
