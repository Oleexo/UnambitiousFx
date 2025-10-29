using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.Results.Builders;

namespace UnambitiousFx.Core.CodeGen;

internal class ResultCodeGenerator : ICodeGenerator
{
    private readonly string _baseNamespace;
    private const int StartArity = 1;
    private const string DirectoryName = "Results";

    public ResultCodeGenerator(string baseNamespace) => _baseNamespace = baseNamespace;

    public void Generate(ushort numberOfArity,
                         string outputPath)
    {
        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

        // Clean stale previously generated files (old signature versions etc.)
        foreach (var stale in Directory.EnumerateFiles(outputPath, "Result*.cs"))
        {
            File.Delete(stale);
        }

        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            WriteClassFile(outputPath, arity, suffix: string.Empty, ResultBaseClassBuilder.Build(arity));
            WriteClassFile(outputPath, arity, suffix: ".Success", SuccessResultClassBuilder.Build(arity));
            WriteClassFile(outputPath, arity, suffix: ".Failure", FailureResultClassBuilder.Build(arity));
            // Static factory generation currently disabled pending naming strategy to avoid conflict between generic/non-generic type names.
        }
    }

    private void WriteClassFile(string outputPath,
                                ushort arity,
                                string suffix,
                                ClassWriter classWriter)
    {
        var fileWriter = new FileWriter($"{_baseNamespace}.Results");
        fileWriter.AddClass(classWriter);
        var fileName = Path.Combine(outputPath, $"Result{arity}{suffix}.cs");
        using var stringWriter = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);
        File.WriteAllText(fileName, stringWriter.ToString());
    }
}
