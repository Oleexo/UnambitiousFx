using UnambitiousFx.Core.CodeGen.Common;

namespace UnambitiousFx.Core.CodeGen.Configuration;

/// <summary>
/// Base class for code generators implementing the Template Method pattern.
/// Provides common structure and validation for all generators.
/// </summary>
internal abstract class BaseCodeGenerator : ICodeGenerator
{
    protected readonly GenerationConfig Config;

    protected BaseCodeGenerator(GenerationConfig config)
    {
        Config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Generates code for the specified arity range.
    /// Implements template method pattern with validation and directory setup.
    /// </summary>
    public void Generate(ushort numberOfArity, string outputPath)
    {
        ValidateInputs(numberOfArity, outputPath);
        var targetPath = PrepareOutputDirectory(outputPath);
        GenerateForArityRange(numberOfArity, targetPath);
    }

    /// <summary>
    /// Validates generation inputs.
    /// </summary>
    protected virtual void ValidateInputs(ushort numberOfArity, string outputPath)
    {
        if (numberOfArity < Config.StartArity)
            throw new ArgumentOutOfRangeException(nameof(numberOfArity),
                $"Arity must be >= {Config.StartArity}.");

        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Output path cannot be null or whitespace.", nameof(outputPath));
    }

    /// <summary>
    /// Prepares the output directory structure.
    /// </summary>
    protected virtual string PrepareOutputDirectory(string outputPath)
    {
        if (string.IsNullOrEmpty(Config.DirectoryName))
            return outputPath;

        return FileSystemHelper.CreateSubdirectory(outputPath, Config.DirectoryName);
    }

    /// <summary>
    /// Generates code for all arities in the range.
    /// Derived classes must implement this method.
    /// </summary>
    protected abstract void GenerateForArityRange(ushort numberOfArity, string outputPath);
}
