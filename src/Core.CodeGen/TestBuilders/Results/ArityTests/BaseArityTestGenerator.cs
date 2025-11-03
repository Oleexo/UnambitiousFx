using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
///     Base class for arity test generators implementing the Template Method pattern.
///     Provides common structure, validation, and directory preparation for all test generators.
///     Extends BaseCodeGenerator to leverage existing infrastructure while adding test-specific functionality.
/// </summary>
internal abstract class BaseArityTestGenerator : BaseCodeGenerator, IArityTestGenerator {
    protected readonly ArityTestGenerationConfig TestConfig;

    protected BaseArityTestGenerator(ArityTestGenerationConfig testConfig)
        : base(testConfig.BaseConfig) {
        TestConfig = testConfig ?? throw new ArgumentNullException(nameof(testConfig));
    }

    /// <summary>
    ///     Generates test code for the specified arity range.
    ///     Uses the base implementation while adding test-specific validation and directory preparation.
    /// </summary>
    public new void Generate(ushort numberOfArity,
                             string outputPath) {
        ValidateTestInputs(numberOfArity, outputPath);
        PrepareTestOutputDirectory(outputPath);

        // Call base implementation which handles the template method pattern
        base.Generate(numberOfArity, outputPath);
    }

    /// <summary>
    ///     Gets the name of the test class that will be generated.
    /// </summary>
    public abstract string GetTestClassName();

    /// <summary>
    ///     Gets the required using statements for the generated test class.
    /// </summary>
    public virtual IEnumerable<string> GetRequiredUsings() {
        return [
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.XUnit"
        ];
    }

    /// <summary>
    ///     Gets the category of tests this generator produces.
    /// </summary>
    public abstract TestGenerationCategory GetCategory();

    /// <summary>
    ///     Validates test generation inputs with additional test-specific checks.
    /// </summary>
    protected override void ValidateInputs(ushort numberOfArity,
                                           string outputPath) {
        // Call base validation first
        base.ValidateInputs(numberOfArity, outputPath);

        // Add test-specific validation
        ValidateTestInputs(numberOfArity, outputPath);
    }

    /// <summary>
    ///     Validates test-specific generation inputs.
    /// </summary>
    protected virtual void ValidateTestInputs(ushort numberOfArity,
                                              string outputPath) {
        if (string.IsNullOrWhiteSpace(TestConfig.TestDirectory)) {
            throw new ArgumentException("Test directory cannot be null or whitespace.", nameof(TestConfig.TestDirectory));
        }

        if (TestConfig.EnabledScenarios == null ||
            !TestConfig.EnabledScenarios.Any()) {
            throw new ArgumentException("At least one test scenario category must be enabled.", nameof(TestConfig.EnabledScenarios));
        }

        if (TestConfig.NamingPattern == null) {
            throw new ArgumentException("Test naming pattern cannot be null.", nameof(TestConfig.NamingPattern));
        }

        // Validate arity range for test generation
        if (numberOfArity > 10) // Reasonable upper limit for test generation
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfArity),
                                                  "Test generation supports maximum arity of 10 to prevent excessive test creation.");
        }
    }

    /// <summary>
    ///     Prepares the test output directory structure.
    ///     Creates necessary subdirectories for test organization.
    /// </summary>
    protected virtual void PrepareTestOutputDirectory(string outputPath) {
        // Ensure the base test directory exists
        FileSystemHelper.EnsureDirectoryExists(TestConfig.TestDirectory);

        // Ensure the output path exists
        FileSystemHelper.EnsureDirectoryExists(outputPath);

        // Create category-specific subdirectory if needed
        var categoryPath = GetCategoryOutputPath(outputPath);
        if (!string.IsNullOrEmpty(categoryPath) &&
            categoryPath != outputPath) {
            FileSystemHelper.EnsureDirectoryExists(categoryPath);
        }
    }

    /// <summary>
    ///     Gets the output path for the specific test category.
    /// </summary>
    protected virtual string GetCategoryOutputPath(string baseOutputPath) {
        var category             = GetCategory();
        var categorySubdirectory = category.ToString();

        return Path.Combine(baseOutputPath, categorySubdirectory);
    }

    /// <summary>
    ///     Generates test classes for a specific arity.
    ///     This is the main template method that derived classes must implement.
    /// </summary>
    protected sealed override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var testClasses = new List<ClassWriter>();

        // Generate tests for each enabled scenario category
        foreach (var scenarioCategory in TestConfig.EnabledScenarios) {
            if (ShouldGenerateForScenario(scenarioCategory, arity)) {
                var classWriter = GenerateTestClassForArity(arity, scenarioCategory);
                if (classWriter != null) {
                    testClasses.Add(classWriter);
                }
            }
        }

        return testClasses.AsReadOnly();
    }

    /// <summary>
    ///     Determines whether tests should be generated for a specific scenario and arity.
    /// </summary>
    protected virtual bool ShouldGenerateForScenario(TestScenarioCategory scenarioCategory,
                                                     ushort               arity) {
        // Skip async scenarios if async tests are disabled
        if (scenarioCategory == TestScenarioCategory.Async &&
            !TestConfig.GenerateAsyncTests) {
            return false;
        }

        // Skip performance scenarios in minimal mode
        if (scenarioCategory == TestScenarioCategory.Performance &&
            TestConfig.Mode  == TestGenerationMode.Minimal) {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Generates a test class for a specific arity and scenario category.
    ///     Derived classes must implement this method to provide category-specific test generation.
    /// </summary>
    protected abstract ClassWriter? GenerateTestClassForArity(ushort               arity,
                                                              TestScenarioCategory scenarioCategory);

    /// <summary>
    ///     Generates a test method name following the configured naming pattern.
    /// </summary>
    protected virtual string GenerateTestMethodName(string               methodName,
                                                    ushort               arity,
                                                    TestScenarioCategory scenario,
                                                    string               expectedBehavior) {
        var pattern        = TestConfig.NamingPattern.TestMethodPattern;
        var scenarioSuffix = GetScenarioSuffix(scenario);

        return pattern
              .Replace("{MethodName}",       methodName)
              .Replace("{Arity}",            arity.ToString())
              .Replace("{Scenario}",         scenarioSuffix)
              .Replace("{ExpectedBehavior}", expectedBehavior);
    }

    /// <summary>
    ///     Gets the suffix for a test scenario category.
    /// </summary>
    protected virtual string GetScenarioSuffix(TestScenarioCategory scenario) {
        return scenario switch {
            TestScenarioCategory.Success   => TestConfig.NamingPattern.SuccessTestSuffix,
            TestScenarioCategory.Failure   => TestConfig.NamingPattern.FailureTestSuffix,
            TestScenarioCategory.Exception => TestConfig.NamingPattern.ExceptionTestSuffix,
            TestScenarioCategory.Async     => TestConfig.NamingPattern.AsyncTestSuffix,
            TestScenarioCategory.EdgeCase  => TestConfig.NamingPattern.EdgeCaseTestSuffix,
            _                              => scenario.ToString()
        };
    }

    /// <summary>
    ///     Creates a base test class writer with common configuration.
    /// </summary>
    protected virtual ClassWriter CreateBaseTestClass(string className,
                                                      ushort arity) {
        var classWriter = new ClassWriter(className, Visibility.Public) {
            Namespace = null, // Let BaseCodeGenerator set the full namespace
            Region    = $"Arity {arity}"
        };

        return classWriter;
    }
}
