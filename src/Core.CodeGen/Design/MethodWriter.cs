using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class MethodWriter : IMethodWriter {
    private readonly string                _name;
    private readonly string                _returnType;
    private readonly Visibility            _visibility;
    private readonly MethodModifier        _modifier;
    private readonly MethodParameter[]?    _parameters;
    private readonly GenericParameter[]?   _genericParameters;
    private readonly AttributeReference[]? _attributes;
    private readonly string                _body;
    private readonly DocumentationWriter?  _documentation;
    private readonly IEnumerable<string>?  _usings;

    public MethodWriter(string                           name,
                        string                           returnType,
                        string                           body,
                        Visibility                       visibility        = Visibility.Public,
                        MethodModifier                   modifier          = MethodModifier.None,
                        IEnumerable<MethodParameter>?    parameters        = null,
                        IEnumerable<GenericParameter>?   genericParameters = null,
                        DocumentationWriter?             documentation     = null,
                        IEnumerable<AttributeReference>? attributes        = null,
                        IEnumerable<string>?             usings            = null) {
        _name              = name;
        _returnType        = returnType;
        _body              = body;
        _visibility        = visibility;
        _modifier          = modifier;
        _parameters        = parameters?.ToArray();
        _genericParameters = genericParameters?.ToArray();
        _documentation     = documentation;
        _usings            = usings;
        _attributes        = attributes?.ToArray();
    }

    public IEnumerable<string> Usings => GetUsings();

    private HashSet<string> GetUsings() {
        var usings = new HashSet<string>();

        if (_attributes is { Length: > 0 }) {
            foreach (var attribute in _attributes) {
                if (!string.IsNullOrWhiteSpace(attribute.Using)) {
                    usings.Add(attribute.Using);
                }
            }
        }

        if (_usings is not null) {
            foreach (var @using in _usings) {
                usings.Add(@using);
            }
        }

        return usings;
    }

    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        if (_attributes is { Length: > 0 }) {
            foreach (var attribute in _attributes) {
                writer.Write($"[{attribute.Name}");
                if (!string.IsNullOrWhiteSpace(attribute.Arguments)) {
                    writer.Write($"({attribute.Arguments})");
                }

                writer.WriteLine("]");
            }
        }

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(' ');

        if (_modifier.HasFlag(MethodModifier.Static)) {
            writer.Write("static ");
        }

        if (_modifier.HasFlag(MethodModifier.Async)) {
            writer.Write("async ");
        }

        if (_modifier.HasFlag(MethodModifier.Virtual)) {
            writer.Write("virtual ");
        }

        if (_modifier.HasFlag(MethodModifier.Override)) {
            writer.Write("override ");
        }

        if (_modifier.HasFlag(MethodModifier.Sealed)) {
            writer.Write("sealed ");
        }

        writer.Write(_returnType);
        writer.Write(' ');

        writer.Write(_name);

        if (_genericParameters is { Length: > 0 }) {
            writer.Write('<');
            writer.Write(string.Join(", ", _genericParameters.Select(p => p.Name)));
            writer.Write('>');
        }

        writer.Write('(');
        if (_parameters is { Length: > 0 }) {
            writer.Write(string.Join(", ", _parameters.Select(p => $"{p.Type} {p.Name}")));
        }

        writer.Write(')');

        if (_genericParameters is { Length: > 0 }) {
            foreach (var genericParam in _genericParameters) {
                if (!string.IsNullOrWhiteSpace(genericParam.Constraint)) {
                    writer.Write(" where ");
                    writer.Write(genericParam.Name);
                    writer.Write(" : ");
                    writer.Write(genericParam.Constraint);
                }
            }
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

    public IEnumerable<GenericParameter> GenericParameters => _genericParameters ?? [];

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
