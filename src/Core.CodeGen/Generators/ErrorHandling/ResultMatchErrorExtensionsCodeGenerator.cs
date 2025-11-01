using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultMatchErrorExtensions class.
/// Generates MatchError extension methods for all Result arities for pattern matching on error types.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultMatchErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMatchErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultMatchErrorExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [GenerateMatchErrorMethods(arity)];
    }

    private ClassWriter GenerateMatchErrorMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate MatchError method
        classWriter.AddMethod(GenerateMatchErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateMatchErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "MatchError";

        var documentationBuilder = DocumentationWriter.Create()
                                                      .WithSummary("Matches the first error of the specified type in the result and executes the corresponding function.")
                                                      .WithTypeParameter("TError", "The type of error to match. Must be a class implementing IError.")
                                                      .WithTypeParameter("TOut", "The type of the output value.")
                                                      .WithParameter("result", "The result to match errors for.")
                                                      .WithParameter("onMatch", "The function to execute when a matching error is found.")
                                                      .WithParameter("onElse", "The function to execute when no matching error is found.")
                                                      .WithReturns("The result of executing either onMatch or onElse function.");

        // Add documentation for value type parameters
        foreach (var param in genericParams) {
            documentationBuilder.WithTypeParameter(param, $"The type of the {GetOrdinalName(Array.IndexOf(genericParams, param) + 1)} value in the result.");
        }

        var documentation = documentationBuilder.Build();

        var body = GenerateMatchErrorBody(arity);

        var builder = MethodWriter.Create(methodName, "TOut", body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<TError, TOut>", "onMatch")
                                  .WithParameter("Func<TOut>", "onElse")
                                  .WithDocumentation(documentation)
                                  .WithUsings("UnambitiousFx.Core.Results.Reasons");

        // Add generic parameters and constraints
        builder.WithGenericParameter("TError");

        foreach (var param in genericParams) {
            builder.WithGenericParameter(param);
        }

        builder.WithGenericParameter("TOut");

        // Add constraints for TError
        builder.WithGenericConstraint("TError", "class")
               .WithGenericConstraint("TError", "IError");

        // Add constraints for value type parameters
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

    private string GenerateMatchErrorBody(ushort arity) {
        return """
               ArgumentNullException.ThrowIfNull(onMatch);
               ArgumentNullException.ThrowIfNull(onElse);

               var match = result.Reasons.OfType<TError>()
                                 .FirstOrDefault();
               return match is not null
                          ? onMatch(match)
                          : onElse();
               """;
    }

    private static string GetOrdinalName(int number) {
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