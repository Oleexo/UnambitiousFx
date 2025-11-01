using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
/// Generator for ResultMapExtensions class.
/// Generates ONE class containing all Map methods, organized by arity in regions.
/// Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultMapExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly MapMethodBuilder _mapBuilder;

    public ResultMapExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 1,
                   subNamespace: ExtensionsNamespace,
                   className: "ResultMapExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile))
    {
        _mapBuilder = new MapMethodBuilder(baseNamespace);
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

        // Sync Map method
        classWriter.AddMethod(_mapBuilder.BuildStandaloneMethod(arity));
        
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

        // Task + sync func overload
        classWriter.AddMethod(_mapBuilder.BuildTaskSyncFuncMethod(arity, isValueTask));

        // Task + async func overload
        classWriter.AddMethod(_mapBuilder.BuildTaskAsyncFuncMethod(arity, isValueTask));

        // Result + async func overloads (MapAsync on Result<T>)
        classWriter.AddMethod(_mapBuilder.BuildAsyncFuncMethod(arity, isValueTask: isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }
}
