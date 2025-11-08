using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultAccumulateExtensions class.
///     Generates Accumulate extension methods for all Result arities.
///     These methods accumulate errors across operations by applying a mapping function to existing errors
///     and preserving all reasons and metadata from the original result.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultAccumulateExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultAccumulateExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultAccumulateExtensions",
                   FileOrganizationMode.SingleFile))
    {
    }

    protected override string PrepareOutputDirectory(string outputPath)
    {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        return [
            GenerateAccumulateMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateAccumulateMethods(ushort arity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate Accumulate method
        classWriter.AddMethod(GenerateAccumulateMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateAccumulateMethod(ushort arity)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "Accumulate";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Accumulates errors by applying a mapping function to existing errors and preserving all reasons and metadata.")
                                               .WithParameter("original", "The original result to accumulate errors from.")
                                               .WithParameter("mapError", "The function to map and accumulate errors.")
                                               .WithReturns("A new result with accumulated errors, preserving all reasons and metadata from the original result.")
                                               .Build();

        var body = GenerateAccumulateBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithVisibility(Visibility.Internal)
                                  .WithExtensionMethod(resultType, "original")
                                  .WithParameter("Func<IEnumerable<IError>, IEnumerable<IError>>", "mapError")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints
        foreach (var param in genericParams)
        {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints)
        {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private (string resultType, string[] genericParams, GenericConstraint[] constraints) GetResultTypeInfo(ushort arity)
    {
        if (arity == 0)
        {
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

    private string GenerateAccumulateBody(ushort arity)
    {
        var tryGetCall = GenerateTryGetCall(arity);
        var failureCall = GenerateFailureCall(arity);

        return $$"""
                 {{tryGetCall}}
                 var newEx = mapError(existingError!);
                 var mapped = {{failureCall}};
                 foreach (var r in original.Reasons) {
                     mapped.AddReason(r);
                 }

                 foreach (var kv in original.Metadata) {
                     mapped.AddMetadata(kv.Key, kv.Value);
                 }

                 return mapped;
                 """;
    }

    private string GenerateTryGetCall(ushort arity)
    {
        if (arity == 0)
        {
            return "original.TryGet(out var existingError);";
        }

        var outParams = Enumerable.Range(1, arity)
                                  .Select(_ => "out _");
        var allParams = string.Join(", ", outParams.Concat(["out var existingError"]));

        return $"original.TryGet({allParams});";
    }

    private string GenerateFailureCall(ushort arity)
    {
        if (arity == 0)
        {
            return "Result.Failure(newEx)";
        }

        if (arity == 1)
        {
            return "Result.Failure<T1>(newEx)";
        }

        var genericParams = string.Join(", ", Enumerable.Range(1, arity)
                                                        .Select(i => $"T{i}"));
        return $"Result.Failure<{genericParams}>(newEx)";
    }

    private ClassWriter GenerateAsyncMethods(ushort arity,
                                             bool isValueTask)
    {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate AccumulateAsync method for Result -> Task/ValueTask
        classWriter.AddMethod(GenerateAccumulateAsyncMethod(arity, isValueTask, false));

        // Generate AccumulateAsync method for Task/ValueTask<Result> -> Task/ValueTask
        classWriter.AddMethod(GenerateAccumulateAsyncMethod(arity, isValueTask, true));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateAccumulateAsyncMethod(ushort arity,
                                                       bool isValueTask,
                                                       bool isAwaitable)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "AccumulateAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType = $"{taskType}<{resultType}>";
        var parameterType = isAwaitable
                                ? $"{taskType}<{resultType}>"
                                : resultType;
        var mapErrorType = $"Func<IEnumerable<IError>, {taskType}<IEnumerable<IError>>>";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary(
                                                           "Asynchronously accumulates errors by applying a mapping function to existing errors and preserving all reasons and metadata.")
                                                      .WithParameter(isAwaitable
                                                                         ? "awaitableOriginal"
                                                                         : "original", isAwaitable
                                                                                           ? "The awaitable original result to accumulate errors from."
                                                                                           : "The original result to accumulate errors from.")
                                                      .WithParameter("mapError", "The async function to map and accumulate errors.")
                                                      .WithReturns(
                                                           "A task with a new result containing accumulated errors, preserving all reasons and metadata from the original result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateAccumulateAsyncBody(arity, isValueTask, isAwaitable);

        var modifiers = MethodModifier.Static | MethodModifier.Async;

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(modifiers)
                                  .WithVisibility(Visibility.Internal)
                                  .WithExtensionMethod(parameterType, isAwaitable
                                                                          ? "awaitableOriginal"
                                                                          : "original")
                                  .WithParameter(mapErrorType, "mapError")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add using for ValueAccess extensions
        if (isValueTask)
        {
            builder.WithUsings("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        }
        else
        {
            builder.WithUsings("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        }

        // Add generic parameters and constraints for value types
        foreach (var param in genericParams)
        {
            builder.WithGenericParameter(param);
        }

        foreach (var constraint in constraints)
        {
            builder.WithGenericConstraint(constraint);
        }

        return builder.Build();
    }

    private string GenerateAccumulateAsyncBody(ushort arity,
                                               bool isValueTask,
                                               bool isAwaitable)
    {
        if (isAwaitable)
        {
            return """
                   var original = await awaitableOriginal;
                   return await original.AccumulateAsync(mapError);
                   """;
        }

        var tryGetCall = GenerateTryGetCall(arity);
        var failureCall = GenerateFailureCall(arity);
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType = $"{taskType}<{GetResultTypeInfo(arity).resultType}>";

        return $$"""
                 {{tryGetCall}}
                 var newEx = await mapError(existingError!);
                 var mapped = {{failureCall}};
                 foreach (var r in original.Reasons) {
                     mapped.AddReason(r);
                 }
                 foreach (var kv in original.Metadata) {
                     mapped.AddMetadata(kv.Key, kv.Value);
                 }
                 return mapped;
                 """;
    }

    private static string GetOrdinalString(int number)
    {
        return number switch
        {
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
