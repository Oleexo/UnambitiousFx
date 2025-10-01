using Microsoft.CodeAnalysis;

namespace UnambitiousFx.Core.XUnit.Generator;

[Generator]
public sealed class ResultPredicateGenerator : IIncrementalGenerator {
    private const int MaxArity = 8;
    private const string TargetNamespace = "UnambitiousFx.Core.XUnit";

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var factory = new ResultPredicateExtensionsFactory(TargetNamespace);
        // Non generic part
        context.RegisterPostInitializationOutput(ctx => {
            ctx.AddSource("ResultPredicateExtensions.Base.g.cs", factory.GenerateNonGeneric());
        });
        // Per arity parts
        foreach (var arity in Enumerable.Range(1, MaxArity)) {
            var captured = arity; // capture loop variable
            context.RegisterPostInitializationOutput(ctx => {
                ctx.AddSource($"ResultPredicateExtensions`{captured}.g.cs", factory.Generate(captured));
            });
        }
    }
}
