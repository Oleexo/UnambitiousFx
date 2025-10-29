using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.Results.TestBuilders;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
/// Generates comprehensive unit tests for Result types with different arities.
/// </summary>
internal sealed class ResultTestGenerator : ICodeGenerator {
    private readonly string _baseNamespace;
    private const    int    StartArity    = 1;
    private const    string DirectoryName = "Results";

    public ResultTestGenerator(string baseNamespace) {
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

        for (ushort i = StartArity; i <= numberOfArity; i++) {
            GenerateTestClass(i, outputPath);
        }
    }

    private void GenerateTestClass(ushort arity,
                                   string outputPath) {
        var fileWriter = new FileWriter($"{_baseNamespace}.Tests.Results");

        var classWriter = ResultTestClassBuilder.Build(arity);
        fileWriter.AddClass(classWriter);
        fileWriter.AddUsing("UnambitiousFx.Core.Results");
        fileWriter.AddUsing("UnambitiousFx.Core.Results.Reasons");

        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }

        var fileName = Path.Combine(outputPath, $"Result{arity}Tests.cs");

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
