using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
///     Generator for ResultTryExtensions class.
///     Generates ONE class containing all Try methods, organized by arity in regions.
///     Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultTryExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly TryMethodBuilder _tryBuilder;

    public ResultTryExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   1,
                   ExtensionsNamespace,
                   "ResultTryExtensions",
                   FileOrganizationMode.SingleFile)) {
        _tryBuilder = new TryMethodBuilder(baseNamespace);
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
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Sync Try method
        classWriter.AddMethod(_tryBuilder.BuildStandaloneMethod(arity));


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
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Task + sync func overload
        classWriter.AddMethod(_tryBuilder.BuildTaskSyncFuncMethod(arity, isValueTask));

        // Task + async func overload
        classWriter.AddMethod(_tryBuilder.BuildTaskAsyncFuncMethod(arity, isValueTask));

        // Result + async func overloads (TryAsync on Result<T>)
        classWriter.AddMethod(_tryBuilder.BuildAsyncFuncMethod(arity, isValueTask));


        classWriter.Namespace = ns;
        return classWriter;
    }
}
