using UnambitiousFx.Core.CodeGen.Builders.SideEffects;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.SideEffects;

/// <summary>
///     Generator for ResultTapBothExtensions class.
///     Generates ONE class containing all TapBoth methods, organized by arity in regions.
///     Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultTapBothExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    private readonly TapBothMethodBuilder _tapBothBuilder;

    public ResultTapBothExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0,
                   ExtensionsNamespace,
                   "ResultTapBothExtensions",
                   FileOrganizationMode.SingleFile))
    {
        _tapBothBuilder = new TapBothMethodBuilder();
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
            GenerateAsyncMethods(arity, false),
            GenerateAsyncMethods(arity, true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate sync TapBoth method
        classWriter.AddMethod(_tapBothBuilder.BuildStandaloneMethod(arity));
        classWriter.Namespace = ns;
        return classWriter;
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

        // Generate 3 async overloads
        classWriter.AddMethod(_tapBothBuilder.BuildAsyncFuncMethod(arity, isValueTask));
        classWriter.AddMethod(_tapBothBuilder.BuildTaskSyncFuncMethod(arity, isValueTask));
        classWriter.AddMethod(_tapBothBuilder.BuildTaskAsyncFuncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }
}
