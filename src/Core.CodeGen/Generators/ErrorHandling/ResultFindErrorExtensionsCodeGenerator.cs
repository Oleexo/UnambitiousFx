using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultFindErrorExtensions class.
/// Generates FindError and TryPickError extension methods for all Result arities.
/// These methods locate specific attached error reasons via predicate.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultFindErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFindErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultFindErrorExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [GenerateFindErrorMethods(arity)];
    }

    private ClassWriter GenerateFindErrorMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate FindError method
        classWriter.AddMethod(GenerateFindErrorMethod(arity));
        
        // Generate TryPickError method
        classWriter.AddMethod(GenerateTryPickErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateFindErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "FindError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Locates a specific attached error reason via predicate.")
                                               .WithParameter("result", "The result to search for errors.")
                                               .WithParameter("predicate", "The predicate function to match errors.")
                                               .WithReturns("The first error matching the predicate, or null if no match is found.")
                                               .Build();

        var body = GenerateFindErrorBody();

        var builder = MethodWriter.Create(methodName, "IError?", body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithVisibility(Visibility.Public)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IError, bool>", "predicate")
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

    private MethodWriter GenerateTryPickErrorMethod(ushort arity) {
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

    private string GenerateFindErrorBody() {
        return """
               ArgumentNullException.ThrowIfNull(predicate);

               return result.Reasons.OfType<IError>()
                            .FirstOrDefault(predicate);
               """;
    }

    private string GenerateTryPickErrorBody() {
        return """
               ArgumentNullException.ThrowIfNull(predicate);

               error = result.Reasons.OfType<IError>()
                             .FirstOrDefault(predicate);
               return error is not null;
               """;
    }
}