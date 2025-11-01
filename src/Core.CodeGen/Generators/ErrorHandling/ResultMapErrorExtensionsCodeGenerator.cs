using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultMapErrorExtensions class.
/// Generates MapError extension methods for all Result arities with both basic and policy-based overloads.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultMapErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultMapErrorExtensions",
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

        // Generate basic MapError method
        classWriter.AddMethod(GenerateBasicMapErrorMethod(arity));

        // Generate policy-based MapError method
        classWriter.AddMethod(GeneratePolicyMapErrorMethod(arity));

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

        // Generate MapErrorAsync method for Result -> Task/ValueTask
        classWriter.AddMethod(GenerateMapErrorAsyncMethod(arity, isValueTask, isAwaitable: false));

        // Generate MapErrorAsync method for Task/ValueTask<Result> -> Task/ValueTask
        classWriter.AddMethod(GenerateMapErrorAsyncMethod(arity, isValueTask, isAwaitable: true));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateBasicMapErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MapError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Maps errors in the result using the specified mapping function.")
                                               .WithParameter("result",   "The result to map errors for.")
                                               .WithParameter("mapError", "The function to map errors.")
                                               .WithReturns($"A new result with mapped errors if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GenerateBasicMapErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IEnumerable<IError>, IEnumerable<IError>>", "mapError")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints
        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints) {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private MethodWriter GeneratePolicyMapErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MapError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Maps errors in the result using the specified mapping function and policy.")
                                               .WithParameter("result",   "The result to map errors for.")
                                               .WithParameter("mapError", "The function to map errors.")
                                               .WithParameter("policy",   "The policy controlling how successive MapError operations behave.")
                                               .WithReturns($"A new result with mapped errors if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GeneratePolicyMapErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IEnumerable<IError>, IEnumerable<IError>>", "mapError")
                                  .WithParameter("MapErrorChainPolicy",                            "policy")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons", "UnambitiousFx.Core.Results.Types");

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
            return ("Result", Array.Empty<string>(), Array.Empty<GenericConstraint>());
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

    private string GenerateBasicMapErrorBody(ushort arity) {
        if (arity == 0) {
            return """
                   return result.Match(
                       Result.Success,
                       ex => Result.Failure(mapError(ex))
                   );
                   """;
        }

        var failureCall = arity == 1
                              ? "Result.Failure<T1>(mapError(ex))"
                              : $"Result.Failure<{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"))}>(mapError(ex))";

        return $"""
                return result.Match(
                    Result.Success,
                    ex => {failureCall}
                );
                """;
    }

    private string GeneratePolicyMapErrorBody(ushort arity) {
        return """
               if (result.IsSuccess) {
                   return result;
               }

               return policy switch {
                   MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
                   MapErrorChainPolicy.Accumulate   => result.Accumulate(mapError),
                   _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
               };
               """;
    }

    private MethodWriter GenerateMapErrorAsyncMethod(ushort arity, bool isValueTask, bool isAwaitable) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MapErrorAsync";
        
        var taskType = isValueTask ? "ValueTask" : "Task";
        var returnType = $"{taskType}<{resultType}>";
        var parameterType = isAwaitable ? $"{taskType}<{resultType}>" : resultType;
        var mapErrorType = $"Func<IEnumerable<IError>, {taskType}<IEnumerable<IError>>>";

        var documentationBuilder = DocumentationWriter.Create()
            .WithSummary($"Asynchronously maps errors in the result using the specified mapping function.")
            .WithParameter(isAwaitable ? "awaitableResult" : "result", isAwaitable ? $"The awaitable result to map errors for." : "The result to map errors for.")
            .WithParameter("mapError", "The async function to map errors.")
            .WithReturns($"A task with a new result containing mapped errors if the original result failed, otherwise the original successful result.");

        // Add documentation for all value type parameters
        for (int i = 0; i < genericParams.Length; i++) {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateMapErrorAsyncBody(arity, isValueTask, isAwaitable);

        var modifiers = MethodModifier.Static;
        if (isAwaitable) {
            modifiers |= MethodModifier.Async;
        }
        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(modifiers)
                                  .WithExtensionMethod(parameterType, isAwaitable ? "awaitableResult" : "result")
                                  .WithParameter(mapErrorType, "mapError")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add using for ValueAccess extensions
        if (isValueTask) {
            builder.WithUsings("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        } else {
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

    private string GenerateMapErrorAsyncBody(ushort arity, bool isValueTask, bool isAwaitable) {
        var (resultType, _, _) = GetResultTypeInfo(arity);
        var taskType = isValueTask ? "ValueTask" : "Task";
        var returnType = $"{taskType}<{resultType}>";

        if (isAwaitable) {
            // For awaitable results, use MatchAsync
            return """
                   var result = await awaitableResult;
                   return await result.MapErrorAsync(mapError);
                   """;
        } else {
            // For non-awaitable results, use Match
            return GenerateNonAwaitableMapErrorBody(arity, isValueTask);
        }
    }

    private string GenerateNonAwaitableMapErrorBody(ushort arity, bool isValueTask) {
        var (resultType, _, _) = GetResultTypeInfo(arity);
        var taskType = isValueTask ? "ValueTask" : "Task";
        var returnType = $"{taskType}<{resultType}>";

        var successCall = GenerateSuccessCall(arity, isValueTask);
        var failureCall = GenerateFailureCall(arity, "await mapError(ex)");

        if (arity == 0) {
            return $@"return result.Match<{returnType}>(
                        () => {successCall},
                        async ex => {failureCall}
                    );";
        }

        var valueParams = Enumerable.Range(1, arity)
                                   .Select(i => $"value{i}")
                                   .ToArray();

        var matchParams = arity == 1 
            ? "value1" 
            : $"({string.Join(",\n             ", valueParams)})";

        return $@"return result.Match<{returnType}>(
                    {matchParams} => {successCall},
                    async ex => {failureCall}
                );";
    }

    private string GenerateAwaitableMapErrorBody(ushort arity, bool isValueTask) {
        var (resultType, _, _) = GetResultTypeInfo(arity);
        var taskType = isValueTask ? "ValueTask" : "Task";

        var successCall = GenerateSuccessCall(arity, isValueTask);
        var failureCall = GenerateFailureCall(arity, "await mapError(ex)");

        if (arity == 0) {
            return $@"return awaitableResult.MatchAsync<{resultType}>(
                        () => {successCall},
                        async ex => {failureCall}
                    );";
        }

        var valueParams = Enumerable.Range(1, arity)
                                   .Select(i => $"value{i}")
                                   .ToArray();

        var matchParams = arity == 1 
            ? "value1" 
            : $"({string.Join(",\n             ", valueParams)})";

        return $@"return awaitableResult.MatchAsync(
                    {matchParams} => {successCall},
                    async ex => {failureCall}
                );";
    }

    private string GenerateSuccessCall(ushort arity, bool isValueTask) {
        if (arity == 0) {
            return isValueTask ? "new ValueTask<Result>(Result.Success())" : "Task.FromResult(Result.Success())";
        }

        var valueParams = Enumerable.Range(1, arity)
                                   .Select(i => $"value{i}")
                                   .ToArray();

        var successCall = $"Result.Success({string.Join(", ", valueParams)})";
        
        return isValueTask 
            ? $"new ValueTask<{GetResultTypeInfo(arity).resultType}>({successCall})" 
            : $"Task.FromResult({successCall})";
    }

    private string GenerateFailureCall(ushort arity, string errorParam) {
        if (arity == 0) {
            return $"Result.Failure({errorParam})";
        }

        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => $"T{i}")
                                      .ToArray();

        return $"Result.Failure<{string.Join(", ", genericParams)}>({errorParam})";
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
