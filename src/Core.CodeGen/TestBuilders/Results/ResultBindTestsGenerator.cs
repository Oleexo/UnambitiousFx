using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultBindTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultBindTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";
    private const int MaxOutputArity = 8;

    public ResultBindTestsGenerator(string baseNamespace,
                                    FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort inputArity)
    {
        var results = new List<ClassWriter>();

        // Generate tests for all output arities (0 to MaxOutputArity) for this input arity
        for (ushort outputArity = 0; outputArity <= MaxOutputArity; outputArity++)
        {
            var scopedOutputArity = outputArity;
            results.AddRange(
                GenerateVariants(inputArity, ClassName,
                                 (ia => GenerateSyncTests(ia, scopedOutputArity), false),
                                 (ia => GenerateAsyncTests(ia, scopedOutputArity, "Task"), true),
                                 (ia => GenerateAsyncTests(ia, scopedOutputArity, "ValueTask"), true)));
        }

        return results;
    }

    #region Helper Methods

    /// <summary>
    ///     Generates the Result creation for the output of the Bind operation.
    ///     This creates Result.Success() or Result.Success(values...) based on output arity.
    /// </summary>
    private string GenerateOutputResultCreation(ushort outputArity)
    {
        if (outputArity == 0)
        {
            return "Result.Success()";
        }

        // Create output values - use simple transformations
        var outputValues = string.Join(", ", Enumerable.Range(1, outputArity)
                                                       .Select(i =>
                                                       {
                                                           var type = GetTestType(i);
                                                           return type switch
                                                           {
                                                               "int" => $"{i * 100}",
                                                               "string" => $"\"output{i}\"",
                                                               "bool" => i % 2 == 0
                                                                             ? "true"
                                                                             : "false",
                                                               "double" => $"{i * 1.5}",
                                                               "long" => $"{i * 1000}L",
                                                               "DateTime" => "DateTime.UtcNow.AddDays(1)",
                                                               "Guid" => "Guid.NewGuid()",
                                                               "TimeSpan" => $"TimeSpan.FromMinutes({i * 10})",
                                                               _ => $"\"output{i}\""
                                                           };
                                                       }));

        return $"Result.Success({outputValues})";
    }

    #endregion

    #region Sync Tests

    private ClassWriter GenerateSyncTests(ushort inputArity,
                                          ushort outputArity)
    {
        var cw = new ClassWriter($"ResultBindSyncTestsInput{inputArity}Output{outputArity}", Visibility.Public)
        {
            Region = $"Input {inputArity} → Output {outputArity} - Sync Bind"
        };

        cw.AddMethod(GenerateSyncSuccessTest(inputArity, outputArity));
        cw.AddMethod(GenerateSyncFailureTest(inputArity, outputArity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort inputArity,
                                                 ushort outputArity)
    {
        return new MethodWriter(
            $"Bind_Input{inputArity}Output{outputArity}_Success_ShouldTransform",
            "void",
            GenerateSyncSuccessBody(inputArity, outputArity),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort inputArity,
                                                 ushort outputArity)
    {
        return new MethodWriter(
            $"Bind_Input{inputArity}Output{outputArity}_Failure_ShouldNotTransform",
            "void",
            GenerateSyncFailureBody(inputArity, outputArity),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort inputArity,
                                           ushort outputArity)
    {
        var givenLines = new List<string>();

        if (inputArity > 0)
        {
            givenLines.Add(GenerateTestValues(inputArity));
        }

        givenLines.Add(GenerateResultCreation(inputArity));

        var call = GenerateBindSyncCall(inputArity, outputArity);
        var assertions = new[] { "Assert.True(transformedResult.IsSuccess);" };

        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateSyncFailureBody(ushort inputArity,
                                           ushort outputArity)
    {
        var creation = GenerateFailureResultCreation(inputArity);
        var call = GenerateBindSyncCall(inputArity, outputArity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateBindSyncCall(ushort inputArity,
                                        ushort outputArity)
    {
        // Generate the bind lambda parameters based on input arity
        var lambdaParams = inputArity == 0 ? "()" :
                           inputArity == 1 ? "x" :
                                             "(" +
                                             string.Join(", ", Enumerable.Range(1, inputArity)
                                                                         .Select(i => $"x{i}")) +
                                             ")";

        // Generate the output result creation based on output arity
        var resultCreation = GenerateOutputResultCreation(outputArity);

        return $"var transformedResult = result.Bind({lambdaParams} => {resultCreation});";
    }

    #endregion

    #region Async Tests

    private ClassWriter GenerateAsyncTests(ushort inputArity,
                                           ushort outputArity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultBind{asyncType}TestsInput{inputArity}Output{outputArity}", Visibility.Public)
        {
            Region = $"Input {inputArity} → Output {outputArity} - {asyncType} Bind"
        };

        // Tests for sync Result with async bind function: Result.BindAsync(async () => ...)
        cw.AddMethod(GenerateSyncResultAsyncBindSuccessTest(inputArity, outputArity, asyncType));
        cw.AddMethod(GenerateSyncResultAsyncBindFailureTest(inputArity, outputArity, asyncType));

        // Tests for async Task<Result> with sync bind function: Task<Result>.BindAsync(() => ...)
        cw.AddMethod(GenerateAsyncResultSyncBindSuccessTest(inputArity, outputArity, asyncType));
        cw.AddMethod(GenerateAsyncResultSyncBindFailureTest(inputArity, outputArity, asyncType));

        // Tests for async Task<Result> with async bind function: Task<Result>.BindAsync(async () => ...)
        cw.AddMethod(GenerateAsyncResultAsyncBindSuccessTest(inputArity, outputArity, asyncType));
        cw.AddMethod(GenerateAsyncResultAsyncBindFailureTest(inputArity, outputArity, asyncType));

        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    // Sync Result with async bind function
    private MethodWriter GenerateSyncResultAsyncBindSuccessTest(ushort inputArity,
                                                                ushort outputArity,
                                                                string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_SyncResult_Input{inputArity}Output{outputArity}_Success_ShouldTransform",
            "async Task",
            GenerateSyncResultAsyncBindSuccessBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateSyncResultAsyncBindFailureTest(ushort inputArity,
                                                                ushort outputArity,
                                                                string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_SyncResult_Input{inputArity}Output{outputArity}_Failure_ShouldNotTransform",
            "async Task",
            GenerateSyncResultAsyncBindFailureBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private string GenerateSyncResultAsyncBindSuccessBody(ushort inputArity,
                                                          ushort outputArity,
                                                          string asyncType)
    {
        var givenLines = new List<string>();

        if (inputArity > 0)
        {
            givenLines.Add(GenerateTestValues(inputArity));
        }

        givenLines.Add(GenerateResultCreation(inputArity));

        var call = GenerateSyncResultAsyncBindCall(inputArity, outputArity, asyncType);
        var assertions = new[] { "Assert.True(transformedResult.IsSuccess);" };

        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateSyncResultAsyncBindFailureBody(ushort inputArity,
                                                          ushort outputArity,
                                                          string asyncType)
    {
        var creation = GenerateFailureResultCreation(inputArity);
        var call = GenerateSyncResultAsyncBindCall(inputArity, outputArity, asyncType);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateSyncResultAsyncBindCall(ushort inputArity,
                                                   ushort outputArity,
                                                   string asyncType)
    {
        // Generate the bind lambda parameters based on input arity
        var lambdaParams = inputArity == 0 ? "()" :
                           inputArity == 1 ? "x" :
                                             "(" +
                                             string.Join(", ", Enumerable.Range(1, inputArity)
                                                                         .Select(i => $"x{i}")) +
                                             ")";

        // Generate the output result creation based on output arity
        var resultCreation = GenerateOutputResultCreation(outputArity);

        // Use Task.FromResult or ValueTask.FromResult depending on asyncType
        var fromResultMethod = asyncType == "Task" ? "Task.FromResult" : "ValueTask.FromResult";

        return $"var transformedResult = await result.BindAsync({lambdaParams} => {fromResultMethod}({resultCreation}));";
    }

    // Async Task<Result> with sync bind function
    private MethodWriter GenerateAsyncResultSyncBindSuccessTest(ushort inputArity,
                                                                ushort outputArity,
                                                                string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_AsyncResult_SyncBind_Input{inputArity}Output{outputArity}_Success_ShouldTransform",
            "async Task",
            GenerateAsyncResultSyncBindSuccessBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateAsyncResultSyncBindFailureTest(ushort inputArity,
                                                                ushort outputArity,
                                                                string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_AsyncResult_SyncBind_Input{inputArity}Output{outputArity}_Failure_ShouldNotTransform",
            "async Task",
            GenerateAsyncResultSyncBindFailureBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private string GenerateAsyncResultSyncBindSuccessBody(ushort inputArity,
                                                          ushort outputArity,
                                                          string asyncType)
    {
        var givenLines = new List<string>();

        if (inputArity > 0)
        {
            givenLines.Add(GenerateTestValues(inputArity));
        }

        givenLines.Add(GenerateAsyncSuccessResultCreation(inputArity, asyncType));

        var call = GenerateAsyncResultSyncBindCall(inputArity, outputArity);
        var assertions = new[] { "Assert.True(transformedResult.IsSuccess);" };

        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateAsyncResultSyncBindFailureBody(ushort inputArity,
                                                          ushort outputArity,
                                                          string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(inputArity, asyncType);
        var call = GenerateAsyncResultSyncBindCall(inputArity, outputArity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateAsyncResultSyncBindCall(ushort inputArity,
                                                   ushort outputArity)
    {
        // Generate the bind lambda parameters based on input arity
        var lambdaParams = inputArity == 0 ? "()" :
                           inputArity == 1 ? "x" :
                                             "(" +
                                             string.Join(", ", Enumerable.Range(1, inputArity)
                                                                         .Select(i => $"x{i}")) +
                                             ")";

        // Generate the output result creation based on output arity
        var resultCreation = GenerateOutputResultCreation(outputArity);

        // Sync bind function - no Task.FromResult needed
        return $"var transformedResult = await taskResult.BindAsync({lambdaParams} => {resultCreation});";
    }

    // Async Task<Result> with async bind function
    private MethodWriter GenerateAsyncResultAsyncBindSuccessTest(ushort inputArity,
                                                                 ushort outputArity,
                                                                 string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_AsyncResult_Input{inputArity}Output{outputArity}_Success_ShouldTransform",
            "async Task",
            GenerateAsyncResultAsyncBindSuccessBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateAsyncResultAsyncBindFailureTest(ushort inputArity,
                                                                 ushort outputArity,
                                                                 string asyncType)
    {
        return new MethodWriter(
            $"BindAsync{asyncType}_AsyncResult_Input{inputArity}Output{outputArity}_Failure_ShouldNotTransform",
            "async Task",
            GenerateAsyncResultAsyncBindFailureBody(inputArity, outputArity, asyncType),
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private string GenerateAsyncResultAsyncBindSuccessBody(ushort inputArity,
                                                           ushort outputArity,
                                                           string asyncType)
    {
        var givenLines = new List<string>();

        if (inputArity > 0)
        {
            givenLines.Add(GenerateTestValues(inputArity));
        }

        givenLines.Add(GenerateAsyncSuccessResultCreation(inputArity, asyncType));

        var call = GenerateAsyncResultAsyncBindCall(inputArity, outputArity, asyncType);
        var assertions = new[] { "Assert.True(transformedResult.IsSuccess);" };

        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateAsyncResultAsyncBindFailureBody(ushort inputArity,
                                                           ushort outputArity,
                                                           string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(inputArity, asyncType);
        var call = GenerateAsyncResultAsyncBindCall(inputArity, outputArity, asyncType);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateAsyncResultAsyncBindCall(ushort inputArity,
                                                    ushort outputArity,
                                                    string asyncType)
    {
        // Generate the bind lambda parameters based on input arity
        var lambdaParams = inputArity == 0 ? "()" :
                           inputArity == 1 ? "x" :
                                             "(" +
                                             string.Join(", ", Enumerable.Range(1, inputArity)
                                                                         .Select(i => $"x{i}")) +
                                             ")";

        // Generate the output result creation based on output arity
        var resultCreation = GenerateOutputResultCreation(outputArity);

        // Use Task.FromResult or ValueTask.FromResult depending on asyncType
        var fromResultMethod = asyncType == "Task" ? "Task.FromResult" : "ValueTask.FromResult";

        return $"var transformedResult = await taskResult.BindAsync({lambdaParams} => {fromResultMethod}({resultCreation}));";
    }

    #endregion
}
