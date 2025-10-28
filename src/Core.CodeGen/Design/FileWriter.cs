using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed class FileWriter {
    private readonly string                      _namespace;
    private readonly List<ITypeDefinitionWriter> _typeBuilders;

    public FileWriter(string                              @namespace,
                      IEnumerable<ITypeDefinitionWriter>? typeBuilders = null) {
        _namespace = @namespace;
        _typeBuilders = typeBuilders is not null
                            ? typeBuilders.ToList()
                            : [];
    }

    public FileWriter AddClass(ClassWriter @class) {
        _typeBuilders.Add(@class);
        return this;
    }

    public void Write(IndentedTextWriter writer) {
        var usings = _typeBuilders.SelectMany(x => x.Usings)
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
}
