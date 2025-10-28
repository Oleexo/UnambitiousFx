using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class PropertyWriter {
    private readonly string               _name;
    private readonly string               _type;
    private readonly Visibility           _visibility;
    private readonly bool                 _hasGetter;
    private readonly bool                 _hasSetter;
    private readonly string?              _getterBody;
    private readonly PropertyStyle        _style;
    private readonly DocumentationWriter? _documentation;

    public PropertyWriter(string               name,
                          string               type,
                          Visibility           visibility    = Visibility.Public,
                          bool                 hasGetter     = true,
                          bool                 hasSetter     = false,
                          string?              getterBody    = null,
                          PropertyStyle        style         = PropertyStyle.Expression,
                          DocumentationWriter? documentation = null) {
        _name          = name;
        _type          = type;
        _visibility    = visibility;
        _hasGetter     = hasGetter;
        _hasSetter     = hasSetter;
        _getterBody    = getterBody;
        _style         = style;
        _documentation = documentation;
    }

    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(" ");

        if (_style == PropertyStyle.Override) {
            writer.Write("override ");
        }
        else if (_style == PropertyStyle.Abstract) {
            writer.Write("abstract ");
        }

        writer.Write(_type);
        writer.Write(' ');
        writer.Write(_name);

        if (_style is PropertyStyle.Expression or PropertyStyle.Override &&
            _getterBody != null) {
            writer.Write(" => ");
            writer.Write(_getterBody);
            writer.WriteLine(';');
        }
        else {
            writer.Write(" { ");
            if (_hasGetter) {
                writer.Write("get; ");
            }

            if (_hasSetter) {
                writer.Write("set; ");
            }

            writer.WriteLine("}");
        }
    }

    private static string GetVisibilityString(Visibility visibility) => visibility switch {
        Visibility.Public            => "public",
        Visibility.Internal          => "internal",
        Visibility.Private           => "private",
        Visibility.Protected         => "protected",
        Visibility.ProtectedInternal => "protected internal",
        Visibility.PrivateProtected  => "private protected",
        _                            => throw new ArgumentOutOfRangeException(nameof(visibility))
    };
}

internal enum PropertyStyle {
    AutoProperty,
    Expression,
    Override,
    Abstract
}
