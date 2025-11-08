using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Represents a writer for generating abstract method declarations in code.
/// </summary>
public sealed class AbstractMethodWriter : IMethodWriter
{
    private readonly DocumentationWriter? _documentation;
    private readonly GenericParameter[]? _genericParameters;
    private readonly string _name;
    private readonly MethodParameter[]? _parameters;
    private readonly string _returnType;
    private readonly IEnumerable<string>? _usings;
    private readonly Visibility _visibility;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractMethodWriter"/> class.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="visibility">The visibility of the method. Defaults to <see cref="Visibility.Public"/>.</param>
    /// <param name="parameters">The parameters of the method. Optional.</param>
    /// <param name="genericParameters">The generic parameters of the method. Optional.</param>
    /// <param name="documentation">The documentation writer for the method. Optional.</param>
    /// <param name="usings">The namespaces to include for the method. Optional.</param>
    public AbstractMethodWriter(string name,
                                string returnType,
                                Visibility visibility = Visibility.Public,
                                IEnumerable<MethodParameter>? parameters = null,
                                IEnumerable<GenericParameter>? genericParameters = null,
                                DocumentationWriter? documentation = null,
                                IEnumerable<string>? usings = null)
    {
        _name = name;
        _returnType = returnType;
        _visibility = visibility;
        _parameters = parameters?.ToArray();
        _genericParameters = genericParameters?.ToArray();
        _documentation = documentation;
        _usings = usings;
    }

    /// <summary>
    /// Gets the namespaces to include for the method.
    /// </summary>
    public IEnumerable<string> Usings => _usings ?? [];

    /// <summary>
    /// Writes the abstract method declaration to the specified <see cref="IndentedTextWriter"/>.
    /// </summary>
    /// <param name="writer">The writer to output the method declaration.</param>
    public void Write(IndentedTextWriter writer)
    {
        _documentation?.Write(writer);

        writer.Write(GetVisibilityString(_visibility));
        writer.Write(" abstract ");

        writer.Write(_returnType);
        writer.Write(' ');

        writer.Write(_name);

        if (_genericParameters is { Length: > 0 })
        {
            writer.Write('<');
            writer.Write(string.Join(", ", _genericParameters.Select(p => p.Name)));
            writer.Write('>');
        }

        writer.Write('(');
        if (_parameters is { Length: > 0 })
        {
            writer.Write(string.Join(", ", _parameters.Select(p => $"{p.Type} {p.Name}")));
        }

        writer.Write(')');

        if (_genericParameters is { Length: > 0 })
        {
            foreach (var genericParam in _genericParameters)
            {
                if (!string.IsNullOrWhiteSpace(genericParam.Constraint))
                {
                    writer.Write(" where ");
                    writer.Write(genericParam.Name);
                    writer.Write(" : ");
                    writer.Write(genericParam.Constraint);
                }
            }
        }

        writer.WriteLine(';');
    }

    /// <summary>
    /// Gets the generic parameters of the method.
    /// </summary>
    public IEnumerable<GenericParameter> GenericParameters => _genericParameters ?? [];

    /// <summary>
    /// Converts the visibility enum to its string representation.
    /// </summary>
    /// <param name="visibility">The visibility enum value.</param>
    /// <returns>The string representation of the visibility.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the visibility value is invalid.</exception>
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
