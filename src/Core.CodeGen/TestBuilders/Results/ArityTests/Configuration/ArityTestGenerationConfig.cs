using UnambitiousFx.Core.CodeGen.Configuration;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;

/// <summary>
///     Configuration for arity test generation operations, using composition with GenerationConfig.
/// </summary>
internal sealed class ArityTestGenerationConfig
{
    /// <summary>
    ///     Initializes a new instance of the ArityTestGenerationConfig class.
    /// </summary>
    /// <param name="baseNamespace">The base namespace for generated tests.</param>
    /// <param name="testDirectory">The directory where test files will be generated.</param>
    /// <param name="startArity">The starting arity for generation.</param>
    /// <param name="subNamespace">The sub-namespace for organization.</param>
    /// <param name="className">The class name prefix.</param>
    /// <param name="fileOrganization">The file organization mode.</param>
    /// <param name="namingPattern">The naming pattern for tests.</param>
    /// <param name="enabledScenarios">The enabled test scenario categories.</param>
    /// <param name="mode">The test generation mode.</param>
    /// <param name="generateAsyncTests">Whether to generate async tests.</param>
    /// <param name="generateErrorHandlingTests">Whether to generate error handling tests.</param>
    public ArityTestGenerationConfig(string baseNamespace,
                                     string testDirectory,
                                     int startArity = 1,
                                     string subNamespace = "",
                                     string className = "",
                                     FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles,
                                     TestNamingPattern? namingPattern = null,
                                     IEnumerable<TestScenarioCategory>? enabledScenarios = null,
                                     TestGenerationMode mode = TestGenerationMode.Comprehensive,
                                     bool generateAsyncTests = true,
                                     bool generateErrorHandlingTests = true)
    {
        if (string.IsNullOrWhiteSpace(testDirectory))
        {
            throw new ArgumentException("Test directory cannot be null or whitespace.", nameof(testDirectory));
        }

        BaseConfig = new GenerationConfig(baseNamespace, startArity, subNamespace, className, fileOrganization, true);
        TestDirectory = testDirectory;
        NamingPattern = namingPattern ?? new TestNamingPattern();
        EnabledScenarios = enabledScenarios ?? GetDefaultEnabledScenarios();
        Mode = mode;
        GenerateAsyncTests = generateAsyncTests;
        GenerateErrorHandlingTests = generateErrorHandlingTests;
    }

    /// <summary>
    ///     Gets the base generation configuration.
    /// </summary>
    public GenerationConfig BaseConfig { get; init; }

    /// <summary>
    ///     Gets the directory where test files will be generated.
    /// </summary>
    public string TestDirectory { get; init; }

    /// <summary>
    ///     Gets a value indicating whether async tests should be generated.
    /// </summary>
    public bool GenerateAsyncTests { get; init; }

    /// <summary>
    ///     Gets a value indicating whether error handling tests should be generated.
    /// </summary>
    public bool GenerateErrorHandlingTests { get; init; }

    /// <summary>
    ///     Gets the naming pattern for generated tests.
    /// </summary>
    public TestNamingPattern NamingPattern { get; init; }

    /// <summary>
    ///     Gets the enabled test scenario categories.
    /// </summary>
    public IEnumerable<TestScenarioCategory> EnabledScenarios { get; init; }

    /// <summary>
    ///     Gets the test generation mode.
    /// </summary>
    public TestGenerationMode Mode { get; init; }

    private static IEnumerable<TestScenarioCategory> GetDefaultEnabledScenarios()
    {
        return [
            TestScenarioCategory.Success,
            TestScenarioCategory.Failure,
            TestScenarioCategory.Exception,
            TestScenarioCategory.EdgeCase
        ];
    }
}
