using System.Diagnostics;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Represents a fluent assertion for an Option that ensures the value is in a "none" state.
/// </summary>
/// <remarks>
///     This struct is used to chain additional assertions or actions after verifying that an Option is none.
/// </remarks>
[DebuggerStepThrough]
public readonly struct NoneAssertion
{
    /// <summary>
    ///     Executes the specified assertion action and returns the current instance of <see cref="NoneAssertion" /> for
    ///     further chaining.
    /// </summary>
    /// <param name="assert">An action representing the assertion to be executed.</param>
    /// <returns>The current instance of <see cref="NoneAssertion" /> to allow method chaining.</returns>
    public NoneAssertion And(Action assert)
    {
        assert();
        return this;
    }
}
