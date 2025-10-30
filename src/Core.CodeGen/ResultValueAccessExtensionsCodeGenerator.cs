using UnambitiousFx.Core.CodeGen.Builders.ValueAccess;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
/// Generator for Result value access extension methods.
/// Generates main extensions, partial classes for specialized methods, and async variants.
/// </summary>
internal sealed class ResultValueAccessExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    private readonly ToNullableMethodBuilder _toNullableBuilder;
    private readonly ValueOrMethodBuilder _valueOrBuilder;
    private readonly ValueOrThrowMethodBuilder _valueOrThrowBuilder;
    private readonly AsyncMethodBuilder _asyncBuilder;

    public ResultValueAccessExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 1,
                   directoryName: "Results/Extensions/ValueAccess",
                   className: "ResultValueAccessExtensions"))
    {
        _toNullableBuilder = new ToNullableMethodBuilder(baseNamespace);
        _valueOrBuilder = new ValueOrMethodBuilder(baseNamespace);
        _valueOrThrowBuilder = new ValueOrThrowMethodBuilder(baseNamespace);
        _asyncBuilder = new AsyncMethodBuilder(baseNamespace);
    }

    protected override string PrepareOutputDirectory(string outputPath)
    {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.DirectoryName);
        return mainOutput;
    }

    protected override void GenerateForArityRange(ushort numberOfArity,
                                                  string outputPath)
    {
        var (mainOutput, tasksOutput, valueTasksOutput) = PrepareOutputDirectories(outputPath);

        for (var arity = (ushort)Config.StartArity; arity <= numberOfArity; arity++)
        {
            // Specialized partial classes
            GenerateToNullablePartial(arity, mainOutput);
            GenerateValueOrPartial(arity, mainOutput);
            GenerateValueOrThrowPartial(arity, mainOutput);

            // Async partial classes
            GenerateToNullableAsyncPartial(arity, tasksOutput, valueTasksOutput);
            GenerateValueOrAsyncPartials(arity, tasksOutput, valueTasksOutput);
            GenerateValueOrThrowAsyncPartials(arity, tasksOutput, valueTasksOutput);
            GenerateMatchAsyncPartials(arity, tasksOutput, valueTasksOutput);
        }
    }

    private (string Main, string Tasks, string ValueTasks) PrepareOutputDirectories(string outputPath)
    {
        var tasksOutput = FileSystemHelper.CreateSubdirectory(outputPath, "Tasks");
        var valueTasksOutput = FileSystemHelper.CreateSubdirectory(outputPath, "ValueTasks");
        return (outputPath, tasksOutput, valueTasksOutput);
    }

    #region Specialized Partial Classes

    private void GenerateToNullablePartial(ushort arity,
                                           string outputPath)
    {
        var fileWriter = CreateFileWriter();
        var classWriter = new ClassWriter(
            name: "ResultToNullableExtensions",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_toNullableBuilder.BuildStandaloneMethod(arity));
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"ResultExtensions.ToNullable.{arity}.g.cs");
        FileSystemHelper.WriteFile(fileWriter, fileName);
    }

    private void GenerateValueOrPartial(ushort arity,
                                        string outputPath)
    {
        var fileWriter = CreateFileWriter();
        var classWriter = new ClassWriter(
            name: "ResultValueOrExtensions",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_valueOrBuilder.BuildWithDefaultFallback(arity));
        classWriter.AddMethod(_valueOrBuilder.BuildWithFactory(arity));
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"ResultExtensions.ValueOr.{arity}.g.cs");
        FileSystemHelper.WriteFile(fileWriter, fileName);
    }

    private void GenerateValueOrThrowPartial(ushort arity,
                                             string outputPath)
    {
        var fileWriter = CreateFileWriter();
        var classWriter = new ClassWriter(
            name: "ResultValueOrThrowExtensions",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static | ClassModifier.Partial
        );

        classWriter.AddMethod(_valueOrThrowBuilder.BuildWithDefaultException(arity));
        classWriter.AddMethod(_valueOrThrowBuilder.BuildWithExceptionFactory(arity));
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"ResultExtensions.ValueOrThrow.{arity}.g.cs");
        FileSystemHelper.WriteFile(fileWriter, fileName);
    }

    #endregion

    #region Async Partial Classes

    private void GenerateValueOrAsyncPartials(ushort arity,
                                              string tasksPath,
                                              string valueTasksPath)
    {
        // Task partial
        var taskWriter = CreateFileWriter("Tasks");
        var taskClass = new ClassWriter("ResultValueOrExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildValueOrDefaultAsync(arity, isValueTask: false));
        taskClass.AddMethod(_asyncBuilder.BuildValueOrFactoryAsync(arity, isValueTask: false));
        taskWriter.AddClass(taskClass);
        FileSystemHelper.WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultExtensions.ValueOr.{arity}.g.cs"));

        // ValueTask partial
        var vtWriter = CreateFileWriter("ValueTasks");
        var vtClass = new ClassWriter("ResultValueOrExtensions", Visibility.Public,
                                      ClassModifier.Static | ClassModifier.Partial);
        vtClass.AddMethod(_asyncBuilder.BuildValueOrDefaultAsync(arity, isValueTask: true));
        vtClass.AddMethod(_asyncBuilder.BuildValueOrFactoryAsync(arity, isValueTask: true));
        vtWriter.AddClass(vtClass);
        FileSystemHelper.WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultExtensions.ValueOr.{arity}.g.cs"));
    }

    private void GenerateValueOrThrowAsyncPartials(ushort arity,
                                                   string tasksPath,
                                                   string valueTasksPath)
    {
        // Task partial
        var taskWriter = CreateFileWriter("Tasks");
        var taskClass = new ClassWriter("ResultValueOrThrowExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildValueOrThrowDefaultAsync(arity, isValueTask: false));
        taskClass.AddMethod(_asyncBuilder.BuildValueOrThrowFactoryAsync(arity, isValueTask: false));
        taskWriter.AddClass(taskClass);
        FileSystemHelper.WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultExtensions.ValueOrThrow.{arity}.g.cs"));

        // ValueTask partial
        var vtWriter = CreateFileWriter("ValueTasks");
        var vtClass = new ClassWriter("ResultValueOrThrowExtensions", Visibility.Public,
                                      ClassModifier.Static | ClassModifier.Partial);
        vtClass.AddMethod(_asyncBuilder.BuildValueOrThrowDefaultAsync(arity, isValueTask: true));
        vtClass.AddMethod(_asyncBuilder.BuildValueOrThrowFactoryAsync(arity, isValueTask: true));
        vtWriter.AddClass(vtClass);
        FileSystemHelper.WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultExtensions.ValueOrThrow.{arity}.g.cs"));
    }

    private void GenerateToNullableAsyncPartial(ushort arity,
                                                string tasksPath,
                                                string valueTasksPath)
    {
        // Task partial
        var taskWriter = CreateFileWriter("Tasks");
        var taskClass = new ClassWriter("ResultToNullableExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildToNullableDefaultAsync(arity, isValueTask: false));
        taskWriter.AddClass(taskClass);
        FileSystemHelper.WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultExtensions.ToNullable.{arity}.g.cs"));

        // ValueTask partial
        var vtWriter = CreateFileWriter("ValueTasks");
        var vtClass = new ClassWriter("ResultToNullableExtensions", Visibility.Public,
                                      ClassModifier.Static | ClassModifier.Partial);
        vtClass.AddMethod(_asyncBuilder.BuildToNullableDefaultAsync(arity, isValueTask: true));
        vtWriter.AddClass(vtClass);
        FileSystemHelper.WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultExtensions.ToNullable.{arity}.g.cs"));
    }

    private void GenerateMatchAsyncPartials(ushort arity,
                                            string tasksPath,
                                            string valueTasksPath)
    {
        // Task partial
        var taskWriter = CreateFileWriter("Tasks");
        var taskClass = new ClassWriter("ResultMatchExtensions", Visibility.Public,
                                        ClassModifier.Static | ClassModifier.Partial);
        taskClass.AddMethod(_asyncBuilder.BuildMatchAsync(arity, isValueTask: false));
        taskWriter.AddClass(taskClass);
        FileSystemHelper.WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultExtensions.Match.{arity}.g.cs"));

        // ValueTask partial
        var vtWriter = CreateFileWriter("ValueTasks");
        var vtClass = new ClassWriter("ResultMatchExtensions", Visibility.Public,
                                      ClassModifier.Static | ClassModifier.Partial);
        vtClass.AddMethod(_asyncBuilder.BuildMatchAsync(arity, isValueTask: true));
        vtWriter.AddClass(vtClass);
        FileSystemHelper.WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultExtensions.Match.{arity}.g.cs"));
    }

    #endregion

    private FileWriter CreateFileWriter(string? sub = null)
    {
        return sub != null
                   ? new FileWriter($"{Config.BaseNamespace}.{ExtensionsNamespace}.{sub}")
                   : new FileWriter($"{Config.BaseNamespace}.{ExtensionsNamespace}");
    }
}
