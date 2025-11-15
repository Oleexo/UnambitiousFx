using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a writer for generating field definitions in a class.
/// </summary>
public sealed class FieldWriter : ICodeWriter
{
    private readonly DocumentationWriter? _documentation;

    private readonly bool _isReadonly;

    // Private fields for storing field metadata
    private readonly string _name;
    private readonly string _type;
    private readonly Visibility _visibility;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FieldWriter" /> class.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <param name="type">The data type of the field.</param>
    /// <param name="visibility">The visibility of the field (default is private).</param>
    /// <param name="isReadonly">Indicates whether the field is readonly (default is true).</param>
    /// <param name="documentation">Optional documentation writer for the field.</param>
    /// <param name="usings"></param>
    public FieldWriter(string name,
                       string type,
                       Visibility visibility = Visibility.Private,
                       bool isReadonly = true,
                       DocumentationWriter? documentation = null,
                       IEnumerable<string>? usings = null)
    {
        Usings = usings ?? [];
        _name = name;
        _type = type;
        _visibility = visibility;
        _isReadonly = isReadonly;
        _documentation = documentation;
    }

    /// <inheritdoc />
    public IEnumerable<string> Usings { get; }

    /// <inheritdoc />
    public void Write(IndentedTextWriter writer)
    {
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(' ');

        if (_isReadonly)
        {
            writer.Write("readonly ");
        }

        writer.Write(_type);
        writer.Write(' ');
        writer.Write(_name);
        writer.WriteLine(';');
    }

    /// <summary>
    ///     Converts the visibility enum to its string representation.
    /// </summary>
    /// <param name="visibility">The visibility enum value.</param>
    /// <returns>The string representation of the visibility.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the visibility value is out of range.</exception>
    private static string GetVisibilityString(Visibility visibility)
    {
        return visibility switch
        {
            Visibility.Public => "public",
            Visibility.Internal => "internal",
            Visibility.Private => "private",
            Visibility.Protected => "protected",
            Visibility.ProtectedInternal => "protected internal",
            Visibility.PrivateProtected => "private protected",
            _ => throw new ArgumentOutOfRangeException(nameof(visibility))
        };
    }
}
