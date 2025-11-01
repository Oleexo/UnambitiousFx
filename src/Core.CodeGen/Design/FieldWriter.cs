using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class FieldWriter {
    private readonly string               _name;
    private readonly string               _type;
    private readonly Visibility           _visibility;
    private readonly bool                 _isReadonly;
    private readonly DocumentationWriter? _documentation;

    public FieldWriter(string               name,
                       string               type,
                       Visibility           visibility   = Visibility.Private,
                       bool                 isReadonly   = true,
                       DocumentationWriter? documentation = null) {
        _name          = name;
        _type          = type;
        _visibility    = visibility;
        _isReadonly    = isReadonly;
        _documentation = documentation;
    }

    public void Write(IndentedTextWriter writer) {
        // Write documentation
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(' ');

        if (_isReadonly) {
            writer.Write("readonly ");
        }

        writer.Write(_type);
        writer.Write(' ');
        writer.Write(_name);
        writer.WriteLine(';');
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
