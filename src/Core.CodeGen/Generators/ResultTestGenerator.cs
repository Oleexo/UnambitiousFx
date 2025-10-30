using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results;

namespace UnambitiousFx.Core.CodeGen.Generators;

/// <summary>
/// Generates comprehensive unit tests for Result types with different arities.
/// </summary>
internal sealed class ResultTestGenerator : BaseCodeGenerator
{
    public ResultTestGenerator(string baseNamespace)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: 0,
                   subNamespace: "Results",
                   className: "Result",
                   isTest: true))
    {
    }

    private ClassWriter GenerateTestClass(ushort arity)
    {
        return ResultTestClassBuilder.Build(arity);
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        return [
            GenerateTestClass(arity)
        ];
    }
}
