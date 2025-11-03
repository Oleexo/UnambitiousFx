using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
///     Generator for ResultThenExtensions class.
///     Generates ONE class containing all Then methods, organized by arity in regions.
///     Follows architecture rule: One generator per class.
/// </summary>
internal sealed class ResultThenExtensionsCodeGenerator : BaseCodeGenerator {
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly ThenMethodBuilder _thenBuilder;

    public ResultThenExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   1,
                   ExtensionsNamespace,
                   "ResultThenExtensions",
                   FileOrganizationMode.SingleFile)) {
        _thenBuilder = new ThenMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateSyncMethods(arity),
            GenerateAsyncMethodsForTasks(arity, false),
            GenerateAsyncMethodsForTasks(arity, true)
        ];
    }

    private ClassWriter GenerateSyncMethods(ushort arity) {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_thenBuilder.BuildStandaloneMethod(arity));
        classWriter.Namespace = ns;
        return classWriter;
    }

    private ClassWriter GenerateAsyncMethodsForTasks(ushort arity,
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

        // Generate both Task + sync func and Task + async func overloads
        classWriter.AddMethod(_thenBuilder.BuildTaskSyncFuncMethod(arity, isValueTask));
        classWriter.AddMethod(_thenBuilder.BuildTaskAsyncFuncMethod(arity, isValueTask));
        // Also add the Result + async func overload in the main namespace
        classWriter.AddMethod(_thenBuilder.BuildAsyncFuncMethod(arity, isValueTask));

        classWriter.Namespace = ns;
        return classWriter;
    }
}
