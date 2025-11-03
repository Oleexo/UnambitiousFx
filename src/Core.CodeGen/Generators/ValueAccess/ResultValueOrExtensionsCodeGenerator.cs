using UnambitiousFx.Core.CodeGen.Builders.ValueAccess;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ValueAccess;

/// <summary>
///     Generator for ResultValueOrExtensions class.
///     Generates ONE class containing all ValueOr methods, organized by arity in regions.
///     Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultValueOrExtensionsCodeGenerator : BaseCodeGenerator {
    private const    string             ExtensionsNamespace = "Results.Extensions.ValueAccess";
    private readonly AsyncMethodBuilder _asyncBuilder;

    private readonly ValueOrMethodBuilder _valueOrBuilder;

    public ResultValueOrExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   1,
                   ExtensionsNamespace,
                   "ResultValueOrExtensions",
                   FileOrganizationMode.SingleFile)) {
        _valueOrBuilder = new ValueOrMethodBuilder(baseNamespace);
        _asyncBuilder   = new AsyncMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateSyncMethods(arity),
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";

        // Add methods organized by arity regions
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Add both ValueOr methods for this arity
        classWriter.AddMethod(_valueOrBuilder.BuildWithDefaultFallback(arity));
        classWriter.AddMethod(_valueOrBuilder.BuildWithFactory(arity));
        classWriter.Namespace = ns;


        return classWriter;
    }

    private ClassWriter GenerateAsyncMethods(ushort arity,
                                             bool   isValueTask) {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        // Add methods organized by arity regions
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Add both async ValueOr methods for this arity
        classWriter.AddMethod(_asyncBuilder.BuildValueOrDefaultAsync(arity, isValueTask));
        classWriter.AddMethod(_asyncBuilder.BuildValueOrFactoryAsync(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }
}
