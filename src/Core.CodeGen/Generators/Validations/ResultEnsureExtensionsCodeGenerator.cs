using UnambitiousFx.Core.CodeGen.Builders.Validations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Validations;

/// <summary>
/// Generator for Result validation extension methods.
/// Generates Ensure, EnsureNotEmpty, EnsureNotNull extensions, and their async variants.
/// </summary>
internal sealed class ResultEnsureExtensionsCodeGenerator : BaseCodeGenerator {
    private readonly EnsureMethodBuilder          _ensureBuilder;
    private readonly ValidationAsyncMethodBuilder _asyncBuilder;
    private const    string                       ExtensionsNamespace = "Results.Extensions.Validations";

    public ResultEnsureExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 1,
                   subNamespace: ExtensionsNamespace,
                   className: "ResultEnsureExtensions",
                   fileOrganization: FileOrganizationMode.SingleFile)) {
        _ensureBuilder = new EnsureMethodBuilder(baseNamespace);
        _asyncBuilder  = new ValidationAsyncMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath) {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var results = new List<ClassWriter>();
        results.Add(GenerateEnsurePartial(arity));
        results.AddRange(GenerateEnsureAsyncMethods(arity, isValueTask: false));
        results.AddRange(GenerateEnsureAsyncMethods(arity, isValueTask: true));
        return results;
    }

    #region Ensure

    private ClassWriter GenerateEnsurePartial(ushort arity) {
        var classWriter = new ClassWriter(
            name: "ResultEnsureExtensions",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_ensureBuilder.BuildStandaloneMethod(arity));
        return classWriter;
    }

    private IEnumerable<ClassWriter> GenerateEnsureAsyncMethods(ushort arity,
                                                                bool   isValueTask) {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";
        var taskClass = new ClassWriter("ResultExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildEnsureAsync(arity, isValueTask: isValueTask));
        taskClass.Namespace = ns;
        yield return taskClass;

        // Task partial - for Task{Result{T}}
        var taskAwaitableClass = new ClassWriter("ResultExtensions", Visibility.Public,
                                                 ClassModifier.Static | ClassModifier.Partial);
        taskAwaitableClass.AddMethod(_asyncBuilder.BuildEnsureAwaitableAsync(arity, isValueTask: isValueTask));
        taskAwaitableClass.Namespace = ns;
        yield return taskAwaitableClass;
    }

    #endregion
}
