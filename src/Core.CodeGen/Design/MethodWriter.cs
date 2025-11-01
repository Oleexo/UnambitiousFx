using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Immutable writer for generating C# method declarations and implementations.
/// </summary>
internal sealed class MethodWriter : IMethodWriter {
    private readonly string                           _name;
    private readonly string                           _returnType;
    private readonly string                           _body;
    private readonly Visibility                       _visibility;
    private readonly MethodModifier                   _modifier;
    private readonly IReadOnlyList<MethodParameter>   _parameters;
    private readonly IReadOnlyList<GenericParameter>  _genericParameters;
    private readonly IReadOnlyList<GenericConstraint> _genericConstraints;
    private readonly IReadOnlyList<AttributeReference> _attributes;
    private readonly DocumentationWriter?             _documentation;
    private readonly IReadOnlySet<string>             _usings;
    private readonly bool                             _isExtensionMethod;

    // Backward-compatible constructor for existing code
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
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _returnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        _body = body ?? throw new ArgumentNullException(nameof(body));
        _visibility = visibility;
        _modifier = modifier;
        _parameters = parameters?.ToArray() ?? Array.Empty<MethodParameter>();
        _genericParameters = genericParameters?.ToArray() ?? Array.Empty<GenericParameter>();
        _genericConstraints = Array.Empty<GenericConstraint>();
        _attributes = attributes?.ToArray() ?? Array.Empty<AttributeReference>();
        _documentation = documentation;
        _usings = usings?.ToHashSet() ?? new HashSet<string>();
        _isExtensionMethod = false;
    }

    // Internal constructor for builder
    internal MethodWriter(
        string name,
        string returnType,
        string body,
        Visibility visibility,
        MethodModifier modifier,
        IReadOnlyList<MethodParameter> parameters,
        IReadOnlyList<GenericParameter> genericParameters,
        IReadOnlyList<GenericConstraint> genericConstraints,
        IReadOnlyList<AttributeReference> attributes,
        DocumentationWriter? documentation,
        IReadOnlySet<string> usings,
        bool isExtensionMethod) {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _returnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        _body = body ?? throw new ArgumentNullException(nameof(body));
        _visibility = visibility;
        _modifier = modifier;
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _genericParameters = genericParameters ?? throw new ArgumentNullException(nameof(genericParameters));
        _genericConstraints = genericConstraints ?? throw new ArgumentNullException(nameof(genericConstraints));
        _attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        _documentation = documentation;
        _usings = usings ?? throw new ArgumentNullException(nameof(usings));
        _isExtensionMethod = isExtensionMethod;
    }

    public IEnumerable<string> Usings => GetUsings();

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

    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        WriteAttributes(writer);
        WriteMethodSignature(writer);
        WriteGenericConstraints(writer);
        WriteMethodBody(writer);
    }

    private void WriteAttributes(IndentedTextWriter writer) {
        foreach (var attribute in _attributes) {
            writer.Write($"[{attribute.Name}");
            if (!string.IsNullOrWhiteSpace(attribute.Arguments)) {
                writer.Write($"({attribute.Arguments})");
            }
            writer.WriteLine("]");
        }
    }

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

    private void WriteGenericParameters(IndentedTextWriter writer) {
        if (_genericParameters.Count > 0) {
            writer.Write('<');
            writer.Write(string.Join(", ", _genericParameters.Select(p => p.Name)));
            writer.Write('>');
        }
    }

    private void WriteParameters(IndentedTextWriter writer) {
        writer.Write('(');
        
        if (_parameters.Count > 0) {
            var parameterStrings = new List<string>();
            
            for (int i = 0; i < _parameters.Count; i++) {
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

    private void WriteMethodBody(IndentedTextWriter writer) {
        writer.WriteLine(" {");
        writer.Indent++;

        var bodyLines = _body.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in bodyLines) {
            writer.WriteLine(line);
        }

        writer.Indent--;
        writer.WriteLine("}");
    }

    public IEnumerable<GenericParameter> GenericParameters => _genericParameters;

    private static string GetVisibilityString(Visibility visibility) => visibility switch {
        Visibility.Public            => "public",
        Visibility.Internal          => "internal",
        Visibility.Private           => "private",
        Visibility.Protected         => "protected",
        Visibility.ProtectedInternal => "protected internal",
        Visibility.PrivateProtected  => "private protected",
        _                            => throw new ArgumentOutOfRangeException(nameof(visibility))
    };

    /// <summary>
    /// Creates a new MethodWriterBuilder for fluent construction of MethodWriter instances.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="body">The body of the method.</param>
    /// <returns>A new MethodWriterBuilder instance.</returns>
    public static MethodWriterBuilder Create(string name, string returnType, string body) =>
        new(name, returnType, body);
}
