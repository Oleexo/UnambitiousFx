using System;
using System.IO;

namespace UnambitiousFx.Core.CodeGen;

internal static class Program
{
    private const string TargetSourceDirectory = "src/Core";
    private const string TargetTestDirectory = "test/Core.Tests";
    private const int TargetArity = 8;

    public static void Main(string[] args)
    {
        var (sourceDirectoryPath, testDirectoryPath) = ResolvePaths(TargetSourceDirectory, TargetTestDirectory);
        EnsureDirectories(sourceDirectoryPath, testDirectoryPath);

        GenerateOneOf(sourceDirectoryPath, testDirectoryPath, TargetArity);
        GenerateResults(sourceDirectoryPath, testDirectoryPath, TargetArity);

        Console.WriteLine("Done!");
    }

    private static (string SourcePath, string TestPath) ResolvePaths(string sourceDir, string testDir)
    {
        var sourcePath = Path.GetFullPath(sourceDir);
        var testPath = Path.GetFullPath(testDir);
        return (sourcePath, testPath);
    }

    private static void EnsureDirectories(string sourceDirectoryPath, string testDirectoryPath)
    {
        if (!Directory.Exists(sourceDirectoryPath))
            Directory.CreateDirectory(sourceDirectoryPath);
        if (!Directory.Exists(testDirectoryPath))
            Directory.CreateDirectory(testDirectoryPath);
    }

    private static void GenerateOneOf(string sourceDirectoryPath, string testDirectoryPath, int arity)
    {
        var oneOfGenerator = new OneOfCodeGenerator(Constant.BaseNamespace);
        oneOfGenerator.Generate((ushort)arity, sourceDirectoryPath);

        var oneOfTestsGenerator = new OneOfTestsGenerator(Constant.BaseNamespace);
        oneOfTestsGenerator.Generate((ushort)arity, testDirectoryPath);
    }

    private static void GenerateResults(string sourceDirectoryPath, string testDirectoryPath, int arity)
    {
        var resultGenerator = new ResultCodeGenerator(Constant.BaseNamespace);
        resultGenerator.Generate((ushort)arity, sourceDirectoryPath);

        var resultTestsGenerator = new ResultTestGenerator(Constant.BaseNamespace);
        resultTestsGenerator.Generate((ushort)arity, testDirectoryPath);
    }
}