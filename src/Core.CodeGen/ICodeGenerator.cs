namespace UnambitiousFx.Core.CodeGen;

internal interface ICodeGenerator {
    public void Generate(ushort numberOfArity,
                         string outputPath);
}
