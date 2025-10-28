using UnambitiousFx.Core.CodeGen;

const string targetSourceDirectory = "output/src";
const string targetTestDirectory   = "output/test";
const int    targetArity           = 8;

var sourceDirectoryPath = Path.GetFullPath(targetSourceDirectory);
var testDirectoryPath   = Path.GetFullPath(targetTestDirectory);
if (!Directory.Exists(sourceDirectoryPath)) Directory.CreateDirectory(sourceDirectoryPath);
if (!Directory.Exists(testDirectoryPath)) Directory.CreateDirectory(testDirectoryPath);

var oneOfGenerator = new OneOfCodeGenerator(Constant.BaseNamespace);
oneOfGenerator.Generate(targetArity, sourceDirectoryPath);

var oneOfTestsGenerator = new OneOfTestsGenerator(Constant.BaseNamespace);
oneOfTestsGenerator.Generate(targetArity, testDirectoryPath);

var resultGenerator = new ResultCodeGenerator(Constant.BaseNamespace);
resultGenerator.Generate(targetArity, sourceDirectoryPath);

Console.WriteLine("Done!");