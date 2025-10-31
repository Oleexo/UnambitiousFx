using UnambitiousFx.Core.CodeGen.Builders.SideEffects;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.SideEffects;

/// <summary>
/// Generator for ResultTapErrorExtensions class.
/// Generates ONE class containing all TapError methods, organized by arity in regions.
/// Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultTapErrorExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    private readonly TapErrorMethodBuilder _tapErrorBuilder;

    public ResultTapErrorExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0,
                   subNamespace: ExtensionsNamespace,
                   className: "ResultTapErrorExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile))
    {
        _tapErrorBuilder = new TapErrorMethodBuilder();
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
            GenerateAsyncMethods(arity, isValueTask: false),
            GenerateAsyncMethods(arity, isValueTask: true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate sync TapError method
        classWriter.AddMethod(_tapErrorBuilder.BuildStandaloneMethod(arity));
        classWriter.Namespace = ns;
        return classWriter;
    }

    private ClassWriter GenerateAsyncMethods(ushort arity, bool isValueTask)
    {
        var subNamespace = isValueTask ? "ValueTasks" : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";

        var classWriter = new ClassWriter(
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate 3 async overloads
        classWriter.AddMethod(_tapErrorBuilder.BuildAsyncFuncMethod(arity, isValueTask));
        classWriter.AddMethod(_tapErrorBuilder.BuildTaskSyncFuncMethod(arity, isValueTask));
        classWriter.AddMethod(_tapErrorBuilder.BuildTaskAsyncFuncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }
}
