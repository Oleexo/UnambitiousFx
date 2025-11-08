using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultTryPickErrorExtensions class.
///     Generates TryPickError extension methods for all Result arities.
///     These methods attempt to locate specific attached error reasons via predicate.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultTryPickErrorExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultTryPickErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Start from Result (arity 0)
                   ExtensionsNamespace,
                   "ResultTryPickErrorExtensions",
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
            GenerateTryPickErrorMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateTryPickErrorMethods(ushort arity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate TryPickError method
        classWriter.AddMethod(GenerateTryPickErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateTryPickErrorMethod(ushort arity)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "TryPickError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Attempts to locate a specific attached error reason via predicate.")
                                               .WithParameter("result", "The result to search for errors.")
                                               .WithParameter("predicate", "The predicate function to match errors.")
                                               .WithParameter("error", "When this method returns, contains the first error matching the predicate, or null if no match is found.")
                                               .WithReturns("true if an error matching the predicate was found; otherwise, false.")
                                               .Build();

        var body = GenerateTryPickErrorBody();

        var builder = MethodWriter.Create(methodName, "bool", body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithVisibility(Visibility.Public)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IError, bool>", "predicate")
                                  .WithParameter("out IError?", "error")
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

    private string GenerateTryPickErrorBody()
    {
        return """
               error = result.Reasons.OfType<IError>()
                             .FirstOrDefault(predicate);
               return error is not null;
               """;
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

        // Generate TryPickErrorAsync method for Result -> Task/ValueTask
        classWriter.AddMethod(GenerateTryPickErrorAsyncMethod(arity, isValueTask, false));

        // Generate TryPickErrorAsync method for Task/ValueTask<Result> -> Task/ValueTask
        classWriter.AddMethod(GenerateTryPickErrorAsyncMethod(arity, isValueTask, true));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateTryPickErrorAsyncMethod(ushort arity,
                                                         bool isValueTask,
                                                         bool isAwaitable)
    {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "TryPickErrorAsync";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var returnType = $"{taskType}<(bool Success, IError? Error)>";
        var parameterType = isAwaitable
                                ? $"{taskType}<{resultType}>"
                                : resultType;
        var predicateType = $"Func<IError, {taskType}<bool>>";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Asynchronously attempts to locate a specific attached error reason via predicate.")
                                                      .WithParameter(isAwaitable
                                                                         ? "awaitableResult"
                                                                         : "result", isAwaitable
                                                                                         ? "The awaitable result to search for errors."
                                                                                         : "The result to search for errors.")
                                                      .WithParameter("predicate", "The async predicate function to match errors.")
                                                      .WithReturns(
                                                           "A task containing a tuple with success flag and the first error matching the predicate, or null if no match is found.");

        // Add documentation for all value type parameters
        for (var i = 0; i < genericParams.Length; i++)
        {
            var paramName = genericParams[i];
            var ordinal = GetOrdinalString(i + 1);
            documentationBuilder.WithTypeParameter(paramName, $"The type of the {ordinal} value.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateTryPickErrorAsyncBody(arity, isValueTask, isAwaitable);

        var modifiers = MethodModifier.Static | MethodModifier.Async;

        var builder = MethodWriter.Create(methodName, returnType, body)
                                  .WithModifier(modifiers)
                                  .WithExtensionMethod(parameterType, isAwaitable
                                                                          ? "awaitableResult"
                                                                          : "result")
                                  .WithParameter(predicateType, "predicate")
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

    private string GenerateTryPickErrorAsyncBody(ushort arity,
                                                 bool isValueTask,
                                                 bool isAwaitable)
    {
        if (isAwaitable)
        {
            return """
                   var result = await awaitableResult;
                   return await result.TryPickErrorAsync(predicate);
                   """;
        }

        return """
               foreach (var error in result.Reasons.OfType<IError>()) {
                   if (await predicate(error)) {
                       return (true, error);
                   }
               }
               return (false, null);
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
