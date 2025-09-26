using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultTryExtensions(string @namespace,
                                          ushort maxOfParameters) {
    public SourceText GenerateTask() => GenerateAsync("Task", "Tasks");
    public SourceText GenerateValueTask() => GenerateAsync("ValueTask", "ValueTasks");

    private SourceText GenerateAsync(string taskKeyWork, string subNamespace) {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace}.{subNamespace};");

        tw.WriteLine("public static partial class ResultExtensions");
        tw.WriteLine("{");
        tw.Indent++;

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            if (i == 1) {
                // TryAsync for Result<T>
                tw.WriteLine($"public static {taskKeyWork}<Result<TOut>> TryAsync<TValue, TOut>(this Result<TValue> result, Func<TValue, {taskKeyWork}<TOut>> func)");
                tw.Indent++;
                tw.WriteLine("where TValue : notnull");
                tw.WriteLine("where TOut : notnull");
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return result.BindAsync(async value =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("var newValue = await func(value);");
                tw.WriteLine("return Result.Success(newValue);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine("catch (Exception ex)");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return Result.Failure<TOut>(ex);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.Indent--;
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();

                // TryAsync for awaitable Result<T>
                tw.WriteLine($"public static {taskKeyWork}<Result<TOut>> TryAsync<TValue, TOut>(this {taskKeyWork}<Result<TValue>> awaitableResult, Func<TValue, {taskKeyWork}<TOut>> func)");
                tw.Indent++;
                tw.WriteLine("where TValue : notnull");
                tw.WriteLine("where TOut : notnull");
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return awaitableResult.BindAsync(async value =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("var newValue = await func(value);");
                tw.WriteLine("return Result.Success(newValue);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine("catch (Exception ex)");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return Result.Failure<TOut>(ex);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.Indent--;
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
            }
            else {
                var genericInputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
                var genericOutputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TOut{x}"));
                var callParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"value{x}"));
                var callTupleParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"items.Item{x}"));

                tw.WriteLine($"public static {taskKeyWork}<Result<{genericOutputParameters}>> TryAsync<{genericInputParameters}, {genericOutputParameters}>(this Result<{genericInputParameters}> result, Func<{genericInputParameters}, {taskKeyWork}<({genericOutputParameters})>> func)");
                tw.Indent++;
                foreach (var gp in genericInputParameters.Split(", ".ToCharArray()).Concat(genericOutputParameters.Split(", ".ToCharArray())).Where(x => !string.IsNullOrWhiteSpace(x))) {
                    tw.WriteLine($"where {gp} : notnull");
                }
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"return result.BindAsync(async ({callParameters}) =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"var items = await func({callParameters});");
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
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();

                tw.WriteLine($"public static {taskKeyWork}<Result<{genericOutputParameters}>> TryAsync<{genericInputParameters}, {genericOutputParameters}>(this {taskKeyWork}<Result<{genericInputParameters}>> awaitableResult, Func<{genericInputParameters}, {taskKeyWork}<({genericOutputParameters})>> func)");
                tw.Indent++;
                foreach (var gp in genericInputParameters.Split(", ".ToCharArray()).Concat(genericOutputParameters.Split(", ".ToCharArray())).Where(x => !string.IsNullOrWhiteSpace(x))) {
                    tw.WriteLine($"where {gp} : notnull");
                }
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"return awaitableResult.BindAsync(async ({callParameters}) =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"var items = await func({callParameters});");
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
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
            }
        }

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }

    public SourceText Generate() {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);

        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace};");

        tw.WriteLine("public static partial class ResultExtensions");
        tw.WriteLine("{");
        tw.Indent++;

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            if (i == 1) {
                tw.WriteLine("public static Result<TOut> Try<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> func)");
                tw.Indent++;
                tw.WriteLine("where TValue : notnull");
                tw.WriteLine("where TOut : notnull");
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return result.Bind(value =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("var newValue = func(value);");
                tw.WriteLine("return Result.Success(newValue);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine("catch (Exception ex)");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("return Result.Failure<TOut>(ex);");
                tw.Indent--;
                tw.WriteLine("}");
                tw.Indent--;
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();
            }
            else {
                var genericInputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
                var genericOutputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TOut{x}"));
                var callParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"value{x}"));
                var callTupleParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"items.Item{x}"));

                tw.WriteLine($"public static Result<{genericOutputParameters}> Try<{genericInputParameters}, {genericOutputParameters}>(this Result<{genericInputParameters}> result, Func<{genericInputParameters}, ({genericOutputParameters})> func)");
                tw.Indent++;
                foreach (var gp in genericInputParameters.Split(", ".ToCharArray()).Concat(genericOutputParameters.Split(", ".ToCharArray())).Where(x => !string.IsNullOrWhiteSpace(x))) {
                    tw.WriteLine($"where {gp} : notnull");
                }
                tw.Indent--;
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"return result.Bind(({callParameters}) =>");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine("try");
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"var items = func({callParameters});");
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
                tw.WriteLine("});");
                tw.Indent--;
                tw.WriteLine("}");
                tw.WriteLine();
            }
        }

        tw.Indent--;
        tw.WriteLine("}");

        return SourceText.From(sw.ToString(), Encoding.UTF8);
    }
}
