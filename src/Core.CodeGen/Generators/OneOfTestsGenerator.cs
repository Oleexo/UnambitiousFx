using UnambitiousFx.Core.CodeGen.Builders.OneOf;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.OneOf;

namespace UnambitiousFx.Core.CodeGen.Generators;

/// <summary>
/// Generator for OneOf unit tests using Template Method pattern.
/// </summary>
internal sealed class OneOfTestsGenerator : BaseCodeGenerator {
    public OneOfTestsGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 2,
                   subNamespace: "OneOf",
                   className: "OneOf",
                   isTest: true)) {
    }

    private ClassWriter GenerateTestClass(ushort arity) {
        return OneOfTestClassBuilder.Build(arity);
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return [
            GenerateTestClass(arity)
        ];
    }
}