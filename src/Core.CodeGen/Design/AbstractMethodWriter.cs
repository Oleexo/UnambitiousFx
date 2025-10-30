using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class AbstractMethodWriter : IMethodWriter {
    private readonly string               _name;
    private readonly string               _returnType;
    private readonly Visibility           _visibility;
    private readonly MethodParameter[]?   _parameters;
    private readonly GenericParameter[]?  _genericParameters;
    private readonly DocumentationWriter? _documentation;
    private readonly IEnumerable<string>? _usings;

    public AbstractMethodWriter(string                         name,
                                string                         returnType,
                                Visibility                     visibility        = Visibility.Public,
                                IEnumerable<MethodParameter>?  parameters        = null,
                                IEnumerable<GenericParameter>? genericParameters = null,
                                DocumentationWriter?           documentation     = null,
                                IEnumerable<string>?           usings            = null) {
        _name              = name;
        _returnType        = returnType;
        _visibility        = visibility;
        _parameters        = parameters?.ToArray();
        _genericParameters = genericParameters?.ToArray();
        _documentation     = documentation;
        _usings            = usings;
    }

    public IEnumerable<string> Usings => _usings ?? [];

    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(" abstract ");

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

        writer.WriteLine(';');
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
