namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
/// Represents a type and its corresponding test value.
/// </summary>
internal sealed class TypeValuePair
{
    /// <summary>
    /// Gets the type name.
    /// </summary>
    public string TypeName { get; init; }

    /// <summary>
    /// Gets the test value for this type.
    /// </summary>
    public string TestValue { get; init; }

    /// <summary>
    /// Gets a value indicating whether this type is nullable.
    /// </summary>
    public bool IsNullable { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is a reference type.
    /// </summary>
    public bool IsReferenceType { get; init; }

    /// <summary>
    /// Initializes a new instance of the TypeValuePair class.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <param name="testValue">The test value.</param>
    /// <param name="isNullable">Whether the type is nullable.</param>
    /// <param name="isReferenceType">Whether this is a reference type.</param>
    public TypeValuePair(string typeName, string testValue, bool isNullable = false, bool isReferenceType = false)
    {
        TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        TestValue = testValue ?? throw new ArgumentNullException(nameof(testValue));
        IsNullable = isNullable;
        IsReferenceType = isReferenceType;
    }
}