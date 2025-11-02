namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
/// Represents directory structure patterns from existing test files.
/// </summary>
internal sealed class DirectoryStructure
{
    /// <summary>
    /// Gets the base test directory path.
    /// </summary>
    public string BaseDirectory { get; init; }

    /// <summary>
    /// Gets the subdirectory patterns used for organizing tests.
    /// </summary>
    public IEnumerable<string> SubdirectoryPatterns { get; init; }

    /// <summary>
    /// Gets the file naming patterns used in the directory structure.
    /// </summary>
    public IEnumerable<string> FileNamingPatterns { get; init; }

    /// <summary>
    /// Initializes a new instance of the DirectoryStructure class.
    /// </summary>
    /// <param name="baseDirectory">The base directory.</param>
    /// <param name="subdirectoryPatterns">The subdirectory patterns.</param>
    /// <param name="fileNamingPatterns">The file naming patterns.</param>
    public DirectoryStructure(
        string baseDirectory,
        IEnumerable<string> subdirectoryPatterns,
        IEnumerable<string> fileNamingPatterns)
    {
        BaseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
        SubdirectoryPatterns = subdirectoryPatterns ?? throw new ArgumentNullException(nameof(subdirectoryPatterns));
        FileNamingPatterns = fileNamingPatterns ?? throw new ArgumentNullException(nameof(fileNamingPatterns));
    }
}