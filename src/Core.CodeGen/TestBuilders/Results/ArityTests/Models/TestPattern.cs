namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
/// Represents the extracted pattern information from existing test files.
/// </summary>
internal sealed class TestPattern
{
    /// <summary>
    /// Gets the naming convention extracted from existing tests.
    /// </summary>
    public NamingConvention NamingConvention { get; init; }

    /// <summary>
    /// Gets the assertion pattern extracted from existing tests.
    /// </summary>
    public AssertionPattern AssertionPattern { get; init; }

    /// <summary>
    /// Gets the common using statements found in existing tests.
    /// </summary>
    public IEnumerable<string> CommonUsings { get; init; }

    /// <summary>
    /// Gets the attribute patterns found in existing tests.
    /// </summary>
    public IEnumerable<AttributePattern> AttributePatterns { get; init; }

    /// <summary>
    /// Gets the directory structure pattern from existing tests.
    /// </summary>
    public DirectoryStructure DirectoryStructure { get; init; }

    /// <summary>
    /// Initializes a new instance of the TestPattern class.
    /// </summary>
    /// <param name="namingConvention">The naming convention.</param>
    /// <param name="assertionPattern">The assertion pattern.</param>
    /// <param name="commonUsings">The common using statements.</param>
    /// <param name="attributePatterns">The attribute patterns.</param>
    /// <param name="directoryStructure">The directory structure.</param>
    public TestPattern(
        NamingConvention namingConvention,
        AssertionPattern assertionPattern,
        IEnumerable<string> commonUsings,
        IEnumerable<AttributePattern> attributePatterns,
        DirectoryStructure directoryStructure)
    {
        NamingConvention = namingConvention ?? throw new ArgumentNullException(nameof(namingConvention));
        AssertionPattern = assertionPattern ?? throw new ArgumentNullException(nameof(assertionPattern));
        CommonUsings = commonUsings ?? throw new ArgumentNullException(nameof(commonUsings));
        AttributePatterns = attributePatterns ?? throw new ArgumentNullException(nameof(attributePatterns));
        DirectoryStructure = directoryStructure ?? throw new ArgumentNullException(nameof(directoryStructure));
    }
}