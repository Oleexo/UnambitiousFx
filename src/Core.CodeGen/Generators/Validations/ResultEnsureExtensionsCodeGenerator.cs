using UnambitiousFx.Core.CodeGen.Builders.Validations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Validations;

/// <summary>
///     Generator for Result validation extension methods.
///     Generates Ensure, EnsureNotEmpty, EnsureNotNull extensions, and their async variants.
/// </summary>
internal sealed class ResultEnsureExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.Validations";
    private readonly ValidationAsyncMethodBuilder _asyncBuilder;
    private readonly EnsureMethodBuilder _ensureBuilder;

    public ResultEnsureExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   1,
                   ExtensionsNamespace,
                   "ResultEnsureExtensions",
                   FileOrganizationMode.SingleFile))
    {
        _ensureBuilder = new EnsureMethodBuilder();
        _asyncBuilder = new ValidationAsyncMethodBuilder();
    }

    protected override string PrepareOutputDirectory(string outputPath)
    {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        var results = new List<ClassWriter>();
        results.Add(GenerateEnsurePartial(arity));
        results.AddRange(GenerateEnsureAsyncMethods(arity, false));
        results.AddRange(GenerateEnsureAsyncMethods(arity, true));
        return results;
    }

    #region Ensure

    private ClassWriter GenerateEnsurePartial(ushort arity)
    {
        var classWriter = new ClassWriter(
            "ResultEnsureExtensions",
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_ensureBuilder.BuildStandaloneMethod(arity));
        return classWriter;
    }

    private IEnumerable<ClassWriter> GenerateEnsureAsyncMethods(ushort arity,
                                                                bool isValueTask)
    {
        var subNamespace = isValueTask
                               ? "ValueTasks"
                               : "Tasks";
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{subNamespace}";
        var taskClass = new ClassWriter("ResultExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildEnsureAsync(arity, isValueTask));
        taskClass.Namespace = ns;
        yield return taskClass;

        // Task partial - for Task{Result{T}}
        var taskAwaitableClass = new ClassWriter("ResultExtensions", Visibility.Public,
                                                 ClassModifier.Static | ClassModifier.Partial);
        taskAwaitableClass.AddMethod(_asyncBuilder.BuildEnsureAwaitableAsync(arity, isValueTask));
        taskAwaitableClass.Namespace = ns;
        yield return taskAwaitableClass;
    }

    #endregion
}
