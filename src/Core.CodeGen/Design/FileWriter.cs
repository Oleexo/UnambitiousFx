using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class FileWriter {
    private readonly string                      _namespace;
    private readonly bool                        _useNullable;
    private readonly List<ITypeDefinitionWriter> _typeBuilders;
    private readonly List<string>                _usings;

    public FileWriter(string                              @namespace,
                      IEnumerable<ITypeDefinitionWriter>? typeBuilders = null,
                      bool                                useNullable  = true) {
        _namespace   = @namespace;
        _useNullable = useNullable;
        _typeBuilders = typeBuilders is not null
                            ? typeBuilders.ToList()
                            : [];
        _usings = [];
    }

    public FileWriter AddClass(ClassWriter @class) {
        _typeBuilders.Add(@class);
        return this;
    }

    public void Write(IndentedTextWriter writer) {
        if (_useNullable) {
            writer.WriteLine("#nullable enable");
            writer.WriteLine();
        }

        var usings = _typeBuilders.SelectMany(x => x.Usings)
                                  .Concat(_usings)
                                  .Distinct()
                                  .Order();

        foreach (var @using in usings) {
            writer.WriteLine($"using {@using};");
        }

        writer.WriteLine();
        writer.WriteLine($"namespace {_namespace};");
        writer.WriteLine();

        foreach (var typeBuilder in _typeBuilders) {
            typeBuilder.Write(writer);
        }
    }

    public void AddUsing(string @using) {
        _usings.Add(@using);
    }
}
