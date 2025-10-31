using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
/// Builds Try extension methods for Result types.
/// </summary>
internal sealed class TryMethodBuilder
{
    private readonly string _baseNamespace;

    public TryMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    /// Builds a standalone Try method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var returnType = $"Result<{outTypes}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build function signature and body
        string funcSignature;
        string bindCall;

        if (arity == 1)
        {
            funcSignature = "Func<TValue1, TOut1>";
            bindCall = """
value => {
                try {
                    var newValue = func(value);
                    return Result.Success(newValue);
                }
                catch (Exception ex) {
                    return Result.Failure<TOut1>(ex);
                }
            }
""";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {outTuple}>";

            var lambdaParams = string.Join(",\n                            ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));
            var funcParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"value{n}"));
            var successParams = string.Join(", ",
                Enumerable.Range(1, arity).Select(n => $"items.Item{n}"));
            var failureTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TOut{n}"));

            bindCall = $$$"""
({{{lambdaParams}}}) => {
                try {
                    var items = func({{{funcParams}}});
                    return Result.Success({{{successParams}}});
                }
                catch (Exception ex) {
                    return Result.Failure<{{{failureTypes}}}>(ex);
                }
            }
""";
        }

        var body = $"""
                    return result.Bind({bindCall});
                    """;

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Executes a function that may throw, catching exceptions and converting to Result failure.")
            .WithParameter("result", "The result instance.")
            .WithParameter("func", "The function to execute.")
            .WithReturns("A new result with transformed value(s) or failure.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }
        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            name: "Try",
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature, "func")
            ],
            genericParameters: genericParams.ToArray(),
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Try method: Result + async func.
    /// </summary>
    public MethodWriter BuildAsyncFuncMethod(ushort arity, bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var returnType = $"Result<{outTypes}>";
        var asyncType = isValueTask ? "ValueTask" : "Task";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build async function signature
        string funcSignature;
        string body;

        if (arity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<TOut1>>";
            body = """
                   return result.BindAsync(async value =>
                   {
                       try
                       {
                           var newValue = await func(value).ConfigureAwait(false);;
                           return Result.Success(newValue);
                       }
                       catch (Exception ex)
                       {
                           return Result.Failure<TOut1>(ex);
                       }
                   });
                   """;
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {asyncType}<{outTuple}>>";

            var matchParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"v{n}"));
            var successParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value.Item{n}"));

            body = $$"""
                     return result.BindAsync(async ({{matchParams}}) =>
                     {
                         try
                         {
                             var value = await func({{matchParams}}).ConfigureAwait(false);;
                             return Result.Success({{successParams}});
                         }
                         catch (Exception ex)
                         {
                             return Result.Failure<{{outTypes}}>(ex);
                         }
                     });
                     """;
        }

        var asyncReturn = isValueTask
            ? $"ValueTask<{returnType}>"
            : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Try executing an async function, catching exceptions and converting to Result.Failure.")
            .WithParameter("result", "The result instance.")
            .WithParameter("func", "The async function to execute.")
            .WithReturns("A task with the result of the operation.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }
        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            name: "TryAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier:  MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature, "func")
            ],
            genericParameters: genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Try method: Task + sync func.
    /// </summary>
    public MethodWriter BuildTaskSyncFuncMethod(ushort arity, bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var asyncResultType = isValueTask
            ? $"ValueTask<{resultType}>"
            : $"Task<{resultType}>";
        var returnType = $"Result<{outTypes}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        if (arity == 1)
        {
            funcSignature = "Func<TValue1, TOut1>";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {outTuple}>";
        }

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return result.Try(func);
                   """;

        var asyncReturn = isValueTask
            ? $"ValueTask<{returnType}>"
            : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Try awaiting result then executing a sync function with exception handling.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("func", "The function to execute.")
            .WithReturns("A task with the result of the operation.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }
        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            name: "TryAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature, "func")
            ],
            genericParameters: genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Try method: Task + async func.
    /// </summary>
    public MethodWriter BuildTaskAsyncFuncMethod(ushort arity, bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var asyncResultType = isValueTask
            ? $"ValueTask<{resultType}>"
            : $"Task<{resultType}>";
        var returnType = $"Result<{outTypes}>";
        var asyncType = isValueTask ? "ValueTask" : "Task";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build async function signature
        string funcSignature;
        if (arity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<TOut1>>";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {asyncType}<{outTuple}>>";
        }

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return await result.TryAsync(func).ConfigureAwait(false);
                   """;

        var asyncReturn = isValueTask
            ? $"ValueTask<{returnType}>"
            : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Try awaiting result then executing an async function with exception handling.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("func", "The async function to execute.")
            .WithReturns("A task with the result of the operation.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }
        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            name: "TryAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature, "func")
            ],
            genericParameters: genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }
}
