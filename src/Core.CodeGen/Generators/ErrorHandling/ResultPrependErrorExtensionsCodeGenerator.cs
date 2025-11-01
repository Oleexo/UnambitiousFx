using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultPrependErrorExtensions class.
/// Generates PrependError extension methods for all Result arities.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultPrependErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultPrependErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultPrependErrorExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [GeneratePrependErrorMethods(arity)];
    }

    private ClassWriter GeneratePrependErrorMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate PrependError method
        classWriter.AddMethod(GeneratePrependErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GeneratePrependErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "PrependError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Prepends a prefix to the error message of the first error in the result.")
                                               .WithParameter("result", "The result to prepend error message to.")
                                               .WithParameter("prefix", "The prefix to prepend to the error message.")
                                               .WithReturns("A new result with the prepended error message if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GeneratePrependErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("string", "prefix")
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

    private string GeneratePrependErrorBody(ushort arity) {
        return """
               ArgumentNullException.ThrowIfNull(result);
               if (string.IsNullOrEmpty(prefix) || result.IsSuccess) return result; // no-op
               return result.MapError(errs => errs.Select(x => x.WithMessage(prefix + x.Message)));
               """;
    }
}