using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultAppendErrorExtensions class.
///     Generates AppendError extension methods for all Result arities.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultAppendErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultAppendErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultAppendErrorExtensions",
                   FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateAppendErrorMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateAppendErrorMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate AppendError method
        classWriter.AddMethod(GenerateAppendErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateAppendErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "AppendError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Appends a suffix to the error message of the first error in the result.")
                                               .WithParameter("result", "The result to append error message to.")
                                               .WithParameter("suffix", "The suffix to append to the error message.")
                                               .WithReturns("A new result with the appended error message if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GenerateAppendErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("string", "suffix")
                                  .WithDocumentation(documentation);

        // Add generic parameters and constraints
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
                                      .Select(i => $"T{i}")
                                      .ToArray();

        var constraints = genericParams
                         .Select(param => GenericConstraint.NotNull(param))
                         .ToArray();

        var resultType = arity == 1
                             ? "Result<T1>"
                             : $"Result<{string.Join(", ", genericParams)}>";

        return (resultType, genericParams, constraints);
    }

    private string GenerateAppendErrorBody(ushort arity) {
        return """
               if (string.IsNullOrEmpty(suffix) || result.IsSuccess) return result; // no-op
               return result.MapError(errs => errs.Select(x => x.WithMessage(x.Message + suffix)));
               """;
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

        // Generate AppendErrorAsync method for Result -> Task/ValueTask
        classWriter.AddMethod(GenerateAppendErrorAsyncMethod(arity, isValueTask, false));

        // Generate AppendErrorAsync method for Task/ValueTask<Result> -> Task/ValueTask
        classWriter.AddMethod(GenerateAppendErrorAsyncMethod(arity, isValueTask, true));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateAppendErrorAsyncMethod(ushort arity,
                                                        bool   isValueTask,
                                                        bool   isAwaitable) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "AppendErrorAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType = $"{taskType}<{resultType}>";
        var parameterType = isAwaitable
                                ? $"{taskType}<{resultType}>"
                                : resultType;

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Asynchronously appends a suffix to the error message of the first error in the result.")
                                                      .WithParameter(isAwaitable
                                                                         ? "awaitableResult"
                                                                         : "result", isAwaitable
                                                                                         ? "The awaitable result to append error message to."
                                                                                         : "The result to append error message to.")
                                                      .WithParameter("suffix", "The suffix to append to the error message.")
                                                      .WithReturns(
                                                           "A task with a new result containing the appended error message if the original result failed, otherwise the original successful result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal   = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateAppendErrorAsyncBody(arity, isValueTask, isAwaitable);

        var modifiers = MethodModifier.Static;
        if (isAwaitable) {
            modifiers |= MethodModifier.Async;
        }

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(modifiers)
                                  .WithExtensionMethod(parameterType, isAwaitable
                                                                          ? "awaitableResult"
                                                                          : "result")
                                  .WithParameter("string", "suffix")
                                  .WithDocumentation(documentation);

        // Add using for ValueAccess extensions
        if (isValueTask) {
            builder.WithUsings("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        }
        else {
            builder.WithUsings("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        }

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private string GenerateAppendErrorAsyncBody(ushort arity,
                                                bool   isValueTask,
                                                bool   isAwaitable) {
        if (isAwaitable) {
            return """
                   var result = await awaitableResult;
                   return result.AppendError(suffix);
                   """;
        }

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";

        return $"""
                if (string.IsNullOrEmpty(suffix) || result.IsSuccess) 
                    return {taskType}.FromResult(result);
                return {taskType}.FromResult(result.MapError(errs => errs.Select(x => x.WithMessage(x.Message + suffix))));
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
