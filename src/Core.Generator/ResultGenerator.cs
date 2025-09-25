using Microsoft.CodeAnalysis;

namespace UnambitiousFx.Core.Generator;

[Generator]
public class ResultGenerator : IIncrementalGenerator {
    private const string DefaultNamespace = "UnambitiousFx.Core.Results";
    private const ushort MaxOfParameters  = 8;

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var resultFactory = new ResultClassFactory(DefaultNamespace, MaxOfParameters);
        var arityClassFactory = new ResultArityClassFactory(DefaultNamespace, MaxOfParameters);
        var bindExtensionsFactory = new ResultBindExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var mapExtensionsFactory = new ResultMapExtensionsFactory(DefaultNamespace, MaxOfParameters);
        context.RegisterPostInitializationOutput(ctx => {
            var abstractClassSource = resultFactory.GenerateResult();
            ctx.AddSource("Result.g.cs", abstractClassSource);
            var bindExtensionsSource = bindExtensionsFactory.Generate();
            ctx.AddSource("ResultBindExtensions.g.cs", bindExtensionsSource);
            var taskBindExtensionsSource = bindExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultBindExtensions.Task.g.cs", taskBindExtensionsSource);
            var valueTaskBindExtensionsSource = bindExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultBindExtensions.ValueTask.g.cs", valueTaskBindExtensionsSource);
            var mapExtensionsSource = mapExtensionsFactory.Generate();
            ctx.AddSource("ResultMapExtensions.g.cs", mapExtensionsSource);
            var mapTaskExtensionsSource = mapExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultMapExtensions.Task.g.cs", mapTaskExtensionsSource);
            var mapValueTaskExtensionsSource = mapExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultMapExtensions.ValueTask.g.cs", mapValueTaskExtensionsSource);
        });
        foreach (ushort i in Enumerable.Range(1, MaxOfParameters)) {
            context.RegisterPostInitializationOutput(ctx => {
                var abstractClassSource = arityClassFactory.GenerateResult(i);
                ctx.AddSource($"Result`{i}.g.cs", abstractClassSource);
                var successClassSource = arityClassFactory.GenerateSuccessResult(i);
                ctx.AddSource($"SuccessResult`{i}.g.cs", successClassSource);
                var failureClassSource = arityClassFactory.GenerateFailureResult(i);
                ctx.AddSource($"FailureResult`{i}.g.cs", failureClassSource);
            });
        }
    }
}
