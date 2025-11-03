namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
///     Defines the categories of test generation for Result types.
/// </summary>
internal enum TestGenerationCategory {
    /// <summary>
    ///     Tests for Result direct methods (Match, IfSuccess, IfFailure, TryGet).
    /// </summary>
    DirectMethods,

    /// <summary>
    ///     Tests for transformation methods (Map, Bind, Flatten, Zip, Try).
    /// </summary>
    Transformations,

    /// <summary>
    ///     Tests for validation methods (Ensure).
    /// </summary>
    Validation,

    /// <summary>
    ///     Tests for side effect methods (Tap, TapBoth, TapError).
    /// </summary>
    SideEffects,

    /// <summary>
    ///     Tests for error handling methods (MapError, MapErrors, PrependError, AppendError, HasError, HasException,
    ///     FindError, MatchError, FilterError, Recover).
    /// </summary>
    ErrorHandling,

    /// <summary>
    ///     Tests for value access methods (ValueOr, ValueOrThrow, ToNullable).
    /// </summary>
    ValueAccess
}
