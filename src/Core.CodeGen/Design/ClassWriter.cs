using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class ClassWriter : IConcreteTypeWriter {
    private readonly Visibility                    _visibility;
    private readonly ClassModifier                 _classModifiers;
    private readonly GenericParameter[]            _genericParameters;
    private readonly TypeDefinitionReference?      _baseClass;
    private readonly List<TypeDefinitionReference> _interfaces;
    private readonly List<AttributeReference>      _attributes;
    private readonly DocumentationWriter?          _documentation;
    private readonly List<FieldWriter>             _fields;
    private readonly List<ConstructorWriter>       _constructors;
    private readonly List<PropertyWriter>          _properties;
    private readonly List<IMethodWriter>           _methods;
    private readonly List<RegionGroup>             _regions;

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
    }

    public string? Region     { get; set; }
    public string? Namespace  { get; set; }
    public string? UnderClass { get; set; }
    public string  Name       { get; }

    public void AddField(FieldWriter field) {
        _fields.Add(field);
    }

    public void AddField(FieldWriter field,
                         string      region) {
        GetOrAddRegion(region)
           .Fields.Add(field);
    }

    public void AddConstructor(ConstructorWriter constructor) {
        _constructors.Add(constructor);
    }

    public void AddConstructor(ConstructorWriter constructor,
                               string            region) {
        GetOrAddRegion(region)
           .Constructors.Add(constructor);
    }

    public void AddProperty(PropertyWriter property) {
        _properties.Add(property);
    }

    public void AddProperty(PropertyWriter property,
                            string         region) {
        GetOrAddRegion(region)
           .Properties.Add(property);
    }

    public void AddMethod(IMethodWriter method,
                          string?       region = null) {
        if (region is null) {
            _methods.Add(method);
            return;
        }

        GetOrAddRegion(region)
           .Methods.Add(method);
    }

    public void AddRegion(string name) {
        GetOrAddRegion(name); // Ensure region exists
    }

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

    private HashSet<string> GetUsings() {
        var usings = new HashSet<string>();

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

    public IEnumerable<string> Usings => GetUsings();

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
        if ((_classModifiers & ClassModifier.Static)   != 0) modifierParts.Add("static");
        if ((_classModifiers & ClassModifier.Abstract) != 0) modifierParts.Add("abstract");
        if ((_classModifiers & ClassModifier.Sealed)   != 0) modifierParts.Add("sealed");
        if ((_classModifiers & ClassModifier.Partial)  != 0) modifierParts.Add("partial");
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

    public static ClassWriter Merge(IReadOnlyCollection<ClassWriter> classes) {
        if (classes.Count == 0) {
            throw new ArgumentException("Cannot merge empty classes", nameof(classes));
        }

        var f      = classes.First();
        var groups = classes.GroupBy(x => x.Region);
        var result = new ClassWriter(f.Name,
                                     visibility: f._visibility,
                                     classModifiers: f._classModifiers);
        foreach (var g in groups) {
            foreach (var method in g.SelectMany(x => x.Methods)) {
                result.AddMethod(method, g.Key);
            }
        }

        return result;
    }

    public IEnumerable<IMethodWriter> Methods => _methods.Concat(_regions.SelectMany(r => r.Methods));

    private sealed class RegionGroup(string name) {
        public string                  Name         { get; } = name;
        public List<FieldWriter>       Fields       { get; } = [];
        public List<ConstructorWriter> Constructors { get; } = [];
        public List<PropertyWriter>    Properties   { get; } = [];
        public List<IMethodWriter>     Methods      { get; } = [];
    }
}
