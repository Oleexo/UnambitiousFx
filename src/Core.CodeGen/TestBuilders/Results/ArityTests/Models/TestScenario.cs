using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents a test scenario with specific conditions and expected outcomes.
/// </summary>
internal sealed class TestScenario {
    /// <summary>
    ///     Initializes a new instance of the TestScenario class.
    /// </summary>
    /// <param name="name">The scenario name.</param>
    /// <param name="type">The scenario type.</param>
    /// <param name="arity">The arity.</param>
    /// <param name="methodName">The method name.</param>
    /// <param name="inputData">The input data.</param>
    /// <param name="expectedResult">The expected result.</param>
    public TestScenario(string               name,
                        TestScenarioCategory type,
                        ushort               arity,
                        string               methodName,
                        TestData             inputData,
                        ExpectedResult       expectedResult) {
        Name           = name ?? throw new ArgumentNullException(nameof(name));
        Type           = type;
        Arity          = arity;
        MethodName     = methodName     ?? throw new ArgumentNullException(nameof(methodName));
        InputData      = inputData      ?? throw new ArgumentNullException(nameof(inputData));
        ExpectedResult = expectedResult ?? throw new ArgumentNullException(nameof(expectedResult));
    }

    /// <summary>
    ///     Gets the name of the test scenario.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Gets the type of test scenario.
    /// </summary>
    public TestScenarioCategory Type { get; init; }

    /// <summary>
    ///     Gets the arity being tested in this scenario.
    /// </summary>
    public ushort Arity { get; init; }

    /// <summary>
    ///     Gets the name of the method being tested.
    /// </summary>
    public string MethodName { get; init; }

    /// <summary>
    ///     Gets the input data for the test scenario.
    /// </summary>
    public TestData InputData { get; init; }

    /// <summary>
    ///     Gets the expected result for the test scenario.
    /// </summary>
    public ExpectedResult ExpectedResult { get; init; }
}
