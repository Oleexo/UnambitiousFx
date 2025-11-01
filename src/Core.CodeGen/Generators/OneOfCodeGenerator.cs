using UnambitiousFx.Core.CodeGen.Builders.OneOf;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators;

/// <summary>
/// Generator for OneOf types using Template Method pattern.
/// </summary>
internal sealed class OneOfCodeGenerator : BaseCodeGenerator {
    public OneOfCodeGenerator(string               baseNamespace,
                              FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 2,
                   subNamespace: "OneOf",
                   className: "OneOf",
                   fileOrganization: fileOrganization)) {
    }

    private ClassWriter GenerateOneOfBase(ushort arity) {
        return OneOfBaseClassBuilder.Build(arity, Config.BaseNamespace, Config.ClassName);
    }

    private IEnumerable<ClassWriter> GenerateOneOfImplementations(ushort arity) {
        for (ushort position = 1; position <= arity; position++) {
            var classWriter = OneOfImplementationBuilder.Build(arity, position);
            classWriter.UnderClass = "OneOf";
            yield return classWriter;
        }
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var result = new List<ClassWriter>();
        result.Add(GenerateOneOfBase(arity));
        result.AddRange(GenerateOneOfImplementations(arity));
        return result;
    }
}