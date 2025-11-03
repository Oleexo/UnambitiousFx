using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Defines an interface for writing type definitions.
/// </summary>
public interface ITypeDefinitionWriter
{
    /// <summary>
    /// Gets the list of using directives required for the type definition.
    /// </summary>
    IEnumerable<string> Usings { get; }

    /// <summary>
    /// Writes the type definition to the specified <see cref="IndentedTextWriter"/>.
    /// </summary>
    /// <param name="writer">The writer to which the type definition will be written.</param>
    void Write(IndentedTextWriter writer);
}
