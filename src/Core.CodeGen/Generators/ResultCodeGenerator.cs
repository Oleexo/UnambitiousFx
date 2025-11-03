using UnambitiousFx.Core.CodeGen.Builders.Results;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Generators;

internal sealed class ResultCodeGenerator : BaseCodeGenerator {
    private const int    StartArity   = 1;
    private const string ClassName    = "Result";
    private const string SubNamespace = "Results";

    public ResultCodeGenerator(string               baseNamespace,
                               FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
        : base(new GenerationConfig(
                   baseNamespace,
                   StartArity,
                   SubNamespace,
                   ClassName,
                   fileOrganization)) {
    }

    private IEnumerable<ClassWriter> GenerateResultBase(ushort arity) {
        yield return ResultStaticFactoryBuilder.Build(arity);
        yield return ResultBaseClassBuilder.Build(arity);
    }

    private ClassWriter GenerateSuccessImplementation(ushort arity) {
        var classWriter = SuccessResultClassBuilder.Build(arity);
        classWriter.UnderClass = ClassName;
        return classWriter;
    }

    private ClassWriter GenerateFailureImplementation(ushort arity) {
        var classWriter = FailureResultClassBuilder.Build(arity);
        classWriter.UnderClass = ClassName;
        return classWriter;
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var result = new List<ClassWriter>();
        result.AddRange(GenerateResultBase(arity));
        result.Add(GenerateSuccessImplementation(arity));
        result.Add(GenerateFailureImplementation(arity));
        return result;
    }
}
