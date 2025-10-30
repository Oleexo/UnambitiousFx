using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Builders.Results;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

internal sealed class ResultCodeGenerator : ICodeGenerator {
    private readonly string _baseNamespace;
    private const    int    StartArity    = 1;
    private const    string ClassName     = "Result";
    private const    string DirectoryName = "Results";

    public ResultCodeGenerator(string baseNamespace) {
        if (string.IsNullOrWhiteSpace(baseNamespace))
            throw new ArgumentException("Base namespace cannot be null or whitespace.", nameof(baseNamespace));
        _baseNamespace = baseNamespace;
    }

    public void Generate(ushort numberOfArity,
                         string outputPath) {
        if (numberOfArity < StartArity)
            throw new ArgumentOutOfRangeException(nameof(numberOfArity), $"Arity must be >= {StartArity}.");

        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Output path cannot be null or whitespace.", nameof(outputPath));

        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }

        for (ushort i = StartArity; i <= numberOfArity; i++) {
            GenerateResultBase(i, outputPath);
            GenerateSuccessImplementation(i, outputPath);
            GenerateFailureImplementation(i, outputPath);
        }
    }

    private void GenerateResultBase(ushort arity,
                                    string outputPath) {
        var fileWriter = new FileWriter($"{_baseNamespace}.Results");
        fileWriter.AddClass(ResultStaticFactoryBuilder.Build(arity));
        fileWriter.AddClass(ResultBaseClassBuilder.Build(arity));

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.cs");

        WriteFile(fileWriter, fileName);
    }


    private void GenerateSuccessImplementation(ushort arity,
                                               string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.Results");
        fileWriter.AddClass(SuccessResultClassBuilder.Build(arity));

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.Success.cs");

        WriteFile(fileWriter, fileName);
    }

    private void GenerateFailureImplementation(ushort arity,
                                               string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.Results");
        fileWriter.AddClass(FailureResultClassBuilder.Build(arity));

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.Failure.cs");

        WriteFile(fileWriter, fileName);
    }

    private static void WriteFile(FileWriter fileWriter,
                                  string     fileName) {
        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);
        File.WriteAllText(fileName, stringWriter.ToString());
    }




}
