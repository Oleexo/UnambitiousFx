using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
///     Generator for ResultShapeErrorExtensions class.
///     Generates ShapeError extension methods for all Result arities to transform error structure.
///     Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultShapeErrorExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultShapeErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0,
                   ExtensionsNamespace,
                   "ResultShapeErrorExtensions",
                   FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [GenerateShapeErrorMethods(arity)];
    }

    private ClassWriter GenerateShapeErrorMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate ShapeError method
        classWriter.AddMethod(GenerateShapeErrorMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateShapeErrorMethod(ushort arity) {
        var (resultType, genericParams, constraints) = GetResultTypeInfo(arity);
        var methodName = "ShapeError";

        var documentation = DocumentationWriter.Create()
                                               .WithSummary("Transforms the error structure of the result using the specified shaping function.")
                                               .WithParameter("result", "The result to shape errors for.")
                                               .WithParameter("shape",  "The function to transform the error structure.")
                                               .WithReturns(
                                                    "A new result with transformed error structure if the original result failed, otherwise the original successful result.")
                                               .Build();

        var body = GenerateShapeErrorBody(arity);

        var builder = MethodWriter.Create(methodName, resultType, body)
                                  .WithModifier(MethodModifier.Static)
                                  .WithExtensionMethod(resultType, "result")
                                  .WithParameter("Func<IEnumerable<IError>, IEnumerable<IError>>", "shape")
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

    private string GenerateShapeErrorBody(ushort arity) {
        if (arity == 0) {
            return """
                   return result.IsSuccess
                       ? result
                       : ResultExtensions.Preserve(result, result.MapError(shape));
                   """;
        }

        return """
               return result.IsSuccess
                   ? result
                   : ResultExtensions.Preserve(result, result.MapError(shape));
               """;
    }
}
