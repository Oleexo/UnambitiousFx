using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a writer for generating C# property code.
/// </summary>
public sealed class PropertyWriter
{
    private readonly DocumentationWriter? _documentation;
    private readonly string? _getterBody;
    private readonly bool _hasGetter;
    private readonly bool _hasSetter;
    private readonly string _name;
    private readonly PropertyStyle _style;
    private readonly string _type;
    private readonly Visibility _visibility;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyWriter" /> class.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="type">The type of the property.</param>
    /// <param name="visibility">The visibility of the property.</param>
    /// <param name="hasGetter">Whether the property has a getter.</param>
    /// <param name="hasSetter">Whether the property has a setter.</param>
    /// <param name="getterBody">The body of the getter, if any.</param>
    /// <param name="style">The style of the property.</param>
    /// <param name="documentation">The documentation writer for the property.</param>
    public PropertyWriter(string name,
                          string type,
                          Visibility visibility = Visibility.Public,
                          bool hasGetter = true,
                          bool hasSetter = false,
                          string? getterBody = null,
                          PropertyStyle style = PropertyStyle.Expression,
                          DocumentationWriter? documentation = null)
    {
        _name = name;
        _type = type;
        _visibility = visibility;
        _hasGetter = hasGetter;
        _hasSetter = hasSetter;
        _getterBody = getterBody;
        _style = style;
        _documentation = documentation;
    }

    /// <inheritdoc />
    public void Write(IndentedTextWriter writer)
    {
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(" ");

        if (_style == PropertyStyle.Override)
        {
            writer.Write("override ");
        }
        else if (_style == PropertyStyle.Abstract)
        {
            writer.Write("abstract ");
        }

        writer.Write(_type);
        writer.Write(' ');
        writer.Write(_name);

        if (_style is PropertyStyle.Expression or PropertyStyle.Override &&
            _getterBody != null)
        {
            writer.Write(" => ");
            writer.Write(_getterBody);
            writer.WriteLine(';');
        }
        else
        {
            writer.Write(" { ");
            if (_hasGetter)
            {
                writer.Write("get; ");
            }

            if (_hasSetter)
            {
                writer.Write("set; ");
            }

            writer.WriteLine("}");
        }
    }

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
