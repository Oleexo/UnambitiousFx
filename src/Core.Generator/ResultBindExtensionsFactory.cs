using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Core.Generator;

internal sealed class ResultBindExtensionsFactory(string @namespace,
                                                  ushort maxOfParameters) {
    public SourceText GenerateTask() {
        return Generate("Task", "Tasks",
            (input, genericParameters) => $"Task.FromResult<Result<{genericParameters}>>({input})");
    }
    
    public SourceText GenerateValueTask() {
        return Generate("ValueTask", "ValueTasks",
                        (input, genericParameters) => $"new ValueTask<Result<{genericParameters}>>({input})");
    }
    private SourceText Generate(string taskKeyWork, 
                                string subNamespace,
                                Func<string, string, string> newFromResultFunc) {
        using var sw = new StringWriter();
        using var tw = new IndentedTextWriter(sw);
        
        tw.WriteLine("#nullable enable");
        tw.WriteLine($"namespace {@namespace}.{subNamespace};");
        
        tw.WriteLine("public static partial class ResultExtensions");
        tw.WriteLine("{");
        tw.Indent++;

        foreach (ushort i in Enumerable.Range(1, maxOfParameters)) {
            foreach (ushort j in Enumerable.Range(1, maxOfParameters)) {
                var genericInputParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"TValue{x}"));
                var genericOutputParameters = string.Join(", ", Enumerable.Range(1, j).Select(x => $"TOut{x}"));
                var callParameters = string.Join(", ", Enumerable.Range(1, i).Select(x => $"value{x}"));
                tw.WriteLine($"public static {taskKeyWork}<Result<{genericOutputParameters}>> Bind<{genericInputParameters}, {genericOutputParameters}>(this Result<{genericInputParameters}> result, Func<{genericInputParameters}, {taskKeyWork}<Result<{genericOutputParameters}>>> bind)");
                tw.Indent++;
                foreach (var genericInputParameter in genericInputParameters.Split(", ".ToCharArray()).Concat(genericOutputParameters.Split(", ".ToCharArray())).Where(x => !string.IsNullOrWhiteSpace(x))) {
                    tw.WriteLine($"where {genericInputParameter} : notnull");
                }

                tw.Indent--;
                
                tw.WriteLine("{");
                tw.Indent++;
                tw.WriteLine($"return result.Match<{taskKeyWork}<Result<{genericOutputParameters}>>>(async ({callParameters}) => await bind({callParameters}), e => {newFromResultFunc($"Result.Failure<{genericOutputParameters}>(e)", genericOutputParameters)});");
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
