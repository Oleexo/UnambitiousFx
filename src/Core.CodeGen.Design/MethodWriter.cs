using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Immutable writer for generating C# method declarations and implementations.
/// </summary>
public sealed class MethodWriter : IMethodWriter {
    private readonly IReadOnlyList<AttributeReference> _attributes;         // The attributes applied to the method.
    private readonly string                            _body;               // The body of the method.
    private readonly DocumentationWriter?              _documentation;      // The documentation writer for the method.
    private readonly IReadOnlyList<GenericConstraint>  _genericConstraints; // The generic constraints of the method.
    private readonly IReadOnlyList<GenericParameter>   _genericParameters;  // The generic parameters of the method.
    private readonly bool                              _isExtensionMethod;  // Indicates whether the method is an extension method.
    private readonly MethodModifier                    _modifier;           // The modifier of the method (e.g., static, async).
    private readonly string                            _name;               // The name of the method.
    private readonly IReadOnlyList<MethodParameter>    _parameters;         // The parameters of the method.
    private readonly string                            _returnType;         // The return type of the method.
    private readonly IReadOnlySet<string>              _usings;             // The set of using directives required by the method.
    private readonly Visibility                        _visibility;         // The visibility of the method (e.g., public, private).

    /// <summary>
    ///     Initializes a new instance of the <see cref="MethodWriter" /> class.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="body">The body of the method.</param>
    /// <param name="visibility">The visibility of the method. Defaults to public.</param>
    /// <param name="modifier">The modifier of the method. Defaults to none.</param>
    /// <param name="parameters">The parameters of the method.</param>
    /// <param name="genericParameters">The generic parameters of the method.</param>
    /// <param name="documentation">The documentation writer for the method.</param>
    /// <param name="attributes">The attributes applied to the method.</param>
    /// <param name="usings">The set of using directives required by the method.</param>
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
        _name               = name       ?? throw new ArgumentNullException(nameof(name));
        _returnType         = returnType ?? throw new ArgumentNullException(nameof(returnType));
        _body               = body       ?? throw new ArgumentNullException(nameof(body));
        _visibility         = visibility;
        _modifier           = modifier;
        _parameters         = parameters?.ToArray()        ?? [];
        _genericParameters  = genericParameters?.ToArray() ?? [];
        _genericConstraints = [];
        _attributes         = attributes?.ToArray() ?? [];
        _documentation      = documentation;
        _usings             = usings?.ToHashSet() ?? [];
        _isExtensionMethod  = false;
    }

    /// <summary>
    ///     Internal constructor for builder.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="body">The body of the method.</param>
    /// <param name="visibility">The visibility of the method.</param>
    /// <param name="modifier">The modifier of the method.</param>
    /// <param name="parameters">The parameters of the method.</param>
    /// <param name="genericParameters">The generic parameters of the method.</param>
    /// <param name="genericConstraints">The generic constraints of the method.</param>
    /// <param name="attributes">The attributes applied to the method.</param>
    /// <param name="documentation">The documentation writer for the method.</param>
    /// <param name="usings">The set of using directives required by the method.</param>
    /// <param name="isExtensionMethod">Indicates whether the method is an extension method.</param>
    internal MethodWriter(string                            name,
                          string                            returnType,
                          string                            body,
                          Visibility                        visibility,
                          MethodModifier                    modifier,
                          IReadOnlyList<MethodParameter>    parameters,
                          IReadOnlyList<GenericParameter>   genericParameters,
                          IReadOnlyList<GenericConstraint>  genericConstraints,
                          IReadOnlyList<AttributeReference> attributes,
                          DocumentationWriter?              documentation,
                          IReadOnlySet<string>              usings,
                          bool                              isExtensionMethod) {
        _name               = name       ?? throw new ArgumentNullException(nameof(name));
        _returnType         = returnType ?? throw new ArgumentNullException(nameof(returnType));
        _body               = body       ?? throw new ArgumentNullException(nameof(body));
        _visibility         = visibility;
        _modifier           = modifier;
        _parameters         = parameters         ?? throw new ArgumentNullException(nameof(parameters));
        _genericParameters  = genericParameters  ?? throw new ArgumentNullException(nameof(genericParameters));
        _genericConstraints = genericConstraints ?? throw new ArgumentNullException(nameof(genericConstraints));
        _attributes         = attributes         ?? throw new ArgumentNullException(nameof(attributes));
        _documentation      = documentation;
        _usings             = usings ?? throw new ArgumentNullException(nameof(usings));
        _isExtensionMethod  = isExtensionMethod;
    }

    /// <inheritdoc />
    public IEnumerable<string> Usings => GetUsings();

    /// <inheritdoc />
    /// <summary>
    ///     Writes the method declaration and implementation to the specified writer.
    /// </summary>
    /// <param name="writer">The writer to which the method will be written.</param>
    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        WriteAttributes(writer);
        WriteMethodSignature(writer);
        WriteGenericConstraints(writer);
        WriteMethodBody(writer);
    }

    /// <summary>
    ///     Gets the generic parameters of the method.
    /// </summary>
    public IEnumerable<GenericParameter> GenericParameters => _genericParameters;

    /// <summary>
    ///     Retrieves the set of using directives required by the method.
    /// </summary>
    /// <returns>A set of using directives.</returns>
    private HashSet<string> GetUsings() {
        var usings = new HashSet<string>(_usings);

        foreach (var attribute in _attributes) {
            if (!string.IsNullOrWhiteSpace(attribute.Using)) {
                usings.Add(attribute.Using);
            }
        }

        foreach (var genericParameter in _genericParameters) {
            if (!string.IsNullOrWhiteSpace(genericParameter.Using)) {
                usings.Add(genericParameter.Using);
            }
        }

        return usings;
    }

    /// <summary>
    ///     Writes the attributes applied to the method.
    /// </summary>
    /// <param name="writer">The writer to which the attributes will be written.</param>
    private void WriteAttributes(IndentedTextWriter writer) {
        foreach (var attribute in _attributes) {
            writer.Write($"[{attribute.Name}");
            if (!string.IsNullOrWhiteSpace(attribute.Arguments)) {
                writer.Write($"({attribute.Arguments})");
            }

            writer.WriteLine("]");
        }
    }

    /// <summary>
    ///     Writes the method signature.
    /// </summary>
    /// <param name="writer">The writer to which the method signature will be written.</param>
    private void WriteMethodSignature(IndentedTextWriter writer) {
        writer.Write(GetVisibilityString(_visibility));
        writer.Write(' ');

        WriteModifiers(writer);

        writer.Write(_returnType);
        writer.Write(' ');

        writer.Write(_name);

        WriteGenericParameters(writer);
        WriteParameters(writer);
    }

    /// <summary>
    ///     Writes the modifiers of the method.
    /// </summary>
    /// <param name="writer">The writer to which the modifiers will be written.</param>
    private void WriteModifiers(IndentedTextWriter writer) {
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
    }

    /// <summary>
    ///     Writes the generic parameters of the method.
    /// </summary>
    /// <param name="writer">The writer to which the generic parameters will be written.</param>
    private void WriteGenericParameters(IndentedTextWriter writer) {
        if (_genericParameters.Count > 0) {
            writer.Write('<');
            writer.Write(string.Join(", ", _genericParameters.Select(p => p.Name)));
            writer.Write('>');
        }
    }

    /// <summary>
    ///     Writes the parameters of the method.
    /// </summary>
    /// <param name="writer">The writer to which the parameters will be written.</param>
    private void WriteParameters(IndentedTextWriter writer) {
        writer.Write('(');

        if (_parameters.Count > 0) {
            var parameterStrings = new List<string>();

            for (var i = 0; i < _parameters.Count; i++) {
                var param = _parameters[i];
                var paramString = _isExtensionMethod && i == 0
                                      ? $"this {param.Type} {param.Name}"
                                      : $"{param.Type} {param.Name}";
                parameterStrings.Add(paramString);
            }

            writer.Write(string.Join(", ", parameterStrings));
        }

        writer.Write(')');
    }

    /// <summary>
    ///     Writes the generic constraints of the method.
    /// </summary>
    /// <param name="writer">The writer to which the generic constraints will be written.</param>
    private void WriteGenericConstraints(IndentedTextWriter writer) {
        // Group constraints by parameter name
        var constraintsByParameter = _genericConstraints
                                    .GroupBy(c => c.ParameterName)
                                    .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var genericParam in _genericParameters) {
            var constraints = new List<string>();

            // Add constraints from the new constraint system
            if (constraintsByParameter.TryGetValue(genericParam.Name, out var paramConstraints)) {
                constraints.AddRange(paramConstraints.Select(c => c.ToConstraintString()));
            }

            // Add legacy constraints for backward compatibility
            if (!string.IsNullOrWhiteSpace(genericParam.Constraint)) {
                constraints.Add(genericParam.Constraint);
            }

            if (constraints.Count > 0) {
                writer.Write(" where ");
                writer.Write(genericParam.Name);
                writer.Write(" : ");
                writer.Write(string.Join(", ", constraints));
            }
        }
    }

    /// <summary>
    ///     Writes the body of the method.
    /// </summary>
    /// <param name="writer">The writer to which the method body will be written.</param>
    private void WriteMethodBody(IndentedTextWriter writer) {
        writer.WriteLine(" {");
        writer.Indent++;

        var bodyLines = _body.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
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

    /// <summary>
    ///     Creates a new MethodWriterBuilder for fluent construction of MethodWriter instances.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="body">The body of the method.</param>
    /// <returns>A new MethodWriterBuilder instance.</returns>
    public static MethodWriterBuilder Create(string name,
                                             string returnType,
                                             string body) {
        return new MethodWriterBuilder(name, returnType, body);
    }
}
