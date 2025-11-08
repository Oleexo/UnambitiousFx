using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Base class for Result test generators that provides common functionality for generating test values,
///     result creation, and test method structures across sync, Task, and ValueTask variants.
/// </summary>
internal abstract class ResultTestGeneratorBase : BaseCodeGenerator
{
    protected ResultTestGeneratorBase(GenerationConfig config)
        : base(config)
    {
    }

    #region Variant Generation Helper

    /// <summary>
    ///     Generates test classes for provided variant factories, assigning UnderClass automatically when requested.
    ///     This removes duplication of variant collection logic across concrete generators.
    /// </summary>
    /// <param name="arity">The arity being generated.</param>
    /// <param name="baseClassName">Base class name used for grouping (UnderClass).</param>
    /// <param name="variants">
    ///     Tuple of factory function and flag indicating whether resulting class should be nested
    ///     (UnderClass).
    /// </param>
    /// <returns>Collection of generated class writers.</returns>
    protected IReadOnlyCollection<ClassWriter> GenerateVariants(ushort arity,
                                                                string baseClassName,
                                                                params (Func<ushort, ClassWriter?> factory, bool underBaseClass)[] variants)
    {
        var list = new List<ClassWriter>();
        foreach (var (factory, under) in variants)
        {
            var writer = factory(arity);
            if (writer == null)
            {
                continue;
            }

            if (under && string.IsNullOrEmpty(writer.UnderClass))
            {
                writer.UnderClass = baseClassName;
            }

            list.Add(writer);
        }

        return list;
    }

    #endregion

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
            6 => "DateTime.UtcNow",
            7 => "Guid.NewGuid()",
            8 => "TimeSpan.FromMinutes(5)",
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
            6 => "DateTime",
            7 => "Guid",
            8 => "TimeSpan",
            _ => "string"
        };
    }

    public string GetOtherValue(int index)
    {
        return index switch
        {
            1 => "100",
            2 => "\"World\"",
            3 => "false",
            4 => "6.28",
            5 => "456L",
            6 => "DateTime.UtcNow.AddDays(1)",
            7 => "Guid.NewGuid()",
            8 => "TimeSpan.FromMinutes(10)",
            _ => $"{index * 20}"
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

    protected string GenerateAsyncSuccessResultCreation(ushort arity,
                                                        string asyncType,
                                                        string resultVarName = "taskResult")
    {
        if (arity == 0)
        {
            return $"var {resultVarName} = {asyncType}.FromResult(Result.Success());";
        }

        if (arity == 1)
        {
            return $"var {resultVarName} = {asyncType}.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var {resultVarName} = {asyncType}.FromResult(Result.Success({values}));";
    }

    protected string GenerateFailureResultCreation(ushort arity,
                                                   string errorMessage = "\"Test error\"")
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

    protected string GenerateAsyncFailureResultCreation(ushort arity,
                                                        string asyncType,
                                                        string resultVarName = "taskResult",
                                                        string errorMessage = "\"Test error\"")
    {
        if (arity == 0)
        {
            return $"var {resultVarName} = {asyncType}.FromResult(Result.Failure({errorMessage}));";
        }

        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var {resultVarName} = {asyncType}.FromResult(Result.Failure<{typeParams}>({errorMessage}));";
    }

    #endregion

    #region Type Parameter Generation

    protected string GenerateTypeParams(ushort arity)
    {
        if (arity == 0)
        {
            return string.Empty;
        }

        return string.Join(", ", Enumerable.Range(1, arity)
                                           .Select(GetTestType));
    }

    protected string GenerateTypeParamsWithPrefix(ushort arity,
                                                  string prefix)
    {
        var types = Enumerable.Range(1, arity)
                              .Select(GetTestType)
                              .ToList();
        types.Insert(0, prefix);
        return string.Join(", ", types);
    }

    protected string GenerateValueParams(ushort arity,
                                         string? prefix = null)
    {
        List<string> valueParams;

        if (prefix is null)
        {
            valueParams = Enumerable.Range(1, arity)
                                    .Select(_ => "_")
                                    .ToList();
        }
        else
        {
            valueParams = Enumerable.Range(1, arity)
                                    .Select(x => $"value{x}")
                                    .ToList();
        }

        return string.Join(", ", valueParams);
    }

    #endregion

    #region Common Usings

    protected virtual IEnumerable<string> GetAdditionalUsings()
    {
        return [];
    }

    protected IEnumerable<string> GetUsings()
    {
        return GetAdditionalUsings()
           .Concat([
                "System",
                "System.Linq",
                "System.Threading.Tasks",
                "Xunit",
                "UnambitiousFx.Core",
                "UnambitiousFx.Core.Results",
                "UnambitiousFx.Core.Results.Reasons"
            ]);
    }

    #endregion

    #region Body Construction Helpers

    /// <summary>
    ///     Builds a section (Given/When/Then) with its lines.
    /// </summary>
    protected string BuildSection(string title,
                                  IEnumerable<string> lines)
    {
        var contentLines = lines.Where(l => !string.IsNullOrWhiteSpace(l))
                                .ToList();
        if (!contentLines.Any())
        {
            return string.Empty; // Allow caller to skip empty sections
        }

        return string.Join('\n', new[] { $"// {title}" }.Concat(contentLines));
    }

    /// <summary>
    ///     Composes a full test body with Given/When/Then sections preserving ordering and comments.
    /// </summary>
    protected string BuildTestBody(IEnumerable<string> givenLines,
                                   IEnumerable<string> whenLines,
                                   IEnumerable<string> thenLines)
    {
        var sections = new List<string>();
        var given = BuildSection("Given", givenLines);
        if (!string.IsNullOrEmpty(given))
        {
            sections.Add(given);
        }

        var when = BuildSection("When", whenLines);
        if (!string.IsNullOrEmpty(when))
        {
            sections.Add(when);
        }

        var then = BuildSection("Then", thenLines);
        if (!string.IsNullOrEmpty(then))
        {
            sections.Add(then);
        }

        return string.Join('\n', sections);
    }

    #endregion
}
