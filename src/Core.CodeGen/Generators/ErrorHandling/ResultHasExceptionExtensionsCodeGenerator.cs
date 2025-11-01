using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultHasExceptionExtensions class.
/// Generates HasException extension methods for all Result arities for exception type checking.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultHasExceptionExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultHasExceptionExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultHasExceptionExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateSyncMethods(arity),
            GenerateAsyncMethods(arity, isValueTask: false),
            GenerateAsyncMethods(arity, isValueTask: true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate HasException method
        classWriter.AddMethod(GenerateHasExceptionMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private ClassWriter GenerateAsyncMethods(ushort arity, bool isValueTask) {
        var subNamespace = isValueTask ? "ValueTasks" : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        var classWriter = new ClassWriter(
            name: "ResultExtensions",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate HasExceptionAsync method for Task/ValueTask<Result> -> Task/ValueTask<bool>
        classWriter.AddMethod(GenerateHasExceptionAsyncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateHasExceptionMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "HasException";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Determines whether the result contains an error with an exception of the specified type.")
                                                      .WithTypeParameter("TException", "The type of exception to check for.")
                                                      .WithParameter("result", "The result to check for exceptions.")
                                                      .WithReturns("true if the result contains an error with an exception of the specified type; otherwise, false.");

        // Add documentation for all value type parameters
        for (int i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateHasExceptionBody();

        var builder = MethodWriter.Create(methodName, "bool", body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithGenericParameter("TException")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        // Add constraint for TException
        builder.WithGenericConstraint(GenericConstraint.BaseClass("TException", "Exception"));

        // Add constraints for value types
        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private (string resultType, string[] genericParams, GenericConstraint[] constraints) GetResultTypeInfo(ushort arity) {
        if (arity == 0) {
            return ("Result", Array.Empty<string>(), Array.Empty<GenericConstraint>());
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

    private string GenerateHasExceptionBody() {
        return "return !result.IsSuccess && result.Reasons.OfType<IError>().Any(x => x.Exception is TException);";
    }

    private MethodWriter GenerateHasExceptionAsyncMethod(ushort arity, bool isValueTask) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "HasExceptionAsync";
        
        var taskType = isValueTask ? "ValueTask" : "Task";
        var returnType = $"{taskType}<bool>";
        var parameterType = $"{taskType}<{resultType}>";

        var documentationBuilder = DocumentationWriter.Create()
            .WithSummary($"Asynchronously determines whether the result contains an exception of the specified type.")
            .WithTypeParameter("TException", "The type of exception to check for.")
            .WithParameter("awaitableResult", "The awaitable result to check for exceptions.")
            .WithReturns($"A task with true if the result contains an exception of the specified type; otherwise, false.");

        // Add documentation for all value type parameters
        for (int i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateHasExceptionAsyncBody(arity, genericParams);

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(MethodModifier.Static | MethodModifier.Async)
                                  .WithExtensionMethod(parameterType, "awaitableResult")
                                  .WithGenericParameter("TException")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add constraint for TException
        builder.WithGenericConstraint(GenericConstraint.BaseClass("TException", "Exception"));

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private string GenerateHasExceptionAsyncBody(int arity, string[] genericParams) {
        if (arity == 0) {
            return $"""
                     var result = await awaitableResult;
                     return result.HasException<TException>();
                     """;
        }
        return $"""
               var result = await awaitableResult;
               return result.HasException<TException, {string.Join(", ", genericParams)}>();
               """;
    }

    private static string GetOrdinalString(int number) => number switch {
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