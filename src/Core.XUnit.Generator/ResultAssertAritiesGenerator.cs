using Microsoft.CodeAnalysis;

namespace UnambitiousFx.Core.XUnit.Generator;

[Generator]
public sealed class ResultAssertAritiesGenerator : IIncrementalGenerator
{
    private const int MinArity = 2;
    private const int MaxArity = 8;
    private const string TargetNamespace = "UnambitiousFx.Core.XUnit.Results";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var factory = new ResultAssertAritiesFactory(TargetNamespace);
        foreach (var arity in Enumerable.Range(MinArity, MaxArity - MinArity + 1))
        {
            var captured = arity; // capture loop variable
            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource($"ResultAssertExtensions.Arities`{captured}.g.cs", factory.Generate(captured));
            });
        }
    }
}
