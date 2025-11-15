using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultMapErrorsExtensions class.
///     Generates MapErrors extension methods for all Result arities that transform error structure.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultMapErrorsExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorsExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultMapErrorsExtensions",
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

        // Generate MapErrors method
        classWriter.AddMethod(GenerateMapErrorsMethod(arity));

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

        // Generate MapErrorsAsync method for Task/ValueTask<Result> -> Task/ValueTask<Result>
        classWriter.AddMethod(GenerateMapErrorsAsyncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateMapErrorsMethod(ushort arity)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MapErrors";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Maps all errors in the result using the specified mapping function to transform the error structure.")
                                               .WithParameter("result", "The result to map errors for.")
                                               .WithParameter("map", "The function to map the collection of exceptions to a single exception.")
                                               .WithReturns("A new result with mapped errors if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GenerateMapErrorsBody(arity, genericParams);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IEnumerable<IError>, IError>", "map")
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

    private string GenerateMapErrorsBody(ushort arity,
                                         string[] genericParams)
    {
        var successCheck = arity == 0
                               ? "result.TryGet(out _)"
                               : "result.IsSuccess";

        if (arity == 0)
        {
            return $$"""
                     if ({{successCheck}}) {
                       return result;
                     }


                     return result.Match(Result.Success,
                                  (errors) => Result.Failure(map(errors)));
                     """;
        }

        return $$"""
                 if ({{successCheck}}) {
                     return result;
                 }

                 return result.Match(Result.Success,
                              (errors) => Result.Failure<{{string.Join(",", genericParams)}}>(map(errors)));
                 """;
    }

    private MethodWriter GenerateMapErrorsAsyncMethod(ushort arity,
                                                      bool isValueTask)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MapErrorsAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType = $"{taskType}<{resultType}>";
        var parameterType = $"{taskType}<{resultType}>";
        var mapType = $"Func<IEnumerable<IError>, {taskType}<IError>>";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Asynchronously maps errors in the result to a new exception using the specified mapping function.")
                                                      .WithParameter("awaitableResult", "The awaitable result to map errors for.")
                                                      .WithParameter("map", "The async function to map errors to a new exception.")
                                                      .WithReturns(
                                                           "A task with a new result containing the mapped exception if the original result failed, otherwise the original successful result.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateMapErrorsAsyncBody(arity, isValueTask, genericParams);

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(MethodModifier.Static | MethodModifier.Async)
                                  .WithExtensionMethod(parameterType, "awaitableResult")
                                  .WithParameter(mapType, "map")
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

    private string GenerateMapErrorsAsyncBody(ushort arity,
                                              bool isValueTask,
                                              string[] genericParams)
    {
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";

        var inputParameters = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(x => $"v{x}"));

        if (arity == 0)
        {
            return $$"""
                     var result = await awaitableResult;
                                     
                     if (result.IsSuccess) {
                         return result;
                     }

                     return await result.Match<{{taskType}}<Result>>(() => {{taskType}}.FromResult(Result.Success()), 
                                                            async (errors) => {
                                                                var error = await map(errors);
                                                                return Result.Failure(error);
                                                            });
                     """;
        }

        return $$"""
                 var result = await awaitableResult;
                                        
                 if (result.IsSuccess) {
                     return result;
                 }

                 return await result.Match<{{taskType}}<Result<{{string.Join(",", genericParams)}}>>>(({{inputParameters}}) => {{taskType}}.FromResult(Result.Success<{{string.Join(",", genericParams)}}>({{inputParameters}})), 
                                                                              async (errors) => {
                                                                                  var error = await map(errors);
                                                                                  return Result.Failure<{{string.Join(",", genericParams)}}>(error);
                                                                              });
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
