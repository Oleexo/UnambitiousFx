using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
///     Generator for ResultFlattenExtensions class.
///     Generates ONE class containing all Flatten methods, organized by arity in regions.
///     Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultFlattenExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly FlattenMethodBuilder _flattenBuilder;

    public ResultFlattenExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   1,
                   ExtensionsNamespace,
                   "ResultFlattenExtensions",
                   FileOrganizationMode.SingleFile))
    {
        _flattenBuilder = new FlattenMethodBuilder();
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

        classWriter.AddMethod(_flattenBuilder.BuildStandaloneMethod(arity));
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

        classWriter.AddMethod(_flattenBuilder.BuildAsyncMethod(arity, isValueTask));
        classWriter.Namespace = ns;
        return classWriter;
    }
}
