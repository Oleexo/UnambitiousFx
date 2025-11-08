using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a writer for generating C# class definitions.
/// </summary>
public sealed class ClassWriter : IConcreteTypeWriter {
    private readonly List<AttributeReference>      _attributes;
    private readonly TypeDefinitionReference?      _baseClass;
    private readonly ClassModifier                 _classModifiers;
    private readonly List<ConstructorWriter>       _constructors;
    private readonly DocumentationWriter?          _documentation;
    private readonly List<FieldWriter>             _fields;
    private readonly GenericParameter[]            _genericParameters;
    private readonly List<TypeDefinitionReference> _interfaces;
    private readonly List<IMethodWriter>           _methods;
    private readonly List<PropertyWriter>          _properties;

    private readonly List<RegionGroup> _regions;
    private readonly HashSet<string>   _usings;

    // Private fields for storing class metadata and components
    private readonly Visibility _visibility;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClassWriter" /> class.
    /// </summary>
    /// <param name="name">The name of the class.</param>
    /// <param name="visibility">The visibility of the class (default is internal).</param>
    /// <param name="classModifiers">Modifiers for the class (e.g., static, abstract).</param>
    /// <param name="genericParameters">Generic parameters for the class.</param>
    /// <param name="baseClass">The base class, if any.</param>
    /// <param name="interfaces">Interfaces implemented by the class.</param>
    /// <param name="documentation">Documentation writer for the class.</param>
    /// <param name="attributes">Attributes applied to the class.</param>
    /// <remarks>
    ///     Both <paramref name="baseClass" /> and <paramref name="documentation" /> can be null.
    /// </remarks>
    public ClassWriter(string                                name,
                       Visibility                            visibility        = Visibility.Internal,
                       ClassModifier                         classModifiers    = ClassModifier.None,
                       IEnumerable<GenericParameter>?        genericParameters = null,
                       TypeDefinitionReference?              baseClass         = null,
                       IEnumerable<TypeDefinitionReference>? interfaces        = null,
                       DocumentationWriter?                  documentation     = null,
                       IEnumerable<AttributeReference>?      attributes        = null) {
        Name               = name;
        _visibility        = visibility;
        _classModifiers    = classModifiers;
        _genericParameters = genericParameters?.ToArray() ?? [];
        _baseClass         = baseClass;
        _interfaces        = interfaces?.ToList() ?? [];
        _attributes        = attributes?.ToList() ?? [];
        _documentation     = documentation;
        _fields            = [];
        _constructors      = [];
        _properties        = [];
        _methods           = [];
        _regions           = [];
        _usings            = [];
    }

    /// <summary>
    ///     Gets or sets the region name for the class.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    ///     Gets or sets the namespace for the class.
    /// </summary>
    public string? Namespace { get; set; }

    /// <summary>
    ///     Gets or sets the name of the containing class, if any.
    /// </summary>
    public string? UnderClass { get; set; }

    /// <summary>
    ///     Gets the name of the class.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets all methods in the class, including those in regions.
    /// </summary>
    public IEnumerable<IMethodWriter> Methods => _methods.Concat(_regions.SelectMany(r => r.Methods));

    /// <summary>
    ///     Gets the using directives required by the class.
    /// </summary>
    public IEnumerable<string> Usings => GetUsings();

    /// <summary>
    ///     Writes the class definition to the specified text writer.
    /// </summary>
    /// <param name="writer">The text writer to write to.</param>
    public void Write(IndentedTextWriter writer) {
        _documentation?.Write(writer);

        foreach (var attribute in _attributes) {
            writer.Write($"[{attribute.Name}");
            if (!string.IsNullOrWhiteSpace(attribute.Arguments)) {
                writer.Write($"({attribute.Arguments})");
            }

            writer.WriteLine("]");
        }

        var genericPart = _genericParameters is { Length: > 0 }
                              ? $"<{string.Join(", ", _genericParameters.Select(p => p.Name))}>"
                              : string.Empty;

        var constraints = _genericParameters
                         .Where(p => !string.IsNullOrEmpty(p.Constraint))
                         .Select(p => $"where {p.Name} : {p.Constraint}")
                         .ToArray();

        // Build modifiers string from flags (avoid enum ToString() commas for combined flags)
        var modifierParts = new List<string>();
        if ((_classModifiers & ClassModifier.Static) != 0) {
            modifierParts.Add("static");
        }

        if ((_classModifiers & ClassModifier.Abstract) != 0) {
            modifierParts.Add("abstract");
        }

        if ((_classModifiers & ClassModifier.Sealed) != 0) {
            modifierParts.Add("sealed");
        }

        if ((_classModifiers & ClassModifier.Partial) != 0) {
            modifierParts.Add("partial");
        }

        var modifiersString = modifierParts.Count > 0
                                  ? string.Join(" ", modifierParts) + " "
                                  : string.Empty;
        var classDeclaration = $"{_visibility.ToString().ToLower()} {modifiersString}class {Name}{genericPart}";
        if (_baseClass is not null) {
            classDeclaration += $" : {_baseClass.Name}";
        }

        if (_interfaces.Count > 0) {
            if (_baseClass is null) {
                classDeclaration += " : ";
            }
            else {
                classDeclaration += ", ";
            }

            classDeclaration += string.Join(", ", _interfaces.Select(i => i.Name));
        }

        writer.WriteLine(classDeclaration);

        if (constraints.Length != 0) {
            writer.Indent++;
            foreach (var constraint in constraints) {
                writer.WriteLine($"{constraint}");
            }

            writer.Indent--;
        }

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var field in _fields) {
            field.Write(writer);
        }

        if (_fields.Count > 0 &&
            (_constructors.Count > 0 || _properties.Count > 0 || _methods.Count > 0)) {
            writer.WriteLine();
        }

        foreach (var constructor in _constructors) {
            constructor.Write(writer);
            writer.WriteLine();
        }

        foreach (var property in _properties) {
            property.Write(writer);
        }

        if (_properties.Count > 0 &&
            _methods.Count    > 0) {
            writer.WriteLine();
        }

        foreach (var method in _methods) {
            method.Write(writer);
            writer.WriteLine();
        }

        // Write regions (if any). Regions appear after non-region members.
        if (_regions.Count > 0) {
            // Extra blank line if previous section had members
            if (_fields.Count       > 0 ||
                _constructors.Count > 0 ||
                _properties.Count   > 0 ||
                _methods.Count      > 0) {
                writer.WriteLine();
            }

            foreach (var region in _regions) {
                writer.WriteLine($"#region {region.Name}");
                writer.WriteLine();

                // Fields
                foreach (var field in region.Fields) {
                    field.Write(writer);
                }

                if (region.Fields.Count > 0 &&
                    (region.Constructors.Count > 0 || region.Properties.Count > 0 || region.Methods.Count > 0)) {
                    writer.WriteLine();
                }

                // Constructors
                foreach (var ctor in region.Constructors) {
                    ctor.Write(writer);
                    writer.WriteLine();
                }

                // Properties
                foreach (var property in region.Properties) {
                    property.Write(writer);
                }

                if (region.Properties.Count > 0 &&
                    region.Methods.Count    > 0) {
                    writer.WriteLine();
                }

                // Methods
                foreach (var method in region.Methods) {
                    method.Write(writer);
                    writer.WriteLine();
                }

                writer.WriteLine($"#endregion // {region.Name}");

                // Blank line between regions but not after last
                if (!ReferenceEquals(region, _regions[^1])) {
                    writer.WriteLine();
                }
            }
        }

        writer.Indent--;
        writer.WriteLine("}");
    }

    /// <summary>
    ///     Adds a field to the class.
    /// </summary>
    /// <param name="field">The field to add.</param>
    public void AddField(FieldWriter field) {
        _fields.Add(field);
    }

    /// <summary>
    ///     Adds a field to a specific region in the class.
    /// </summary>
    /// <param name="field">The field to add.</param>
    /// <param name="region">The region name.</param>
    public void AddField(FieldWriter field,
                         string      region) {
        GetOrAddRegion(region)
           .Fields.Add(field);
    }

    /// <summary>
    ///     Adds a constructor to the class.
    /// </summary>
    /// <param name="constructor">The constructor to add.</param>
    public void AddConstructor(ConstructorWriter constructor) {
        _constructors.Add(constructor);
    }

    /// <summary>
    ///     Adds a constructor to a specific region in the class.
    /// </summary>
    /// <param name="constructor">The constructor to add.</param>
    /// <param name="region">The region name.</param>
    public void AddConstructor(ConstructorWriter constructor,
                               string            region) {
        GetOrAddRegion(region)
           .Constructors.Add(constructor);
    }

    /// <summary>
    ///     Adds a property to the class.
    /// </summary>
    /// <param name="property">The property to add.</param>
    public void AddProperty(PropertyWriter property) {
        _properties.Add(property);
    }

    /// <summary>
    ///     Adds a property to a specific region in the class.
    /// </summary>
    /// <param name="property">The property to add.</param>
    /// <param name="region">The region name.</param>
    public void AddProperty(PropertyWriter property,
                            string         region) {
        GetOrAddRegion(region)
           .Properties.Add(property);
    }

    /// <summary>
    ///     Adds a method to the class.
    /// </summary>
    /// <param name="method">The method to add.</param>
    /// <param name="region">The region name (optional).</param>
    public void AddMethod(IMethodWriter method,
                          string?       region = null) {
        if (region is null) {
            _methods.Add(method);
            return;
        }

        GetOrAddRegion(region)
           .Methods.Add(method);
    }

    /// <summary>
    ///     Adds a region to the class.
    /// </summary>
    /// <param name="name">The name of the region.</param>
    public void AddRegion(string name) {
        GetOrAddRegion(name); // Ensure region exists
    }

    /// <summary>
    ///     Retrieves or creates a region group by name.
    /// </summary>
    /// <param name="name">The name of the region.</param>
    /// <returns>The region group.</returns>
    /// <exception cref="ArgumentException">Thrown if the region name is null or whitespace.</exception>
    private RegionGroup GetOrAddRegion(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            throw new ArgumentException("Region name cannot be null or whitespace", nameof(name));
        }

        var existing = _regions.FirstOrDefault(r => r.Name.Equals(name, StringComparison.Ordinal));
        if (existing is not null) {
            return existing;
        }

        var region = new RegionGroup(name);
        _regions.Add(region);
        return region;
    }

    /// <summary>
    ///     Gets the list of using directives required by the class.
    /// </summary>
    /// <returns>A set of using directives.</returns>
    private HashSet<string> GetUsings() {
        var usings = new HashSet<string>();
        foreach (var @using in _usings) {
            usings.Add(@using);
        }

        if (_baseClass is { Using: { } usingBase }) {
            usings.Add(usingBase);
        }

        foreach (var @interface in _interfaces) {
            if (@interface.Using is null) {
                continue;
            }

            usings.Add(@interface.Using);
        }

        foreach (var genericParameter in _genericParameters) {
            if (genericParameter.Using is null) {
                continue;
            }

            usings.Add(genericParameter.Using);
        }

        foreach (var @using in _methods.SelectMany(x => x.Usings)) {
            usings.Add(@using);
        }

        foreach (var @using in _constructors.SelectMany(x => x.Usings)) {
            usings.Add(@using);
        }

        // Region based constructors & methods
        foreach (var region in _regions) {
            foreach (var @using in region.Constructors.SelectMany(c => c.Usings)) {
                usings.Add(@using);
            }

            foreach (var @using in region.Methods.SelectMany(m => m.Usings)) {
                usings.Add(@using);
            }
        }

        return usings;
    }

    /// <summary>
    ///     Merges multiple <see cref="ClassWriter" /> instances into one.
    /// </summary>
    /// <param name="classes">The classes to merge.</param>
    /// <returns>A new <see cref="ClassWriter" /> instance representing the merged classes.</returns>
    /// <exception cref="ArgumentException">Thrown if the input collection is empty.</exception>
    public static ClassWriter Merge(IReadOnlyCollection<ClassWriter> classes) {
        if (classes.Count == 0) {
            throw new ArgumentException("Cannot merge empty classes", nameof(classes));
        }

        var f      = classes.First();
        var groups = classes.GroupBy(x => x.Region);
        var result = new ClassWriter(f.Name,
                                     f._visibility,
                                     f._classModifiers);
        foreach (var g in groups) {
            foreach (var @using in g.SelectMany(x => x._usings)) {
                result._usings.Add(@using);
            }

            foreach (var method in g.SelectMany(x => x.Methods)) {
                result.AddMethod(method, g.Key);
            }
        }

        return result;
    }

    /// <summary>
    ///     Adds a using directive to the class.
    /// </summary>
    /// <param name="using">The using directive to add.</param>
    public void AddUsing(string @using) {
        _usings.Add(@using);
    }

    /// <summary>
    ///     Represents a group of members within a region.
    /// </summary>
    private sealed class RegionGroup(string name) {
        /// <summary>
        ///     Gets the name of the region.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        ///     Gets the fields in the region.
        /// </summary>
        public List<FieldWriter> Fields { get; } = [];

        /// <summary>
        ///     Gets the constructors in the region.
        /// </summary>
        public List<ConstructorWriter> Constructors { get; } = [];

        /// <summary>
        ///     Gets the properties in the region.
        /// </summary>
        public List<PropertyWriter> Properties { get; } = [];

        /// <summary>
        ///     Gets the methods in the region.
        /// </summary>
        public List<IMethodWriter> Methods { get; } = [];
    }
}
