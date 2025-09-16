using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal class ResultClassFactory {
    private readonly string _ns;
    private readonly ushort _maxOfParameters;

    public ResultClassFactory(string @namespace,
                              ushort maxOfParameters) {
        _ns              = @namespace;
        _maxOfParameters = maxOfParameters;
    }

    private string GenerateAbstractBindMethod(ushort numberOfParameterInput,
                                              ushort numberOfParameterOutput,
                                              bool   withIncrementalInput) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        var where = string.Join(" ", Enumerable.Range(0, numberOfParameterOutput)
                                               .Select(i => $"where TOut{i + 1} : notnull"));
        if (!withIncrementalInput) {
            return $"public abstract Result<{output}> Bind<{output}>(Func<Result<{output}>> bind) {where};";
        }

        return $"public abstract Result<{output}> Bind<{output}>(Func<{input}, Result<{output}>> bind) {where};";
    }

    private string GenerateAbstractBindMethods(ushort numberOfParameterInput,
                                               bool   withIncrementalInput = true) {
        var sb = new StringBuilder();
        foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
            var line = GenerateAbstractBindMethod(numberOfParameterInput, (ushort)i, withIncrementalInput);
            if (i > 1) {
                sb.Append('\t');
            }

            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    private string GenerateSuccessBindMethods(ushort numberOfParameterInput,
                                              bool   withIncrementalInput = true) {
        var sb = new StringBuilder();

        foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
            var line = GenerateSuccessBindMethod(numberOfParameterInput, (ushort)i, false);
            if (i > 1) {
                sb.Append('\t');
            }

            sb.AppendLine(line);
        }

        if (withIncrementalInput) {
            foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
                var line = GenerateSuccessBindMethod(numberOfParameterInput, (ushort)i);
                sb.Append('\t');
                sb.AppendLine(line);
            }
        }

        return sb.ToString();
    }

    private string GenerateFailureBindMethods(ushort numberOfParameterInput,
                                              bool   withIncrementalInput = true) {
        var sb = new StringBuilder();

        foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
            var line = GenerateFailureBindMethod(numberOfParameterInput, (ushort)i, false);
            if (i > 1) {
                sb.Append('\t');
            }

            sb.AppendLine(line);
        }

        if (withIncrementalInput) {
            foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
                var line = GenerateFailureBindMethod(numberOfParameterInput, (ushort)i);
                sb.Append('\t');
                sb.AppendLine(line);
            }
        }

        return sb.ToString();
    }

    private string GenerateFailureBindMethod(ushort numberOfParameterInput,
                                             ushort numberOfParameterOutput,
                                             bool   withIncrementalInput = true) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        if (!withIncrementalInput) {
            return $$"""
                     public override Result<{{output}}> Bind<{{output}}>(Func<Result<{{output}}>> bind) 
                     {
                         return new FailureResult<{{output}}>(_error);
                     }
                     """;
        }

        return $$"""
                 public override Result<{{output}}> Bind<{{output}}>(Func<{{input}}, Result<{{output}}>> bind) 
                 {
                     return new FailureResult<{{output}}>(_error);
                 }
                 """;
    }

    private string GenerateSuccessBindMethod(ushort numberOfParameterInput,
                                             ushort numberOfParameterOutput,
                                             bool   withIncrementalInput = true) {
        var input = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                .Select(i => $"TValue{i + 1}"));
        var output = string.Join(", ", Enumerable.Range(0, numberOfParameterOutput)
                                                 .Select(i => $"TOut{i + 1}"));
        var callParameters = string.Join(", ", Enumerable.Range(0, numberOfParameterInput)
                                                         .Select(i => $"_value{i + 1}"));

        if (!withIncrementalInput) {
            return $$"""
                     public override Result<{{output}}> Bind<{{output}}>(Func<Result<{{output}}>> bind) 
                     {
                         return bind();
                     }
                     """;
        }

        return $$"""
                 public override Result<{{output}}> Bind<{{output}}>(Func<{{input}}, Result<{{output}}>> bind) 
                 {
                     return bind({{callParameters}});
                 }
                 """;
    }

    private string GenerateSuccessFields(ushort numberOfParameter) {
        var sb = new StringBuilder();

        foreach (var i in Enumerable.Range(1, numberOfParameter)) {
            if (i > 1) {
                sb.Append('\t');
            }

            sb.AppendLine($"private readonly TValue{i} _value{i};");
        }

        return sb.ToString();
    }

    private string GenerateSuccessConstructor(ushort numberOfParameter) {
        var sb = new StringBuilder();

        sb.AppendLine($"public SuccessResult({string.Join(", ", Enumerable.Range(1, numberOfParameter).Select(i => $"TValue{i} value{i}"))})");
        sb.AppendLine("\t{");
        foreach (var i in Enumerable.Range(1, numberOfParameter)) {
            sb.AppendLine($"\t\t_value{i} = value{i};");
        }

        sb.AppendLine("\t}");

        return sb.ToString();
    }

    private string GenerateAbstractOkMethods(ushort numberOfParameter) {
        var sb = new StringBuilder();

        if (numberOfParameter == 1) {
            sb.AppendLine(
                "public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);");
            sb.AppendLine("\tpublic abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value);");
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));
            sb.AppendLine(
                $"public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters})? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);");
            sb.AppendLine($"\tpublic abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({methodParameters})? value);");
        }

        return sb.ToString();
    }

    private string GenerateSuccessOkMethods(ushort numberOfParameter) {
        var sb = new StringBuilder();

        sb.AppendLine("""
                      public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                      {
                          error = null;
                          return true;
                      }
                      """);

        if (numberOfParameter == 1) {
            sb.AppendLine("""
                          public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                          {
                              value = _value1;
                              error = null;
                              return true;
                          }
                          """);
            sb.AppendLine("""
                          public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value) 
                          {
                              value = _value1;
                              return true;
                          }
                          """);
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));
            var fieldParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                              .Select(i => $"_value{i + 1}"));
            sb.AppendLine($$"""
                            public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({{methodParameters}})? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                            {
                                value = ({{fieldParameters}});
                                error = null;
                                return true;
                            }
                            """);
            sb.AppendLine($$"""
                            public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({{methodParameters}})? value) 
                            {
                                value = ({{fieldParameters}});
                                return true;
                            }
                            """);
        }

        return sb.ToString();
    }

    private string GenerateFailureOkMethods(ushort numberOfParameter) {
        var sb = new StringBuilder();

        sb.AppendLine("""
                      public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                      {
                          error = _error;
                          return false;
                      }
                      """);

        if (numberOfParameter == 1) {
            sb.AppendLine("""
                          public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                          {
                              value = default;
                              error = _error;
                              return false;
                          }
                          """);
            sb.AppendLine("""
                          public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TValue1? value) 
                          {
                              value = default;
                              return false;
                          }
                          """);
        }
        else {
            var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfParameter)
                                                               .Select(i => $"TValue{i + 1} value{i + 1}"));
            sb.AppendLine($$"""
                            public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({{methodParameters}})? value, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) 
                            {
                                value = default;
                                error = _error;
                                return false;
                            }
                            """);
            sb.AppendLine($$"""
                            public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ({{methodParameters}})? value) 
                            {
                                value = default;
                                return false;
                            }
                            """);
        }

        return sb.ToString();
    }
    
    private string GenerateStaticSuccessMethods() {
        var sb = new StringBuilder();

        foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
            var inputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x} value{x}"));
            var callParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"value{x}"));
            var genericParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
            var whereConstraints = string.Join(" ", Enumerable.Range(1, i).Select(x => $"where TValue{x} : notnull"));
            if (i > 1) {
                sb.Append('\t');
            }
            sb.AppendLine($$"""
                          public static Result<{{genericParameters}}> Success<{{genericParameters}}>({{inputParameters}}) {{whereConstraints}}
                          {
                              return new SuccessResult<{{genericParameters}}>({{callParameters}});
                          }
                          """);
        }

        return sb.ToString();
    }

    private string GenerateStaticFailureMethods() {
        var sb = new StringBuilder();

        foreach (var i in Enumerable.Range(1, _maxOfParameters)) {
            var inputParameters   = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x} value{x}"));
            var callParameters    = string.Join(", ", Enumerable.Range(1, i).Select(x => $"value{x}"));
            var genericParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
            var whereConstraints  = string.Join(" ",  Enumerable.Range(1, i).Select(x => $"where TValue{x} : notnull"));
            if (i > 1) {
                sb.Append('\t');
            }
            sb.AppendLine($$"""
                            public static Result<{{genericParameters}}> Failure<{{genericParameters}}>(Exception error) {{whereConstraints}}
                            {
                                return new FailureResult<{{genericParameters}}>(error);
                            }
                            """);
        }

        return sb.ToString();
    }

    public SourceText Generate() {
        var code = $$"""
                     #nullable enable
                     namespace {{_ns}};

                     public abstract class Result 
                     {
                        public abstract bool IsFaulted { get; }

                        public abstract bool IsSuccess { get; }

                        public abstract void Match(Action            success,
                                                   Action<Exception> failure);

                        public abstract TOut Match<TOut>(Func<TOut>            success,
                                                         Func<Exception, TOut> failure);

                        public abstract void IfSuccess(Action action);

                        public abstract void IfFailure(Action<Exception> action);

                        public abstract bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error);
                        
                        public abstract Result Bind(Func<Result> bind);
                        
                        {{GenerateAbstractBindMethods(_maxOfParameters, false)}}
                        
                        public static Result Success() 
                        {
                            return new SuccessResult();
                        }
                        
                        {{GenerateStaticSuccessMethods()}}
                        
                        public static Result Failure(Exception error) 
                        {
                            return new FailureResult(error);
                        }
                        
                        {{GenerateStaticFailureMethods()}}

                        public static Result Failure(string message) 
                        {
                            return new FailureResult(message);
                        }
                     }

                     internal sealed class SuccessResult : Result
                     {
                        public override bool IsFaulted => false;
                        
                        public override bool IsSuccess => true;
                        
                        public override void Match(Action success, Action<Exception> failure)
                        {
                            success();
                        }
                        
                        public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure)
                        {
                            return success();
                        }
                        
                        public override void IfSuccess(Action action) 
                        {
                            action();
                        }
                        public override void IfFailure(Action<Exception> action)
                        {
                        }
                        
                        public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)
                        {
                            error = null;
                            return true;
                        }
                        
                        public override Result Bind(Func<Result> bind)
                        {
                            return bind();
                        }
                        
                        {{GenerateSuccessBindMethods(_maxOfParameters, false)}}
                     }

                     internal sealed class FailureResult : Result
                     {
                        private readonly Exception _error;
                        
                        public FailureResult(Exception error) 
                        {
                            _error = error;
                        }
                        
                        public FailureResult(string message) 
                        {
                            _error = new Exception(message);
                        }
                        
                        public override bool IsFaulted => true;

                        public override bool IsSuccess => false;
                        
                        public override void Match(Action success, Action<Exception> failure)
                        {
                            failure(_error);
                        }
                        
                        public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure)
                        {
                            return failure(_error);
                        }
                        
                        public override void IfSuccess(Action action) 
                        {
                        }
                        public override void IfFailure(Action<Exception> action)
                        {
                            action(_error);
                        }
                        
                        public override bool Ok([global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error)
                        {
                            error = _error;
                            return false;
                        }
                        
                        public override Result Bind(Func<Result> bind)
                        {
                            return new FailureResult(_error);
                        }
                        
                        {{GenerateFailureBindMethods(_maxOfParameters, false)}}
                     }
                     """;

        return SourceText.From(code, Encoding.UTF8);
    }

    public SourceText Generate(ushort numberOfValues) {
        var genericParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"TValue{i + 1}"));
        var methodParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                           .Select(i => $"TValue{i + 1} value{i + 1}"));
        var callParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                         .Select(i => $"value{i + 1}"));
        var callFieldParameters = string.Join(", ", Enumerable.Range(0, numberOfValues)
                                                              .Select(i => $"_value{i + 1}"));
        var genericConstraints = string.Join(" ", Enumerable.Range(0, numberOfValues)
                                                            .Select(i => $"where TValue{i + 1} : notnull"));
        var code = $$"""
                     #nullable enable
                     namespace {{_ns}};

                     public abstract class Result<{{genericParameters}}> : Result {{genericConstraints}}
                     {
                         public abstract void Match(Action<{{genericParameters}}> success, Action<Exception> failure);
                      
                         public abstract TOut Match<TOut>(Func<{{genericParameters}}, TOut> success, Func<Exception, TOut> failure);
                      
                         public abstract void IfSuccess(Action<{{genericParameters}}> action);
                      
                         {{GenerateAbstractBindMethods(numberOfValues)}}
                         
                         {{GenerateAbstractOkMethods(numberOfValues)}}
                     }

                     internal sealed class SuccessResult<{{genericParameters}}> : Result<{{genericParameters}}> {{genericConstraints}}
                     {
                        {{GenerateSuccessFields(numberOfValues)}}
                         
                        {{GenerateSuccessConstructor(numberOfValues)}}
                      
                        public override bool IsFaulted => false;

                        public override bool IsSuccess => true;  

                        public override void Match(Action success, Action<Exception> failure) 
                        {
                            success();
                        }
                        
                        public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) 
                        {
                            return success();
                        }
                        
                        public override void IfSuccess(Action action) 
                        {
                            action();
                        }

                        public override void Match(Action<{{genericParameters}}> success, Action<Exception> failure) 
                        {
                            success({{callFieldParameters}});
                        }

                        public override TOut Match<TOut>(Func<{{genericParameters}}, TOut> success, Func<Exception, TOut> failure) 
                        {
                            return success({{callFieldParameters}});
                        }

                        public override void IfSuccess(Action<{{genericParameters}}> action) 
                        {
                            action({{callFieldParameters}});
                        }

                        public override void IfFailure(Action<Exception> action) 
                        {
                        }
                        
                        public override Result Bind(Func<Result> bind) 
                        {
                            return bind();
                        }

                        {{GenerateSuccessBindMethods(numberOfValues)}}
                        
                        {{GenerateSuccessOkMethods(numberOfValues)}}
                     }

                     internal sealed class FailureResult<{{genericParameters}}> : Result<{{genericParameters}}> {{genericConstraints}}
                     {
                        private readonly Exception _error;

                        public override bool IsFaulted => true;

                        public override bool IsSuccess => false;  

                        public FailureResult(Exception error) 
                        {
                            _error = error;
                        }

                         public override void Match(Action success, Action<Exception> failure) 
                         {
                             failure(_error);
                         }
                         
                         public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) 
                         {
                             return failure(_error);
                         }

                        public override void Match(Action<{{genericParameters}}> success, Action<Exception> failure) 
                        {
                            failure(_error);
                        }
                        
                        public override TOut Match<TOut>(Func<{{genericParameters}}, TOut> success, Func<Exception, TOut> failure) 
                        {
                            return failure(_error);
                        }
                        
                        public override void IfSuccess(Action<{{genericParameters}}> action) 
                        {
                        }
                        
                        public override void IfSuccess(Action action) 
                        {
                        }
                        
                        public override void IfFailure(Action<Exception> action) 
                        {
                            action(_error);
                        }
                        
                        public override Result Bind(Func<Result> bind) 
                        {
                            return new FailureResult(_error);
                        }
                        
                        {{GenerateFailureBindMethods(numberOfValues)}}
                        
                        {{GenerateFailureOkMethods(numberOfValues)}}
                     }
                     """;

        return SourceText.From(code, Encoding.UTF8);
    }
}
