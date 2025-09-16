using Microsoft.CodeAnalysis;

namespace UnambitiousFx.Core.Generator;

[Generator]
public class ResultGenerator : IIncrementalGenerator {
    private const string DefaultNamespace = "UnambitiousFx.Core.Results";
    private const ushort MaxOfParameters  = 8;

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var resultFactory = new ResultClassFactory(DefaultNamespace, MaxOfParameters);
        context.RegisterPostInitializationOutput(ctx => {
            var source = resultFactory.Generate();
            ctx.AddSource("Result.g.cs", source);
        });
        foreach (var i in Enumerable.Range(1, MaxOfParameters)) {
            context.RegisterPostInitializationOutput(ctx => {
                var source = resultFactory.Generate((ushort)i);
                ctx.AddSource($"Result`{i}.g.cs", source);
            });
        }
    }
}
