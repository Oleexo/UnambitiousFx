namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Extension methods to simplify adding documentation to code writers.
/// </summary>
internal static class DocumentationExtensions {
    /// <summary>
    /// Creates a simple documentation with only a summary.
    /// </summary>
    public static DocumentationWriter WithSummary(this string summary) {
        return DocumentationWriter.Create()
                                  .WithSummary(summary)
                                  .Build();
    }

    /// <summary>
    /// Creates documentation for a method with parameters.
    /// </summary>
    public static DocumentationWriter.Builder ForMethod(string summary) {
        return DocumentationWriter.Create()
                                  .WithSummary(summary);
    }

    /// <summary>
    /// Creates documentation for a class with type parameters.
    /// </summary>
    public static DocumentationWriter.Builder ForClass(string summary) {
        return DocumentationWriter.Create()
                                  .WithSummary(summary);
    }

    /// <summary>
    /// Creates documentation for a property.
    /// </summary>
    public static DocumentationWriter ForProperty(string summary) {
        return DocumentationWriter.Create()
                                  .WithSummary(summary)
                                  .Build();
    }
}
