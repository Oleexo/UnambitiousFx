using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a writer for generating code elements.
/// </summary>
public interface ICodeWriter
{
    /// <summary>
    ///     Gets the collection of using statements required for the generated method.
    /// </summary>
    /// <value>
    ///     An enumerable collection of namespace strings that should be included as using directives.
    /// </value>
    IEnumerable<string> Usings { get; }

    /// <summary>
    ///     Writes the method implementation to the specified text writer.
    /// </summary>
    /// <param name="writer">The indented text writer to output the method code to.</param>
    void Write(IndentedTextWriter writer);
}
