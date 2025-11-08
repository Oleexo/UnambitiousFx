using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultRecoveryExtensions class.
///     Generates Recovery extension methods for all Result arities with error recovery scenarios.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultRecoveryExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultRecoveryExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
            baseNamespace,
            1,
            ExtensionsNamespace,
            "ResultRecoveryExtensions",
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
        return
        [
            GenerateSyncMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate both overloads for each arity
        classWriter.AddMethod(GenerateRecoveryWithFunctionMethod(arity));
        classWriter.AddMethod(GenerateRecoveryWithValuesMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
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

        // Generate RecoverAsync method for Task/ValueTask<Result> -> Task/ValueTask<Result>
        classWriter.AddMethod(GenerateRecoveryAsyncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateRecoveryWithFunctionMethod(ushort arity)
    {
        var (resultType, genericParams, constraints, tupleType) = GetResultTypeInfo(arity);
        var methodName = "Recover";

        var documentationBuilder = DocumentationWriter.Create()
            .WithSummary("Recovers from a failed result by providing fallback values through a recovery function.")
            .WithParameter("result", "The result to recover from.")
            .WithParameter("recover", "A function that takes the errors and returns fallback values.")
            .WithReturns(
                "A successful result with the fallback values if the original result failed; otherwise, the original result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = OrdinalHelper.GetOrdinalName(i + 1)
                .ToLowerInvariant();
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateRecoveryWithFunctionBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
            .WithModifier(MethodModifier.Static)
            .WithExtensionMethod(resultType, "result")
            .WithParameter($"Func<IEnumerable<IError>, {tupleType}>", "recover")
            .WithDocumentation(documentation)
            .WithUsings("UnambitiousFx.Core.Results.Reasons");

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

    private MethodWriter GenerateRecoveryWithValuesMethod(ushort arity)
    {
        var (resultType, genericParams, constraints, _) = GetResultTypeInfo(arity);
        var methodName = "Recover";

        var documentationBuilder = DocumentationWriter.Create()
            .WithSummary("Recovers from a failed result by providing specific fallback values.")
            .WithParameter("result", "The result to recover from.")
            .WithReturns(
                "A successful result with the fallback values if the original result failed; otherwise, the original result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = OrdinalHelper.GetOrdinalName(i + 1)
                .ToLowerInvariant();
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");

            var fallbackParamName = $"fallback{i + 1}";
            documentationBuilder.WithParameter(fallbackParamName, $"The fallback value for the {ordinal} parameter.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateRecoveryWithValuesBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
            .WithModifier(MethodModifier.Static)
            .WithExtensionMethod(resultType, "result")
            .WithDocumentation(documentation)
            .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add fallback parameters
        for (var i = 0; i < arity; i++)
        {
            var paramName = $"fallback{i + 1}";
            var paramType = genericParams[i];
            builder.WithParameter(paramType, paramName);
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

    private (string resultType, string[] genericParams, GenericConstraint[] constraints, string tupleType)
        GetResultTypeInfo(ushort arity)
    {
        var genericParams = Enumerable.Range(1, arity)
            .Select(i => $"TValue{i}")
            .ToArray();

        var constraints = genericParams
            .Select(param => GenericConstraint.NotNull(param))
            .ToArray();

        var resultType = arity switch
        {
            0 => "Result",
            1 => "Result<TValue1>",
            _ => $"Result<{string.Join(", ", genericParams)}>"
        };

        var tupleType = arity == 1
            ? "TValue1"
            : $"({string.Join(", ", genericParams)})";

        return (resultType, genericParams, constraints, tupleType);
    }

    private string GenerateRecoveryWithFunctionBody(ushort arity)
    {
        var tryGetParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(_ => "out _"));
        var tryGetCall = $"result.TryGet({tryGetParams}, out var error)";

        var fallbackAssignment = "var fallback = recover(error);";

        var successCall = arity == 1
            ? "return Result.Success(fallback);"
            : GenerateSuccessCallWithTuple(arity);

        return $$"""
                 if ({{tryGetCall}}) {
                     return result;
                 }

                 {{fallbackAssignment}}
                 {{successCall}}
                 """;
    }

    private string GenerateRecoveryWithValuesBody(ushort arity)
    {
        var fallbackParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"fallback{i}"));
        var recoverCall = arity == 1
            ? $"return result.Recover(_ => {fallbackParams});"
            : $"return result.Recover(_ => ({fallbackParams}));";

        return recoverCall;
    }

    private string GenerateSuccessCallWithTuple(ushort arity)
    {
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"fallback.Item{i}"));
        return $"return Result.Success({tupleItems});";
    }

    private MethodWriter GenerateRecoveryAsyncMethod(ushort arity,
        bool isValueTask)
    {
        var (resultType, genericParams, constraints, tupleType) = GetResultTypeInfo(arity);
        var methodName = "RecoverAsync";

        var taskType = isValueTask
            ? "ValueTask"
            : "Task";
        var returnType = $"{taskType}<{resultType}>";
        var parameterType = $"{taskType}<{resultType}>";
        var recoveryType = $"Func<IEnumerable<IError>, {taskType}<{tupleType}>>";

        var documentationBuilder = DocumentationWriter.Create()
            .WithSummary(
                "Asynchronously recovers from a failed result by providing fallback values through a recovery function.")
            .WithParameter("awaitableResult", "The awaitable result to recover from.")
            .WithParameter("recover", "The async function that takes the errors and returns fallback values.")
            .WithReturns(
                "A task with a successful result containing the fallback values if the original result failed; otherwise, the original result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = OrdinalHelper.GetOrdinalName(i + 1)
                .ToLowerInvariant();
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateRecoveryAsyncBody(arity);

        var builder = MethodWriter.Create(methodName, returnType, body)
            .WithModifier(MethodModifier.Static | MethodModifier.Async)
            .WithExtensionMethod(parameterType, "awaitableResult")
            .WithParameter(recoveryType, "recover")
            .WithDocumentation(documentation)
            .WithUsings("UnambitiousFx.Core.Results.Reasons");

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

    private string GenerateRecoveryAsyncBody(ushort arity)
    {
        var tryGetParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(_ => "out _"));
        var tryGetCall = $"result.TryGet({tryGetParams}, out var error)";

        var successCall = arity == 1
            ? "Result.Success(fallback)"
            : GenerateAsyncSuccessCallWithTuple(arity);

        return $$"""
                 var result = await awaitableResult;
                                 
                 if ({{tryGetCall}}) {
                     return result;
                 }
                 var fallback = await recover(error);
                 return {{successCall}};
                 """;
    }

    private string GenerateAsyncSuccessCallWithTuple(ushort arity)
    {
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"fallback.Item{i}"));
        return $"Result.Success({tupleItems})";
    }
}