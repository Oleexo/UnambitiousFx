using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultTaskExtensionsFactory(string @namespace,
                                                  ushort maxOfParameters)
{
    public SourceText GenerateTask() => GenerateAsync("Task", "Tasks",
        (input, gp) => $"Task.FromResult<Result<{gp}>>({input})",
        static input => $"Task.FromResult<Result>({input})");

    public SourceText GenerateValueTask() => GenerateAsync("ValueTask", "ValueTasks",
        (input, gp) => $"new ValueTask<Result<{gp}>>({input})",
        static input => $"new ValueTask<Result>({input})");

    private SourceText GenerateAsync(
        string taskKeyWord,
        string subNamespace,
        Func<string, string, string> newFromResultGeneric,
        Func<string, string> newFromResultNonGeneric)
    {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace}.{subNamespace};");

        tw.WriteLine("public static partial class ResultExtensions");
        tw.WriteLine("{");
        tw.Indent++;

        // FromTask for arity 0 (Task -> Task<Result>)
        tw.WriteLine($"public static async {taskKeyWord}<Result> FromTask({taskKeyWord} task)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("try");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("await task;");
        tw.WriteLine("return Result.Success();");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine("catch (Exception ex)");
        tw.WriteLine("{");
        tw.Indent++;
        tw.WriteLine("return Result.Failure(ex);");
        tw.Indent--;
        tw.WriteLine("}");
        tw.Indent--;
        tw.WriteLine("}");
        tw.WriteLine();

        // ToTask for arity 0 (Result -> Task<Result>)
        tw.WriteLine($"public static {taskKeyWord}<Result> ToTask(this Result result) => {newFromResultNonGeneric("result")};");
        tw.WriteLine();

        foreach (ushort i in Enumerable.Range(1, maxOfParameters))
        {
            // build generic parameter lists
            var genericParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
            var callTupleParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"items.Item{x}"));

            // FromTask for arity i: Task<(T1..Ti)> -> Task<Result<T1..Ti>>
            if (i == 1)
            {
                tw.WriteLine($"public static async {taskKeyWord}<Result<TValue>> FromTask<TValue>({taskKeyWord}<TValue> task)");
                tw.Indent++;
                tw.WriteLine("where TValue : notnull");
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("var value = await task;");
                tw.WriteLine("return Result.Success(value);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine("catch (Exception ex)");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return Result.Failure<TValue>(ex);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();

                // ToTask for arity 1: Result<T> -> Task<Result<T>>
                tw.WriteLine($"public static {taskKeyWord}<Result<TValue>> ToTask<TValue>(this Result<TValue> result)");
                tw.Indent++;
                tw.WriteLine("where TValue : notnull");
                tw.Indent--;
                tw.WriteLine("=> " + newFromResultGeneric("result", "TValue") + ";");
                tw.WriteLine();
            }
            else
            {
                var genericOutputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
                tw.WriteLine($"public static async {taskKeyWord}<Result<{genericOutputParameters}>> FromTask<{genericParameters}>({taskKeyWord}<({genericOutputParameters})> task)");
                tw.Indent++;
                foreach (var gp in genericParameters.Split(", ".ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    tw.WriteLine($"where {gp} : notnull");
                }
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("var items = await task;");
                tw.WriteLine($"return Result.Success({callTupleParameters});");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine("catch (Exception ex)");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"return Result.Failure<{genericOutputParameters}>(ex);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();

                // ToTask for arity i: Result<T1..Ti> -> Task<Result<T1..Ti>>
                tw.WriteLine($"public static {taskKeyWord}<Result<{genericOutputParameters}>> ToTask<{genericParameters}>(this Result<{genericOutputParameters}> result)");
                tw.Indent++;
                foreach (var gp in genericParameters.Split(", ".ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    tw.WriteLine($"where {gp} : notnull");
                }
                tw.Indent--;
                tw.WriteLine("=> " + newFromResultGeneric("result", genericOutputParameters) + ";");
                tw.WriteLine();
            }
        }

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }
}
