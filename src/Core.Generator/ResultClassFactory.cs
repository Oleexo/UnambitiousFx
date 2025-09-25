using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultClassFactory(string @namespace,
                                         ushort maxOfParameters) {
    private void GenerateAbstractBindMethod(IndentedTextWriter tw,
                                            ushort             numberOfParameterOutput) {
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        var where = string.Join(" ", Enumerable.Range(0, numberOfParameterOutput)
                                               .Select(i => $"where TOut{i + 1} : notnull"));

        tw.WriteLine($"public abstract Result<{output}> Bind<{output}>(Func<Result<{output}>> bind) {where};");
    }

    private void GenerateAbstractBindMethods(IndentedTextWriter tw) {
        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateAbstractBindMethod(tw, i);
        }
    }

    private void GenerateSuccessBindMethods(IndentedTextWriter tw) {
        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateSuccessBindMethod(tw, i);
        }
    }

    private void GenerateFailureBindMethods(IndentedTextWriter tw) {

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
              GenerateFailureBindMethod(tw, i);
        }


    }

    private void GenerateFailureBindMethod(IndentedTextWriter tw,
                                             ushort           numberOfParameterOutput) {
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<Result<{output}>> bind)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine($"return new FailureResult<{output}>(_error);");
        tw.Indent--;
        tw.WriteLine("}");

    }

    private void GenerateSuccessBindMethod(IndentedTextWriter tw,
                                           ushort             numberOfParameterOutput) {
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));

        tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<Result<{output}>> bind)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return bind();");
        tw.Indent--;
        tw.WriteLine("}");
    }

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
        }
    }

    public SourceText GenerateResult() {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();

        tw.WriteLine("public abstract class Result");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("public abstract bool IsFaulted { get; }");
        tw.WriteLine("public abstract bool IsSuccess { get; }");
        tw.WriteLine("public abstract void Match(Action success, Action<Exception> failure);");
        tw.WriteLine("public abstract TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure);");
        tw.WriteLine("public abstract void IfSuccess(Action action);");
        tw.WriteLine("public abstract void IfFailure(Action<Exception> action);");
        tw.WriteLine("public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);");
        tw.WriteLine("public abstract Result Bind(Func<Result> bind);");

        GenerateAbstractBindMethods(tw);

        tw.WriteLine("public static Result Success()");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return new SuccessResult();");
        tw.Indent--;
        tw.WriteLine("}");

        GenerateStaticSuccessMethods(tw);

        tw.WriteLine("public static Result Failure(Exception error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return new FailureResult(error);");
        tw.Indent--;
        tw.WriteLine("}");

        GenerateStaticFailureMethods(tw);

        tw.WriteLine("public static Result Failure(string message)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return new FailureResult(message);");
        tw.Indent--;
        tw.WriteLine("}");

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    public SourceText GenerateSuccessResult() {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();

        tw.WriteLine("internal sealed class SuccessResult : Result");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("public override bool IsFaulted => false;");
        tw.WriteLine("public override bool IsSuccess => true;");
        tw.WriteLine("public override void Match(Action success, Action<Exception> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("success();");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return success();");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override void IfSuccess(Action action)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("action();");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override void IfFailure(Action<Exception> action)");
        tw.WriteLine("{");
        tw.WriteLine("}");
        tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("error = null;");
        tw.WriteLine("return true;");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override Result Bind(Func<Result> bind)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return bind();");
        tw.Indent--;
        tw.WriteLine("}");

        GenerateSuccessBindMethods(tw);
        
        tw.Indent--;
        tw.WriteLine("}");


        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    public SourceText GenerateFailureResult() {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();
        
        tw.WriteLine("internal sealed class FailureResult : Result");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("private readonly Exception _error;");
        tw.WriteLine("public FailureResult(Exception error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("_error = error;");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public FailureResult(string message)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("_error = new Exception(message);");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override bool IsFaulted => true;");
        tw.WriteLine("public override bool IsSuccess => false;");
        tw.WriteLine("public override void Match(Action success, Action<Exception> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("failure(_error);");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return failure(_error);");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override void IfSuccess(Action action)");
        tw.WriteLine("{");
        tw.WriteLine("}");
        tw.WriteLine("public override void IfFailure(Action<Exception> action)");
        tw.WriteLine("{");
        tw.WriteLine("action(_error);");
        tw.WriteLine("}");
        tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("error = _error;");
        tw.WriteLine("return false;");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("public override Result Bind(Func<Result> bind)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return new FailureResult(_error);");
        tw.Indent--;
        tw.WriteLine("}");
        
        GenerateFailureBindMethods(tw);

        tw.Indent--;
        tw.WriteLine("}");
        
        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }
}
