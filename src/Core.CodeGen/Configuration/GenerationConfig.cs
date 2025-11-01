namespace UnambitiousFx.Core.CodeGen.Configuration;

/// <summary>
/// Configuration for code generation operations.
/// </summary>
internal sealed class GenerationConfig {
    private readonly string _baseNamespace;

    /// <summary>
    /// Gets the base namespace for generated code.
    /// </summary>
    public string BaseNamespace => !IsTest ? _baseNamespace : $"{_baseNamespace}.Tests";

    /// <summary>
    /// Gets the starting arity for generation.
    /// </summary>
    public int StartArity { get; }

    /// <summary>
    /// Gets the output directory name.
    /// </summary>
    public string SubNamespace { get; }

    /// <summary>
    /// Gets the class name prefix.
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// Gets the file organization mode.
    /// </summary>
    public FileOrganizationMode FileOrganization { get; }

    public bool IsTest { get; }

    public GenerationConfig(string               baseNamespace,
                            int                  startArity       = 1,
                            string               subNamespace     = "",
                            string               className        = "",
                            FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles,
                            bool                 isTest           = false) {
        if (string.IsNullOrWhiteSpace(baseNamespace))
            throw new ArgumentException("Base namespace cannot be null or whitespace.", nameof(baseNamespace));

        _baseNamespace     = baseNamespace;
        StartArity        = startArity;
        SubNamespace = subNamespace;
        ClassName         = className;
        FileOrganization  = fileOrganization;
        IsTest            = isTest;
    }
}
