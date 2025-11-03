using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Base class for Result test generators that provides common functionality for generating test values,
///     result creation, and test method structures across sync, Task, and ValueTask variants.
/// </summary>
internal abstract class ResultTestGeneratorBase : BaseCodeGenerator
{
    protected ResultTestGeneratorBase(GenerationConfig config) : base(config)
    {
    }

    #region Common Test Value Generation

    protected string GenerateTestValues(ushort arity)
    {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    protected string GetTestValue(int index)
    {
        return index switch
        {
            1 => "42",
            2 => "\"test\"",
            3 => "true",
            4 => "3.14",
            5 => "123L",
            _ => $"\"value{index}\""
        };
    }

    protected string GetTestType(int index)
    {
        return index switch
        {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "long",
            _ => "string"
        };
    }

    #endregion

    #region Result Creation Helpers

    protected string GenerateResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = Result.Success();";
        }

        if (arity == 1)
        {
            return "var result = Result.Success(value1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    protected string GenerateTaskResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var taskResult = Task.FromResult(Result.Success());";
        }

        if (arity == 1)
        {
            return "var taskResult = Task.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var taskResult = Task.FromResult(Result.Success({values}));";
    }

    protected string GenerateValueTaskResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success());";
        }

        if (arity == 1)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Success({values}));";
    }

    protected string GenerateFailureResultCreation(ushort arity, string errorMessage = "\"Test error\"")
    {
        if (arity == 0)
        {
            return $"var result = Result.Failure({errorMessage});";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>({errorMessage});";
    }

    protected string GenerateErrorTypeFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = Result.Failure(new Error(\"Test error\"));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(new Error(\"Test error\"));";
    }

    protected string GenerateTaskFailureResultCreation(ushort arity, string errorMessage = "\"Test error\"")
    {
        if (arity == 0)
        {
            return $"var taskResult = Task.FromResult(Result.Failure({errorMessage}));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>({errorMessage}));";
    }

    protected string GenerateTaskErrorTypeFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var taskResult = Task.FromResult(Result.Failure(new Error(\"Test error\")));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(new Error(\"Test error\")));";
    }

    protected string GenerateValueTaskFailureResultCreation(ushort arity, string errorMessage = "\"Test error\"")
    {
        if (arity == 0)
        {
            return $"var valueTaskResult = ValueTask.FromResult(Result.Failure({errorMessage}));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>({errorMessage}));";
    }

    protected string GenerateValueTaskErrorTypeFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Failure(new Error(\"Test error\")));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(new Error(\"Test error\")));";
    }

    protected string GenerateExceptionalFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = Result.Failure(new ExceptionalError(new InvalidOperationException(\"Test exception\")));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(new ExceptionalError(new InvalidOperationException(\"Test exception\")));";
    }

    protected string GenerateTaskExceptionalFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var taskResult = Task.FromResult(Result.Failure(new ExceptionalError(new InvalidOperationException(\"Test exception\"))));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(new ExceptionalError(new InvalidOperationException(\"Test exception\"))));";
    }

    protected string GenerateValueTaskExceptionalFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Failure(new ExceptionalError(new InvalidOperationException(\"Test exception\"))));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(new ExceptionalError(new InvalidOperationException(\"Test exception\"))));";
    }

    #endregion

    #region Type Parameter Generation

    protected string GenerateTypeParams(ushort arity)
    {
        if (arity == 0) return string.Empty;
        return string.Join(", ", Enumerable.Range(1, arity)
                                           .Select(GetTestType));
    }

    protected string GenerateTypeParamsWithPrefix(ushort arity, string prefix)
    {
        var types = Enumerable.Range(1, arity)
                              .Select(GetTestType)
                              .ToList();
        types.Insert(0, prefix);
        return string.Join(", ", types);
    }

    #endregion

    #region Common Usings

    protected virtual IEnumerable<string> GetUsings()
    {
        return [
            "System",
            "System.Linq",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Reasons",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks"
        ];
    }

    #endregion
}
