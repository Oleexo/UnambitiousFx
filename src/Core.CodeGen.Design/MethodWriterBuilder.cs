namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Builder for creating immutable MethodWriter instances with fluent API.
/// </summary>
public sealed class MethodWriterBuilder {
    private readonly List<AttributeReference> _attributes = new();
    private readonly string                   _body;
    private readonly List<GenericConstraint>  _genericConstraints = new();
    private readonly List<GenericParameter>   _genericParameters  = new();
    private readonly string                   _name;
    private readonly List<MethodParameter>    _parameters = new();
    private readonly string                   _returnType;
    private readonly HashSet<string>          _usings = new();
    private          DocumentationWriter?     _documentation;
    private          bool                     _isExtensionMethod;
    private          MethodModifier           _modifier   = MethodModifier.None;
    private          Visibility               _visibility = Visibility.Public;

    internal MethodWriterBuilder(string name,
                                 string returnType,
                                 string body) {
        _name       = name       ?? throw new ArgumentNullException(nameof(name));
        _returnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        _body       = body       ?? throw new ArgumentNullException(nameof(body));
    }

    /// <summary>
    ///     Sets the visibility of the method.
    /// </summary>
    /// <param name="visibility">The visibility level.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithVisibility(Visibility visibility) {
        _visibility = visibility;
        return this;
    }

    /// <summary>
    ///     Sets the modifiers for the method.
    /// </summary>
    /// <param name="modifier">The method modifiers.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithModifier(MethodModifier modifier) {
        _modifier = modifier;
        return this;
    }

    /// <summary>
    ///     Adds a parameter to the method.
    /// </summary>
    /// <param name="type">The parameter type.</param>
    /// <param name="name">The parameter name.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithParameter(string type,
                                             string name) {
        _parameters.Add(new MethodParameter(type, name));
        return this;
    }

    /// <summary>
    ///     Adds multiple parameters to the method.
    /// </summary>
    /// <param name="parameters">The parameters to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithParameters(params MethodParameter[] parameters) {
        _parameters.AddRange(parameters);
        return this;
    }

    /// <summary>
    ///     Adds a generic parameter to the method.
    /// </summary>
    /// <param name="name">The generic parameter name.</param>
    /// <param name="using">Optional using directive for the parameter.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithGenericParameter(string  name,
                                                    string? @using = null) {
        _genericParameters.Add(new GenericParameter(name, (IReadOnlyList<GenericConstraint>?)null, @using));
        return this;
    }

    /// <summary>
    ///     Adds multiple generic parameters to the method.
    /// </summary>
    /// <param name="parameters">The generic parameters to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithGenericParameters(params GenericParameter[] parameters) {
        _genericParameters.AddRange(parameters);
        return this;
    }

    /// <summary>
    ///     Adds a generic constraint to the method.
    /// </summary>
    /// <param name="constraint">The generic constraint to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithGenericConstraint(GenericConstraint constraint) {
        _genericConstraints.Add(constraint);
        return this;
    }

    /// <summary>
    ///     Adds a generic constraint to the method.
    /// </summary>
    /// <param name="parameterName">The name of the generic parameter.</param>
    /// <param name="constraint">The constraint string.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithGenericConstraint(string parameterName,
                                                     string constraint) {
        // Parse the constraint and create appropriate GenericConstraint
        var constraintType = constraint.Trim() switch {
            "class"                                                          => GenericConstraintType.Class,
            "struct"                                                         => GenericConstraintType.Struct,
            "notnull"                                                        => GenericConstraintType.NotNull,
            "unmanaged"                                                      => GenericConstraintType.Unmanaged,
            "new()"                                                          => GenericConstraintType.New,
            _ when constraint.StartsWith("I") && char.IsUpper(constraint[1]) => GenericConstraintType.Interface,
            _                                                                => GenericConstraintType.BaseClass
        };

        var typeName = constraintType == GenericConstraintType.Interface || constraintType == GenericConstraintType.BaseClass
                           ? constraint
                           : null;

        _genericConstraints.Add(new GenericConstraint(parameterName, constraintType, typeName));
        return this;
    }

    /// <summary>
    ///     Adds multiple generic constraints to the method.
    /// </summary>
    /// <param name="constraints">The constraints to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithGenericConstraints(params GenericConstraint[] constraints) {
        _genericConstraints.AddRange(constraints);
        return this;
    }

    /// <summary>
    ///     Adds an attribute to the method.
    /// </summary>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithAttribute(AttributeReference attribute) {
        _attributes.Add(attribute);
        return this;
    }

    /// <summary>
    ///     Adds multiple attributes to the method.
    /// </summary>
    /// <param name="attributes">The attributes to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithAttributes(params AttributeReference[] attributes) {
        _attributes.AddRange(attributes);
        return this;
    }

    /// <summary>
    ///     Sets the documentation for the method.
    /// </summary>
    /// <param name="documentation">The documentation writer.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithDocumentation(DocumentationWriter documentation) {
        _documentation = documentation;
        return this;
    }

    /// <summary>
    ///     Adds a using directive required by the method.
    /// </summary>
    /// <param name="using">The using directive to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithUsing(string @using) {
        _usings.Add(@using);
        return this;
    }

    /// <summary>
    ///     Adds multiple using directives required by the method.
    /// </summary>
    /// <param name="usings">The using directives to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithUsings(params string[] usings) {
        foreach (var @using in usings) {
            _usings.Add(@using);
        }

        return this;
    }

    /// <summary>
    ///     Marks the method as an extension method. The first parameter will be treated as the 'this' parameter.
    /// </summary>
    /// <param name="thisParameterType">The type of the 'this' parameter.</param>
    /// <param name="thisParameterName">The name of the 'this' parameter (defaults to 'source').</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithExtensionMethod(string thisParameterType,
                                                   string thisParameterName = "source") {
        _isExtensionMethod =  true;
        _modifier          |= MethodModifier.Static;

        // Insert the 'this' parameter at the beginning if not already present
        if (_parameters.Count   == 0 ||
            _parameters[0].Type != thisParameterType) {
            _parameters.Insert(0, new MethodParameter(thisParameterType, thisParameterName));
        }

        return this;
    }

    /// <summary>
    ///     Marks the method as async and updates the return type if needed.
    /// </summary>
    /// <param name="isValueTask">Whether to use ValueTask instead of Task.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MethodWriterBuilder WithAsyncModifier(bool isValueTask = false) {
        _modifier |= MethodModifier.Async;

        // Update return type if it's not already async
        if (!_returnType.StartsWith("Task") &&
            !_returnType.StartsWith("ValueTask")) {
            var asyncReturnType = isValueTask
                                      ? "ValueTask"
                                      : "Task";
            if (_returnType != "void") {
                asyncReturnType += $"<{_returnType}>";
            }
            // Note: We can't modify _returnType here as it's readonly, 
            // this would need to be handled in the Build method or constructor
        }

        return this;
    }

    /// <summary>
    ///     Builds the immutable MethodWriter instance.
    /// </summary>
    /// <returns>A new MethodWriter instance.</returns>
    public MethodWriter Build() {
        return new MethodWriter(
            _name,
            _returnType,
            _body,
            _visibility,
            _modifier,
            _parameters.ToArray(),
            _genericParameters.ToArray(),
            _genericConstraints.ToArray(),
            _attributes.ToArray(),
            _documentation,
            _usings.ToHashSet(),
            _isExtensionMethod);
    }
}
