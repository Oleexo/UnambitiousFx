using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.SideEffects;

/// <summary>
/// Builds TapBoth extension methods for Result types.
/// TapBoth executes different side effects for success and failure paths.
/// </summary>
internal sealed class TapBothMethodBuilder {
    /// <summary>
    /// Builds a sync TapBoth method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity) {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var parameters = new List<MethodParameter> {
            new($"this {resultType}", "result")
        };

        // Build Action parameter types
        var onSuccessType = arity == 0
                                ? "Action"
                                : $"Action<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";
        parameters.Add(new MethodParameter(onSuccessType,                 "onSuccess"));
        parameters.Add(new MethodParameter("Action<IEnumerable<IError>>", "onFailure"));

        var body = """
                   result.Match(onSuccess, onFailure);
                   return result;
                   """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Executes different side effects based on success or failure, then returns the original result.")
                                            .WithParameter("result",    "The result instance.")
                                            .WithParameter("onSuccess", "Action to execute on success.")
                                            .WithParameter("onFailure", "Action to execute on failure.")
                                            .WithReturns("The original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapBoth",
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
    /// Builds async TapBoth method: Result + AsyncFuncs
    /// </summary>
    public MethodWriter BuildAsyncFuncMethod(ushort arity,
                                             bool   isValueTask) {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var parameters = new List<MethodParameter> {
            new($"this {resultType}", "result")
        };

        // Build async Func parameter types
        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var onSuccessType = arity == 0
                                ? $"Func<{taskType}>"
                                : $"Func<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}, {taskType}>";
        parameters.Add(new MethodParameter(onSuccessType,                            "onSuccess"));
        parameters.Add(new MethodParameter($"Func<IEnumerable<IError>, {taskType}>", "onFailure"));

        string body;
        if (arity == 0) {
            body = """
                   if (result.IsSuccess) {
                       await onSuccess();
                   }
                   else {
                       await onFailure(result.Errors);
                   }

                   return result;
                   """;
        }
        else {
            // Build TryGet with all value parameters plus error
            var tryGetParams = string.Join(", ", Enumerable.Range(1, arity)
                                                           .Select(i => $"out var value{i}"));
            tryGetParams += ", out var err";
            var onSuccessParams = string.Join(", ", Enumerable.Range(1, arity)
                                                              .Select(i => $"value{i}"));

            body = $$"""
                     if (result.TryGet({{tryGetParams}})) {
                         await onSuccess({{onSuccessParams}});
                     }
                     else {
                         await onFailure(err);
                     }

                     return result;
                     """;
        }

        var asyncReturn = isValueTask
                              ? $"ValueTask<{resultType}>"
                              : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async TapBoth executing different side effects with async functions.")
                                            .WithParameter("result",    "The result instance.")
                                            .WithParameter("onSuccess", "Async function to execute on success.")
                                            .WithParameter("onFailure", "Async function to execute on failure.")
                                            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapBothAsync",
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
    /// Builds async TapBoth method: Task + SyncFuncs
    /// </summary>
    public MethodWriter BuildTaskSyncFuncMethod(ushort arity,
                                                bool   isValueTask) {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var parameters = new List<MethodParameter> {
            new($"this {taskType}<{resultType}>", "awaitableResult")
        };

        // Build Action parameter types
        var onSuccessType = arity == 0
                                ? "Action"
                                : $"Action<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";
        parameters.Add(new MethodParameter(onSuccessType,                 "onSuccess"));
        parameters.Add(new MethodParameter("Action<IEnumerable<IError>>", "onFailure"));

        var body = """
                   var result = await awaitableResult;
                   result.Match(onSuccess, onFailure);
                   return result;
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async TapBoth executing different side effects on an awaitable result with sync actions.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("onSuccess",       "Action to execute on success.")
                                            .WithParameter("onFailure",       "Action to execute on failure.")
                                            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapBothAsync",
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
    /// Builds async TapBoth method: Task + AsyncFuncs
    /// </summary>
    public MethodWriter BuildTaskAsyncFuncMethod(ushort arity,
                                                 bool   isValueTask) {
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = arity == 0
                             ? "Result"
                             : $"Result<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}>";

        var taskType = isValueTask
                           ? "ValueTask"
                           : "Task";
        var parameters = new List<MethodParameter> {
            new($"this {taskType}<{resultType}>", "awaitableResult")
        };

        // Build async Func parameter types
        var onSuccessType = arity == 0
                                ? $"Func<{taskType}>"
                                : $"Func<{GenericTypeHelper.BuildGenericTypeString(arity, "TValue")}, {taskType}>";
        parameters.Add(new MethodParameter(onSuccessType,                            "onSuccess"));
        parameters.Add(new MethodParameter($"Func<IEnumerable<IError>, {taskType}>", "onFailure"));

        var body = """
                   var result = await awaitableResult;
                   return await result.TapBothAsync(onSuccess, onFailure);
                   """;

        var asyncReturn = $"{taskType}<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async TapBoth executing different side effects on an awaitable result with async functions.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("onSuccess",       "Async function to execute on success.")
                                            .WithParameter("onFailure",       "Async function to execute on failure.")
                                            .WithReturns("A task with the original result unchanged.");

        foreach (var i in Enumerable.Range(1, arity)) {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            name: "TapBothAsync",
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
