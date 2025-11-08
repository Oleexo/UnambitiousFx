using UnambitiousFx.Core.CodeGen.Builders.Transformations;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators.Transformations;

/// <summary>
///     Generator for ResultBindExtensions class.
///     Bind is unique: it requires BOTH input and output arity variations.
///     Organization: Three files total - one for sync methods, one for Task methods, one for ValueTask methods.
///     Each file contains all input arities (0-8) × all output arities (0-8) for that async type.
///     Generates methods in 3 categories: sync, Task-based async, and ValueTask-based async.
/// </summary>
internal sealed class ResultBindExtensionsCodeGenerator : BaseCodeGenerator
{
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    private readonly BindMethodBuilder _bindBuilder;

    public ResultBindExtensionsCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   0, // Bind starts at 0 (Result with no values)
                   ExtensionsNamespace,
                   "ResultBindExtensions",
                   FileOrganizationMode.SingleFile))
    {
        _bindBuilder = new BindMethodBuilder();
    }

    protected override string PrepareOutputDirectory(string outputPath)
    {
        var mainOutput = FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
        return mainOutput;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        var inputArity = arity;

        return [
            GenerateSyncMethodsForInputArity(inputArity),
            GenerateAsyncMethodsForInputArity(inputArity, false),
            GenerateAsyncMethodsForInputArity(inputArity, true)
        ];
    }

    /// <summary>
    ///     Generates synchronous Bind methods for a given input arity.
    ///     For each input arity, we generate methods for all possible output arities (0-8).
    /// </summary>
    private ClassWriter GenerateSyncMethodsForInputArity(ushort inputArity)
    {
        var ns = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        var classWriter = new ClassWriter(
            Config.ClassName,
            Visibility.Public,
            ClassModifier.Static | ClassModifier.Partial
        );

        // Generate Bind methods for all output arities (0-8)
        for (ushort outputArity = 0; outputArity <= 8; outputArity++)
        {
            classWriter.AddMethod(_bindBuilder.BuildStandaloneMethod(inputArity, outputArity));
        }

        classWriter.Namespace = ns;
        return classWriter;
    }

    /// <summary>
    ///     Generates async Bind methods for a given input arity (Task or ValueTask).
    ///     Produces three async patterns:
    ///     1. Result + async func -> Task/ValueTask
    ///     2. Task/ValueTask + sync func -> Task/ValueTask
    ///     3. Task/ValueTask + async func -> Task/ValueTask
    /// </summary>
    private ClassWriter GenerateAsyncMethodsForInputArity(ushort inputArity,
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

        // Generate async methods for all output arities (0-8)
        for (ushort outputArity = 0; outputArity <= 8; outputArity++)
        {
            // Pattern 1: Result + async func
            classWriter.AddMethod(_bindBuilder.BuildResultWithAsyncFuncMethod(inputArity, outputArity, isValueTask));

            // Pattern 2: Task/ValueTask + sync func
            classWriter.AddMethod(_bindBuilder.BuildAwaitableWithSyncFuncMethod(inputArity, outputArity, isValueTask));

            // Pattern 3: Task/ValueTask + async func
            classWriter.AddMethod(_bindBuilder.BuildAwaitableWithAsyncFuncMethod(inputArity, outputArity, isValueTask));
        }

        classWriter.Namespace = ns;
        return classWriter;
    }
}
