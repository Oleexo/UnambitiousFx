using UnambitiousFx.Core.CodeGen.Builders.ValueAccess;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.ValueAccess;

/// <summary>
/// Generator for ResultMatchExtensions class.
/// Generates ONE class containing all Match async methods, organized by arity in regions.
/// Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultMatchExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    private readonly AsyncMethodBuilder _asyncBuilder;

    public ResultMatchExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 1,
                   subNamespace: ExtensionsNamespace,
                   className: "ResultMatchExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
        _asyncBuilder = new AsyncMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateAsyncMethods(arity, isValueTask: false),
            GenerateAsyncMethods(arity, isValueTask: true)
        ];
    }

    private ClassWriter GenerateAsyncMethods(ushort arity,
                                             bool   isValueTask) {
        var sub = isValueTask
                      ? "ValueTasks"
                      : "Tasks";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        ) {
            Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{sub}",
        };

        classWriter.AddMethod(_asyncBuilder.BuildMatchAsync(arity, isValueTask));
        return classWriter;
    }
}
