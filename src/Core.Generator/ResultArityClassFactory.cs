using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultArityClassFactory(string @namespace,
                                              ushort maxOfParameters) {
    private void GenerateAbstractBindMethods(IndentedTextWriter tw,
                                             ushort             numberOfValues) {
        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateAbstractBindMethod(tw, numberOfValues, i);
        }
    }

    private void GenerateAbstractBindMethod(IndentedTextWriter tw,
                                            ushort             numberOfParameterInput,
                                            ushort             numberOfParameterOutput) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        var where = string.Join(" ", Enumerable.Range(0, numberOfParameterOutput)
                                               .Select(i => $"where TOut{i + 1} : notnull"));

        tw.WriteLine($"public abstract Result<{output}> Bind<{output}>(Func<{input}, Result<{output}>> bind) {where};");
    }

    private void GenerateAbstractOkMethods(IndentedTextWriter tw,
                                           ushort             numberOfParameter) {
        if (numberOfParameter == 1) {
            tw.WriteLine(
                "public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);");
            tw.WriteLine("public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value);");
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));
            tw.WriteLine(
                $"public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);");
            tw.WriteLine($"public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value);");
        }
    }

    private void GenerateSuccessFields(IndentedTextWriter tw,
                                       ushort             numberOfParameter) {
        foreach (ushort i in Enumerable.Range(1, numberOfParameter)) {
            tw.WriteLine($"private readonly TValue{i} _value{i};");
        }
    }

    private void GenerateSuccessConstructor(IndentedTextWriter tw,
                                            ushort             numberOfParameter) {
        tw.WriteLine($"public SuccessResult({string.Join(", ", Enumerable.Range(1, numberOfParameter).Select(i => $"TValue{i} value{i}"))})");
        tw.WriteLine("{");
        tw.Indent++;

        foreach (var i in Enumerable.Range(1, numberOfParameter)) {
            tw.WriteLine($"_value{i} = value{i};");
        }

        tw.Indent--;
        tw.WriteLine("}");
    }

    private void GenerateSuccessBindMethod(IndentedTextWriter tw,
                                           ushort             numberOfParameterInput,
                                           ushort             numberOfParameterOutput,
                                           bool               withIncrementalInput = true) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        var callParameters = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                         .Select(i => $"_value{i + 1}"));


        if (withIncrementalInput) {
            tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<{input}, Result<{output}>> bind)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"return bind({callParameters});");
            tw.Indent--;
            tw.WriteLine("}");
        }
        else {
            tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<Result<{output}>> bind)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("return bind();");
            tw.Indent--;
            tw.WriteLine("}");
        }
    }

    private void GenerateSuccessBindMethods(IndentedTextWriter tw,
                                            ushort             numberOfParameterInput) {
        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateSuccessBindMethod(tw, numberOfParameterInput, i, false);
        }

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateSuccessBindMethod(tw, numberOfParameterInput, i);
        }
    }

    private void GenerateFailureBindMethods(IndentedTextWriter tw,
                                            ushort             numberOfParameterInput) {
        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateFailureBindMethod(tw, numberOfParameterInput, i, false);
        }

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            GenerateFailureBindMethod(tw, numberOfParameterInput, i);
        }
    }

    private void GenerateFailureBindMethod(IndentedTextWriter tw,
                                           ushort             numberOfParameterInput,
                                           ushort             numberOfParameterOutput,
                                           bool               withIncrementalInput = true) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        if (!withIncrementalInput) {
            tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<Result<{output}>> bind)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"return new FailureResult<{output}>(_error);");
            tw.Indent--;
            tw.WriteLine("}");
        }
        else {
            tw.WriteLine($"public override Result<{output}> Bind<{output}>(Func<{input}, Result<{output}>> bind)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"return new FailureResult<{output}>(_error);");
            tw.Indent--;
            tw.WriteLine("}");
        }
    }

    private void GenerateSuccessOkMethods(IndentedTextWriter tw,
                                          ushort             numberOfParameter) {
        tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("error = null;");
        tw.WriteLine("return true;");
        tw.Indent--;
        tw.WriteLine("}");

        if (numberOfParameter == 1) {
            tw.WriteLine(
                "public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = _value1;");
            tw.WriteLine("error = null;");
            tw.WriteLine("return true;");
            tw.Indent--;
            tw.WriteLine("}");

            tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = _value1;");
            tw.WriteLine("return true;");
            tw.Indent--;
            tw.WriteLine("}");
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));
            var fieldParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                              .Select(i => $"_value{i + 1}"));

            tw.WriteLine(
                $"public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"value = ({fieldParameters});");
            tw.WriteLine("error = null;");
            tw.WriteLine("return true;");
            tw.Indent--;
            tw.WriteLine("}");

            tw.WriteLine($"public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine($"value = ({fieldParameters});");
            tw.WriteLine("return true;");
            tw.Indent--;
            tw.WriteLine("}");
        }
    }

    public SourceText GenerateResult(ushort numberOfValues) {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        var genericParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"TValue{i + 1}"));

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();
        tw.WriteLine($"public abstract class Result<{genericParameters}> : BaseResult");
        tw.Indent++;
        for (var i = 0; i < numberOfValues; i++) {
            tw.WriteLine($"where TValue{i + 1} : notnull");
        }

        tw.Indent--;
        tw.WriteLine("{");
        tw.Indent++;

        tw.WriteLine($"public abstract void Match(Action<{genericParameters}> success, Action<Exception> failure);");
        tw.WriteLine($"public abstract TOut Match<TOut>(Func<{genericParameters}, TOut> success, Func<Exception, TOut> failure);");
        tw.WriteLine($"public abstract void IfSuccess(Action<{genericParameters}> action);");

        //GenerateAbstractBindMethods(tw, numberOfValues);
        GenerateAbstractOkMethods(tw, numberOfValues);

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    public SourceText GenerateSuccessResult(ushort numberOfValues) {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        var genericParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"TValue{i + 1}"));
        var callFieldParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                              .Select(i => $"_value{i + 1}"));
        var genericConstraints = string.Join(" ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"where TValue{i + 1} : notnull"));

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();

        tw.WriteLine($"internal sealed class SuccessResult<{genericParameters}> : Result<{genericParameters}> {genericConstraints}");
        tw.WriteLine("{");
        tw.Indent++;

        GenerateSuccessFields(tw, numberOfValues);
        GenerateSuccessConstructor(tw, numberOfValues);

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

        tw.WriteLine($"public override void Match(Action<{genericParameters}> success, Action<Exception> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine($"success({callFieldParameters});");
        tw.Indent--;
        tw.WriteLine("}");

        tw.WriteLine($"public override TOut Match<TOut>(Func<{genericParameters}, TOut> success, Func<Exception, TOut> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine($"return success({callFieldParameters});");
        tw.Indent--;
        tw.WriteLine("}");

        tw.WriteLine($"public override void IfSuccess(Action<{genericParameters}> action)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine($"action({callFieldParameters});");
        tw.Indent--;
        tw.WriteLine("}");

        tw.WriteLine("public override void IfFailure(Action<Exception> action)");
        tw.WriteLine("{");
        tw.WriteLine("}");

        GenerateSuccessOkMethods(tw, numberOfValues);

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    public SourceText GenerateFailureResult(ushort numberOfValues) {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        var genericParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"TValue{i + 1}"));
        var genericConstraints = string.Join(" ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"where TValue{i + 1} : notnull"));

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");
        tw.WriteLine();

        tw.WriteLine($"internal sealed class FailureResult<{genericParameters}> : Result<{genericParameters}> {genericConstraints}");
        tw.WriteLine("{");
        tw.Indent++;

        tw.WriteLine("private readonly Exception _error;");

        tw.WriteLine("public FailureResult(Exception error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("_error = error;");
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

        tw.WriteLine($"public override void Match(Action<{genericParameters}> success, Action<Exception> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("failure(_error);");
        tw.Indent--;
        tw.WriteLine("}");

        tw.WriteLine($"public override TOut Match<TOut>(Func<{genericParameters}, TOut> success, Func<Exception, TOut> failure)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return failure(_error);");
        tw.Indent--;
        tw.WriteLine("}");

        tw.WriteLine($"public override void IfSuccess(Action<{genericParameters}> action)");
        tw.WriteLine("{");
        tw.WriteLine("}");

        tw.WriteLine("public override void IfFailure(Action<Exception> action)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("action(_error);");
        tw.Indent--;
        tw.WriteLine("}");

        GenerateFailureOkMethods(tw, numberOfValues);

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    private void GenerateFailureOkMethods(IndentedTextWriter tw,
                                          ushort             numberOfParameter) {
        tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("error = _error;");
        tw.WriteLine("return false;");
        tw.Indent--;
        tw.WriteLine("}");

        if (numberOfParameter == 1) {
            tw.WriteLine(
                "public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = default;");
            tw.WriteLine("error = _error;");
            tw.WriteLine("return false;");
            tw.Indent--;
            tw.WriteLine("}");

            tw.WriteLine("public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = default;");
            tw.WriteLine("return false;");
            tw.Indent--;
            tw.WriteLine("}");
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));

            tw.WriteLine(
                $"public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = default;");
            tw.WriteLine("error = _error;");
            tw.WriteLine("return false;");
            tw.Indent--;
            tw.WriteLine("}");

            tw.WriteLine($"public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters}) value)");
            tw.WriteLine("{");
            tw.Indent++;
            tw.WriteLine("value = default;");
            tw.WriteLine("return false;");
            tw.Indent--;
            tw.WriteLine("}");
        }
    }
}
