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
        var tapExtensionsFactory = new ResultTapExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var mapErrorExtensionsFactory = new ResultMapErrorExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var tapErrorExtensionsFactory = new ResultTapErrorExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var ensureExtensionsFactory = new ResultEnsureExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var tryExtensionsFactory = new ResultTryExtensions(DefaultNamespace, MaxOfParameters);
        var matchExtensionsFactory = new ResultMatchExtensionsFactory(DefaultNamespace, MaxOfParameters);
        var taskExtensionsFactory = new ResultTaskExtensionsFactory(DefaultNamespace, MaxOfParameters);
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
            var tapExtensionsSource = tapExtensionsFactory.Generate();
            ctx.AddSource("ResultTapExtensions.g.cs", tapExtensionsSource);
            var tapTaskExtensionsSource = tapExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultTapExtensions.Task.g.cs", tapTaskExtensionsSource);
            var tapValueTaskExtensionsSource = tapExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultTapExtensions.ValueTask.g.cs", tapValueTaskExtensionsSource);
            var mapErrorExtensionsSource = mapErrorExtensionsFactory.Generate();
            ctx.AddSource("ResultMapErrorExtensions.g.cs", mapErrorExtensionsSource);
            var mapErrorTaskExtensionsSource = mapErrorExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultMapErrorExtensions.Task.g.cs", mapErrorTaskExtensionsSource);
            var mapErrorValueTaskExtensionsSource = mapErrorExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultMapErrorExtensions.ValueTask.g.cs", mapErrorValueTaskExtensionsSource);
            var tapErrorExtensionsSource = tapErrorExtensionsFactory.Generate();
            ctx.AddSource("ResultTapErrorExtensions.g.cs", tapErrorExtensionsSource);
            var tapErrorTaskExtensionsSource = tapErrorExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultTapErrorExtensions.Task.g.cs", tapErrorTaskExtensionsSource);
            var tapErrorValueTaskExtensionsSource = tapErrorExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultTapErrorExtensions.ValueTask.g.cs", tapErrorValueTaskExtensionsSource);
            var ensureExtensionsSource = ensureExtensionsFactory.Generate();
            ctx.AddSource("ResultEnsureExtensions.g.cs", ensureExtensionsSource);
            var ensureTaskExtensionsSource = ensureExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultEnsureExtensions.Task.g.cs", ensureTaskExtensionsSource);
            var ensureValueTaskExtensionsSource = ensureExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultEnsureExtensions.ValueTask.g.cs", ensureValueTaskExtensionsSource);
            var tryExtensionsSource = tryExtensionsFactory.Generate();
            ctx.AddSource("ResultTryExtensions.g.cs", tryExtensionsSource);
            var tryTaskExtensionsSource = tryExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultTryExtensions.Task.g.cs", tryTaskExtensionsSource);
            var tryValueTaskExtensionsSource = tryExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultTryExtensions.ValueTask.g.cs", tryValueTaskExtensionsSource);
            var matchTaskExtensionsSource = matchExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultMatchExtensions.Task.g.cs", matchTaskExtensionsSource);
            var matchValueTaskExtensionsSource = matchExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultMatchExtensions.ValueTask.g.cs", matchValueTaskExtensionsSource);
            var fromToTaskExtensionsTaskSource = taskExtensionsFactory.GenerateTask();
            ctx.AddSource("ResultTaskExtensions.Task.g.cs", fromToTaskExtensionsTaskSource);
            var fromToTaskExtensionsValueTaskSource = taskExtensionsFactory.GenerateValueTask();
            ctx.AddSource("ResultTaskExtensions.ValueTask.g.cs", fromToTaskExtensionsValueTaskSource);
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
