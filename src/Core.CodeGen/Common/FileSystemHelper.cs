using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Common;

/// <summary>
/// Provides utility methods for file system operations in code generation.
/// </summary>
internal static class FileSystemHelper
{
    /// <summary>
    /// Ensures a directory exists, creating it if necessary.
    /// </summary>
    /// <param name="directoryPath">The directory path to ensure.</param>
    public static void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    /// <summary>
    /// Writes a FileWriter to disk.
    /// </summary>
    /// <param name="fileWriter">The FileWriter containing the code to write.</param>
    /// <param name="filePath">The absolute path where the file should be written.</param>
    public static void WriteFile(FileWriter fileWriter, string filePath)
    {
        using var stringWriter = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);
        File.WriteAllText(filePath, stringWriter.ToString());
    }

    /// <summary>
    /// Writes a RegionFileWriter to disk.
    /// </summary>
    /// <param name="regionWriter">The RegionFileWriter containing the code to write.</param>
    /// <param name="filePath">The absolute path where the file should be written.</param>
    public static void WriteRegionFile(RegionFileWriter regionWriter, string filePath)
    {
        using var stringWriter = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        regionWriter.Write(indentedWriter);
        File.WriteAllText(filePath, stringWriter.ToString());
    }

    /// <summary>
    /// Creates a subdirectory within the given output path and ensures it exists.
    /// </summary>
    /// <param name="outputPath">The base output path.</param>
    /// <param name="subdirectory">The subdirectory name.</param>
    /// <returns>The full path to the subdirectory.</returns>
    public static string CreateSubdirectory(string outputPath, string subdirectory)
    {
        var fullPath = Path.Combine(outputPath, subdirectory);
        EnsureDirectoryExists(fullPath);
        return fullPath;
    }
}
