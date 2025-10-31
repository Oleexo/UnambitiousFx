using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.SideEffects;

/// <summary>
/// Builds TapError extension methods for Result types.
/// TapError executes a side effect on failure without changing the result.
/// </summary>
internal sealed class TapErrorMethodBuilder
{
    /// <summary>
    /// Builds a sync TapError method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
            ? "Result"
            : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var parameters = new List<MethodParameter>
        {
            new($"this {resultType}", "result"),
            new("Action<IEnumerable<IError>>", "tap")
        };

        var body = """
                   result.IfFailure(tap);
                   return result;
                   """;

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Executes a side effect if the result is a failure, then returns the original result.")
            .WithParameter("result", "The result instance.")
            .WithParameter("tap", "Action to execute on failure.")
            .WithReturns("The original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapError",
            returnType: resultType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["UnambitiousFx.Core.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async TapError method: Result + AsyncFunc
    /// </summary>
    public MethodWriter BuildAsyncFuncMethod(ushort arity, bool isValueTask)
    {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var parameters = new List<MethodParameter>
        {
            new($"this {resultType}", "result")
        };

        // Build async Func parameter type
        var taskType = isValueTask ? "ValueTask" : "Task";
        parameters.Add(new MethodParameter($"Func<IEnumerable<IError>, {taskType}>", "tap"));

        var body = """
                   if (!result.TryGet(out IEnumerable<IError>? err)) {
                       await tap(err);
                   }

                   return result;
                   """;

        var asyncReturn = isValueTask
            ? $"ValueTask<{resultType}>"
            : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async TapError executing a side effect on failure with async function.")
            .WithParameter("result", "The result instance.")
            .WithParameter("tap", "Async function to execute on failure.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapErrorAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async TapError method: Task + SyncFunc
    /// </summary>
    public MethodWriter BuildTaskSyncFuncMethod(ushort arity, bool isValueTask)
    {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var taskType = isValueTask ? "ValueTask" : "Task";
        var parameters = new List<MethodParameter>
        {
            new($"this {taskType}<{resultType}>", "awaitableResult"),
            new("Action<IEnumerable<IError>>", "tap")
        };

        var body = """
                   var result = await awaitableResult;
                   result.IfFailure(tap);
                   return result;
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async TapError executing a side effect on an awaitable result with sync action.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("tap", "Action to execute on failure.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapErrorAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async TapError method: Task + AsyncFunc
    /// </summary>
    public MethodWriter BuildTaskAsyncFuncMethod(ushort arity, bool isValueTask)
    {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var taskType = isValueTask ? "ValueTask" : "Task";
        var parameters = new List<MethodParameter>
        {
            new($"this {taskType}<{resultType}>", "awaitableResult")
        };

        // Build async Func parameter type
        parameters.Add(new MethodParameter($"Func<IEnumerable<IError>, {taskType}>", "tap"));

        var body = """
                   var result = await awaitableResult;
                   return await result.TapErrorAsync(tap);
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async TapError executing a side effect on an awaitable result with async function.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("tap", "Async function to execute on failure.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapErrorAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Reasons"],
            documentation: docBuilder.Build()
        );
    }
}
