namespace UnambitiousFx.Core.CodeGen.Configuration;

/// <summary>
/// Configuration for code generation operations.
/// </summary>
internal sealed class GenerationConfig
{
    /// <summary>
    /// Gets the base namespace for generated code.
    /// </summary>
    public string BaseNamespace { get; }

    /// <summary>
    /// Gets the starting arity for generation.
    /// </summary>
    public int StartArity { get; }

    /// <summary>
    /// Gets the output directory name.
    /// </summary>
    public string DirectoryName { get; }

    /// <summary>
    /// Gets the class name prefix.
    /// </summary>
    public string ClassName { get; }

    public GenerationConfig(
        string baseNamespace,
        int startArity = 1,
        string directoryName = "",
        string className = "")
    {
        if (string.IsNullOrWhiteSpace(baseNamespace))
            throw new ArgumentException("Base namespace cannot be null or whitespace.", nameof(baseNamespace));

        BaseNamespace = baseNamespace;
        StartArity = startArity;
        DirectoryName = directoryName;
        ClassName = className;
    }
}
