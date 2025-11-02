using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Generators;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
///     Coordinates the overall code generation process.
///     Implements Facade pattern to simplify the generation workflow.
/// </summary>
internal sealed class CodeGenerationOrchestrator {
    private readonly string               _baseNamespace;
    private readonly FileOrganizationMode _fileOrganization;
    private readonly string               _sourceDirectory;
    private readonly ushort               _targetArity;
    private readonly string               _testDirectory;

    public CodeGenerationOrchestrator(string               baseNamespace,
                                      string               sourceDirectory,
                                      string               testDirectory,
                                      ushort               targetArity,
                                      FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles) {
        _baseNamespace    = baseNamespace   ?? throw new ArgumentNullException(nameof(baseNamespace));
        _sourceDirectory  = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
        _testDirectory    = testDirectory   ?? throw new ArgumentNullException(nameof(testDirectory));
        _targetArity      = targetArity;
        _fileOrganization = fileOrganization;
    }

    /// <summary>
    ///     Executes the complete code generation process.
    /// </summary>
    public void Execute() {
        Console.WriteLine("Starting code generation...");
        Console.WriteLine($"Base Namespace: {_baseNamespace}");
        Console.WriteLine($"Source Directory: {_sourceDirectory}");
        Console.WriteLine($"Test Directory: {_testDirectory}");
        Console.WriteLine($"Target Arity: {_targetArity}");
        Console.WriteLine($"File Organization: {_fileOrganization}");
        Console.WriteLine();

        PreparePaths(out var sourceDirectoryPath, out var testDirectoryPath);
        EnsureDirectories(sourceDirectoryPath, testDirectoryPath);

        GenerateOneOf(sourceDirectoryPath, testDirectoryPath);
        GenerateResults(sourceDirectoryPath, testDirectoryPath);

        Console.WriteLine();
        Console.WriteLine("Code generation completed successfully!");
    }

    private void PreparePaths(out string sourceDirectoryPath,
                              out string testDirectoryPath) {
        sourceDirectoryPath = Path.GetFullPath(_sourceDirectory);
        testDirectoryPath   = Path.GetFullPath(_testDirectory);

        Console.WriteLine($"Resolved source path: {sourceDirectoryPath}");
        Console.WriteLine($"Resolved test path: {testDirectoryPath}");
    }

    private void EnsureDirectories(string sourceDirectoryPath,
                                   string testDirectoryPath) {
        FileSystemHelper.EnsureDirectoryExists(sourceDirectoryPath);
        FileSystemHelper.EnsureDirectoryExists(testDirectoryPath);
    }

    private void GenerateOneOf(string sourceDirectoryPath,
                               string testDirectoryPath) {
        Console.WriteLine("Generating OneOf types...");

        foreach (var generator in CodeGeneratorFactory.CreateOneOfGenerators(_baseNamespace, _fileOrganization)) {
            var outputPath = generator is OneOfTestsGenerator
                                 ? testDirectoryPath
                                 : sourceDirectoryPath;
            generator.Generate(_targetArity, outputPath);
        }

        Console.WriteLine("OneOf types generated.");
    }

    private void GenerateResults(string sourceDirectoryPath,
                                 string testDirectoryPath) {
        Console.WriteLine("Generating Result types...");

        foreach (var generator in CodeGeneratorFactory.CreateResultGenerators(_baseNamespace, _fileOrganization)) {
            var outputPath = sourceDirectoryPath;
            generator.Generate(_targetArity, outputPath);
        }

        Console.WriteLine("Result types generated.");
    }
}
