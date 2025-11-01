using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.SideEffects;

/// <summary>
/// Builds Tap extension methods for Result types.
/// Tap executes a side effect on success without changing the result.
/// </summary>
internal sealed class TapMethodBuilder
{
    /// <summary>
    /// Builds a sync Tap method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
            ? "Result"
            : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var parameters = new List<MethodParameter>
        {
            new($"this {resultType}", "result")
        };

        // Build Action parameter type
        var actionType = arity == 0
            ? "Action"
            : $"Action<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";
        parameters.Add(new MethodParameter(actionType, "tap"));

        var body = arity == 0
            ? """
              result.IfSuccess(tap);
              return result;
              """
            : """
              result.IfSuccess(tap);
              return result;
              """;

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Executes a side effect if the result is successful, then returns the original result.")
            .WithParameter("result", "The result instance.")
            .WithParameter("tap", "Action to execute on success.")
            .WithReturns("The original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "Tap",
            returnType: resultType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Tap method: Result + AsyncFunc (TValue -> Task)
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
        var funcType = arity == 0
            ? $"Func<{taskType}>"
            : $"Func<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}, {taskType}>";
        parameters.Add(new MethodParameter(funcType, "tap"));

        string body;
        if (arity == 0)
        {
            body = """
                   if (result.IsSuccess) {
                       await tap();
                   }

                   return result;
                   """;
        }
        else
        {
            // Build TryGet with all value parameters
            var tryGetParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"out var value{i}"));
            var tapParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"));

            body = $$"""
                     if (result.TryGet({{tryGetParams}})) {
                         await tap({{tapParams}});
                     }

                     return result;
                     """;
        }

        var asyncReturn = isValueTask
            ? $"ValueTask<{resultType}>"
            : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Tap executing a side effect on success with async function.")
            .WithParameter("result", "The result instance.")
            .WithParameter("tap", "Async function to execute on success.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Tap method: Task + SyncFunc (TValue -> void)
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
            new($"this {taskType}<{resultType}>", "awaitableResult")
        };

        // Build Action parameter type
        var actionType = arity == 0
            ? "Action"
            : $"Action<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";
        parameters.Add(new MethodParameter(actionType, "tap"));

        var body = """
                   var result = await awaitableResult;
                   result.IfSuccess(tap);
                   return result;
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Tap executing a side effect on an awaitable result with sync action.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("tap", "Action to execute on success.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    /// Builds async Tap method: Task + AsyncFunc (TValue -> Task)
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
        var funcType = arity == 0
            ? $"Func<{taskType}>"
            : $"Func<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}, {taskType}>";
        parameters.Add(new MethodParameter(funcType, "tap"));

        var body = """
                   var result = await awaitableResult;
                   return await result.TapAsync(tap);
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
            .WithSummary("Async Tap executing a side effect on an awaitable result with async function.")
            .WithParameter("awaitableResult", "The awaitable result instance.")
            .WithParameter("tap", "Async function to execute on success.")
            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapAsync",
            returnType: asyncReturn,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async | MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }
}