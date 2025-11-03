using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     A writer class responsible for generating constructor code for a given class.
/// </summary>
public sealed class ConstructorWriter : ICodeWriter {
    private readonly string?              _baseCall;
    private readonly string               _body;
    private readonly string               _className;
    private readonly DocumentationWriter? _documentation;
    private readonly MethodParameter[]    _parameters;
    private readonly IEnumerable<string>? _usings;
    private readonly Visibility           _visibility;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConstructorWriter" /> class.
    /// </summary>
    /// <param name="className">The name of the class for which the constructor is being written.</param>
    /// <param name="body">The body of the constructor.</param>
    /// <param name="visibility">The visibility of the constructor (default is public).</param>
    /// <param name="parameters">The parameters of the constructor.</param>
    /// <param name="documentation">Optional documentation writer for the constructor.</param>
    /// <param name="baseCall">Optional base call to include in the constructor.</param>
    /// <param name="usings">Optional list of using directives required for the constructor.</param>
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

    /// <summary>
    ///     Gets the list of using directives required for the constructor.
    /// </summary>
    public IEnumerable<string> Usings => _usings ?? [];

    /// <summary>
    ///     Writes the constructor code to the specified <see cref="IndentedTextWriter" />.
    /// </summary>
    /// <param name="writer">The writer to which the constructor code will be written.</param>
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

    /// <summary>
    ///     Converts the visibility enum to its string representation.
    /// </summary>
    /// <param name="visibility">The visibility enum value.</param>
    /// <returns>The string representation of the visibility.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the visibility value is not recognized.</exception>
    private static string GetVisibilityString(Visibility visibility) {
        return visibility switch {
            Visibility.Public            => "public",
            Visibility.Internal          => "internal",
            Visibility.Private           => "private",
            Visibility.Protected         => "protected",
            Visibility.ProtectedInternal => "protected internal",
            Visibility.PrivateProtected  => "private protected",
            _                            => throw new ArgumentOutOfRangeException(nameof(visibility))
        };
    }
}
