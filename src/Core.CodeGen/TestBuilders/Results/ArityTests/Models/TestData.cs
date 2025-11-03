namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents test data for a specific test scenario.
/// </summary>
internal sealed class TestData {
    /// <summary>
    ///     Initializes a new instance of the TestData class.
    /// </summary>
    /// <param name="arity">The arity.</param>
    /// <param name="values">The type-value pairs.</param>
    /// <param name="expectedState">The expected state.</param>
    /// <param name="setupCode">The setup code.</param>
    public TestData(ushort                     arity,
                    IEnumerable<TypeValuePair> values,
                    ResultState                expectedState,
                    string                     setupCode = "") {
        Arity         = arity;
        Values        = values ?? throw new ArgumentNullException(nameof(values));
        ExpectedState = expectedState;
        SetupCode     = setupCode ?? string.Empty;
    }

    /// <summary>
    ///     Gets the arity of the Result type for this test data.
    /// </summary>
    public ushort Arity { get; init; }

    /// <summary>
    ///     Gets the type-value pairs for the test data.
    /// </summary>
    public IEnumerable<TypeValuePair> Values { get; init; }

    /// <summary>
    ///     Gets the expected Result state for this test data.
    /// </summary>
    public ResultState ExpectedState { get; init; }

    /// <summary>
    ///     Gets the setup code required for this test data.
    /// </summary>
    public string SetupCode { get; init; }
}
