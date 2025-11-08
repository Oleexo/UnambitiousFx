using UnambitiousFx.Core.CodeGen.Configuration;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
///     Entry point using improved architecture with Facade and Factory patterns.
///     Demonstrates clean separation of concerns and improved maintainability.
/// </summary>
internal static class Program
{
    private const string TargetSourceDirectory = "src/Core";
    private const string TargetTestDirectory = "test/Core.Tests";
    private const int TargetArity = 8;

    public static void Main(string[] args)
    {
        var orchestrator = new CodeGenerationOrchestrator(
            Constant.BaseNamespace,
            TargetSourceDirectory,
            TargetTestDirectory,
            TargetArity
        );

        orchestrator.Execute();
    }
}
