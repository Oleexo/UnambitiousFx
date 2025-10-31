using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
/// Generator for ResultZipExtensions class.
/// Generates ONE class containing all Zip methods, organized by arity in regions.
/// Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultZipExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly ZipMethodBuilder _zipBuilder;

    public ResultZipExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 2, // Zip requires at least 2 results
                   subNamespace: ExtensionsNamespace,
                   className: "ResultZipExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile))
    {
        _zipBuilder = new ZipMethodBuilder();
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

        // Generate both variants: with and without projector
        classWriter.AddMethod(_zipBuilder.BuildStandaloneMethod(arity));
        classWriter.AddMethod(_zipBuilder.BuildProjectorMethod(arity));
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
            name: Config.ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        // Generate both async variants: with and without projector
        classWriter.AddMethod(_zipBuilder.BuildAsyncMethod(arity, isValueTask));
        classWriter.AddMethod(_zipBuilder.BuildProjectorAsyncMethod(arity, isValueTask));
        classWriter.Namespace = ns;
        return classWriter;
    }
}
