using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;

/// <summary>
/// Generator for ResultAccumulateExtensions class.
/// Generates Accumulate extension methods for all Result arities.
/// These methods accumulate errors across operations by applying a mapping function to existing errors
/// and preserving all reasons and metadata from the original result.
/// Follows architecture rule: One generator per extension method category.
/// </summary>
internal sealed class ResultAccumulateExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultAccumulateExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0, // Start from Result (arity 0)
                   subNamespace: ExtensionsNamespace,
                   className: "ResultAccumulateExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [GenerateAccumulateMethods(arity)];
    }

    private ClassWriter GenerateAccumulateMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate Accumulate method
        classWriter.AddMethod(GenerateAccumulateMethod(arity));

        classWriter.Namespace = ns;
        return classWriter;
    }

    private MethodWriter GenerateAccumulateMethod(ushort arity) {
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

    private string GenerateAccumulateBody(ushort arity) {
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

    private string GenerateTryGetCall(ushort arity) {
        if (arity == 0) {
            return "original.TryGet(out var existingError);";
        }

        var outParams = Enumerable.Range(1, arity).Select(_ => "out _");
        var allParams = string.Join(", ", outParams.Concat(["out var existingError"]));
        
        return $"original.TryGet({allParams});";
    }

    private string GenerateFailureCall(ushort arity) {
        if (arity == 0) {
            return "Result.Failure(newEx)";
        }

        if (arity == 1) {
            return "Result.Failure<T1>(newEx)";
        }

        var genericParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));
        return $"Result.Failure<{genericParams}>(newEx)";
    }
}