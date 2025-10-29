using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class ConstructorWriter {
    private readonly string               _className;
    private readonly Visibility           _visibility;
    private readonly MethodParameter[]    _parameters;
    private readonly string               _body;
    private readonly DocumentationWriter? _documentation;
    private readonly string?              _baseCall;
    private readonly IEnumerable<string>? _usings;

    public ConstructorWriter(string                        className,
                             string                        body,
                             Visibility                    visibility    = Visibility.Public,
                             IEnumerable<MethodParameter>? parameters    = null,
                             DocumentationWriter?          documentation = null,
                             string?                       baseCall      = null,
                             IEnumerable<string>?          usings        = null) {
        _className     = className;
        _body          = body;
        _visibility    = visibility;
        _parameters    = parameters?.ToArray() ?? [];
        _documentation = documentation;
        _baseCall      = baseCall;
        _usings        = usings;
    }
    
    public IEnumerable<string> Usings => _usings ?? [];

    public void Write(IndentedTextWriter writer) {
        // Write documentation
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(' ');
        writer.Write(_className);
        writer.Write('(');

        if (_parameters.Length > 0) {
            writer.Write(string.Join(", ", _parameters.Select(p => $"{p.Type} {p.Name}")));
        }

        writer.Write(")");

        if (!string.IsNullOrEmpty(_baseCall)) {
            writer.Write(" : ");
            writer.Write(_baseCall);
        }

        writer.WriteLine(" {");
        writer.Indent++;

        var bodyLines = _body.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in bodyLines) {
            writer.WriteLine(line);
        }

        writer.Indent--;
        writer.WriteLine("}");
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
