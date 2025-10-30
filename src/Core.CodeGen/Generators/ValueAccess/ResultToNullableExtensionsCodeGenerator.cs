using UnambitiousFx.Core.CodeGen.Builders.ValueAccess;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ValueAccess;

/// <summary>
/// Generator for ResultToNullableExtensions class.
/// Generates ONE class containing all ToNullable methods, organized by arity in regions.
/// Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultToNullableExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    private readonly ToNullableMethodBuilder _toNullableBuilder;
    private readonly AsyncMethodBuilder      _asyncBuilder;

    public ResultToNullableExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 1,
                   subNamespace: ExtensionsNamespace,
                   className: "ResultToNullableExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
        _toNullableBuilder = new ToNullableMethodBuilder(baseNamespace);
        _asyncBuilder      = new AsyncMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateSyncMethods(arity),
            GenerateAsyncMethods(arity, isValueTask: false),
            GenerateAsyncMethods(arity, isValueTask: true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_toNullableBuilder.BuildStandaloneMethod(arity));
        classWriter.Namespace = ns;
        return classWriter;
    }

    private ClassWriter GenerateAsyncMethods(ushort arity,
                                             bool   isValueTask) {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_asyncBuilder.BuildToNullableDefaultAsync(arity, isValueTask));
        classWriter.Namespace = ns;
        return classWriter;
    }
}
