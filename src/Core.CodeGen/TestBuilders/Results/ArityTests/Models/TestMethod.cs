namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents a generated test method with its structure and content.
/// </summary>
internal sealed class TestMethod {
    /// <summary>
    ///     Initializes a new instance of the TestMethod class.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="body">The method body.</param>
    /// <param name="attributes">The method attributes.</param>
    /// <param name="returnType">The return type.</param>
    /// <param name="visibility">The visibility modifier.</param>
    public TestMethod(string              name,
                      string              body,
                      IEnumerable<string> attributes,
                      string              returnType = "void",
                      string              visibility = "public") {
        Name       = name       ?? throw new ArgumentNullException(nameof(name));
        Body       = body       ?? throw new ArgumentNullException(nameof(body));
        Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        Visibility = visibility ?? throw new ArgumentNullException(nameof(visibility));
    }

    /// <summary>
    ///     Gets the name of the test method.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Gets the body/content of the test method.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Gets the attributes applied to the test method.
    /// </summary>
    public IEnumerable<string> Attributes { get; init; }

    /// <summary>
    ///     Gets the return type of the test method.
    /// </summary>
    public string ReturnType { get; init; }

    /// <summary>
    ///     Gets the visibility modifier of the test method.
    /// </summary>
    public string Visibility { get; init; }
}
