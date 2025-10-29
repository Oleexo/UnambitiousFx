using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class ClassWriter : IConcreteTypeWriter
{
    private readonly string _name;
    private readonly Visibility _visibility;
    private readonly ClassModifier _classModifiers;
    private readonly GenericParameter[] _genericParameters;
    private readonly TypeDefinitionReference? _baseClass;
    private readonly List<TypeDefinitionReference> _interfaces;
    private readonly DocumentationWriter? _documentation;
    private readonly List<FieldWriter> _fields;
    private readonly List<ConstructorWriter> _constructors;
    private readonly List<PropertyWriter> _properties;
    private readonly List<IMethodWriter> _methods;

    public ClassWriter(string name,
                       Visibility visibility = Visibility.Internal,
                       ClassModifier classModifiers = ClassModifier.None,
                       IEnumerable<GenericParameter>? genericParameters = null,
                       TypeDefinitionReference? baseClass = null,
                       IEnumerable<TypeDefinitionReference>? interfaces = null,
                       DocumentationWriter? documentation = null)
    {
        _name = name;
        _visibility = visibility;
        _classModifiers = classModifiers;
        _genericParameters = genericParameters?.ToArray() ?? [];
        _baseClass = baseClass;
        _interfaces = interfaces?.ToList() ?? [];
        _documentation = documentation;
        _fields = [];
        _constructors = [];
        _properties = [];
        _methods = [];
    }

    public void AddField(FieldWriter field)
    {
        _fields.Add(field);
    }

    public void AddConstructor(ConstructorWriter constructor)
    {
        _constructors.Add(constructor);
    }

    public void AddProperty(PropertyWriter property)
    {
        _properties.Add(property);
    }

    public void AddMethod(IMethodWriter method)
    {
        _methods.Add(method);
    }

    private HashSet<string> GetUsings()
    {
        var usings = new HashSet<string>();

        if (_baseClass is { Using: { } usingBase })
        {
            usings.Add(usingBase);
        }

        foreach (var @interface in _interfaces)
        {
            if (@interface.Using is null)
            {
                continue;
            }

            usings.Add(@interface.Using);
        }

        foreach (var genericParameter in _genericParameters)
        {
            if (genericParameter.Using is null)
            {
                continue;
            }

            usings.Add(genericParameter.Using);
        }

        foreach (var @using in _methods.SelectMany(x => x.Usings))
        {
            usings.Add(@using);
        }

        return usings;
    }

    public IEnumerable<string> Usings => GetUsings();

    public void Write(IndentedTextWriter writer)
    {
        _documentation?.Write(writer);

        var genericPart = _genericParameters is { Length: > 0 }
                              ? $"<{string.Join(", ", _genericParameters.Select(p => p.Name))}>"
                              : string.Empty;

        var constraints = _genericParameters
                         .Where(p => !string.IsNullOrEmpty(p.Constraint))
                         .Select(p => $"where {p.Name} : {p.Constraint}")
                         .ToArray();

        var classDeclaration = $"{_visibility.ToString().ToLower()} {_classModifiers.ToString().Trim().ToLower()} class {_name}{genericPart}";
        if (_baseClass is not null)
        {
            classDeclaration += $" : {_baseClass.Name}";
        }

        if (_interfaces.Count > 0)
        {
            if (_baseClass is null)
            {
                classDeclaration += " : ";
            }
            else
            {
                classDeclaration += ", ";
            }

            classDeclaration += string.Join(", ", _interfaces.Select(i => i.Name));
        }

        writer.WriteLine(classDeclaration);

        if (constraints.Length != 0)
        {
            writer.Indent++;
            foreach (var constraint in constraints)
            {
                writer.WriteLine($"{constraint}");
            }

            writer.Indent--;
        }

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var field in _fields)
        {
            field.Write(writer);
        }

        if (_fields.Count > 0 &&
            (_constructors.Count > 0 || _properties.Count > 0 || _methods.Count > 0))
        {
            writer.WriteLine();
        }

        foreach (var constructor in _constructors)
        {
            constructor.Write(writer);
            writer.WriteLine();
        }

        foreach (var property in _properties)
        {
            property.Write(writer);
        }

        if (_properties.Count > 0 &&
            _methods.Count > 0)
        {
            writer.WriteLine();
        }

        foreach (var method in _methods)
        {
            method.Write(writer);
            writer.WriteLine();
        }

        writer.Indent--;
        writer.WriteLine("}");
    }
}
