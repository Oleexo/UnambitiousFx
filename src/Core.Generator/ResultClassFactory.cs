using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultClassFactory(string @namespace,
                                         ushort maxOfParameters) {
    private void GenerateStaticSuccessMethods(IndentedTextWriter tw) {
        foreach (var i in Enumerable.Range(1, maxOfParameters)) {
            var inputParameters = string.Join(", ", Enumerable.Range(1, i)
                                                              .Select(x => $"TValue{x} value{x}"));
            var callParameters = string.Join(", ", Enumerable.Range(1, i)
                                                             .Select(x => $"value{x}"));
            var genericParameters = string.Join(", ", Enumerable.Range(1, i)
                                                                .Select(x => $"TValue{x}"));
            var whereConstraints = string.Join(" ", Enumerable.Range(1, i)
                                                              .Select(x => $"where TValue{x} : notnull"));


            tw.WriteLine($"public static Result<{genericParameters}> Success<{genericParameters}>({inputParameters}) {whereConstraints}");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"return new SuccessResult<{genericParameters}>({callParameters});");
            tw.Indent--;
            tw.WriteLine("}");
            tw.WriteLine();
        }
    }

    private void GenerateStaticFailureMethods(IndentedTextWriter tw) {
        foreach (var i in Enumerable.Range(1, maxOfParameters)) {
            var genericParameters = string.Join(", ", Enumerable.Range(1, i)
                                                                .Select(x => $"TValue{x}"));
            var whereConstraints = string.Join(" ", Enumerable.Range(1, i)
                                                              .Select(x => $"where TValue{x} : notnull"));

            tw.WriteLine($"public static Result<{genericParameters}> Failure<{genericParameters}>(Exception error) {whereConstraints}");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"return new FailureResult<{genericParameters}>(error);");
            tw.Indent--;
            tw.WriteLine("}");
            tw.WriteLine();
            tw.WriteLine($"public static Result<{genericParameters}> Failure<{genericParameters}>(UnambitiousFx.Core.Results.Reasons.IError error) {whereConstraints}");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"var r = new FailureResult<{genericParameters}>(error.Exception ?? new System.Exception(error.Message), attachPrimaryExceptionalReason: false);");
            tw.WriteLine("r.AddReason(error);");
            tw.WriteLine("foreach (var kv in error.Metadata) r.AddMetadata(kv.Key, kv.Value);");
            tw.WriteLine("return r;");
            tw.Indent--;
            tw.WriteLine("}");
        }
    }

    public SourceText GenerateResult() {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();

        tw.WriteLine("public abstract partial class Result");
        tw.WriteLine("{");
        tw.Indent++;
        
        GenerateStaticSuccessMethods(tw);

        GenerateStaticFailureMethods(tw);

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }
    
}
