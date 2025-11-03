using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Configuration;

/// <summary>
///     Base class for code generators implementing the Template Method pattern.
///     Provides common structure and validation for all generators.
/// </summary>
internal abstract class BaseCodeGenerator : ICodeGenerator {
    protected readonly GenerationConfig Config;

    protected BaseCodeGenerator(GenerationConfig config) {
        Config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    ///     Generates code for the specified arity range.
    ///     Implements template method pattern with validation and directory setup.
    /// </summary>
    public void Generate(ushort numberOfArity,
                         string outputPath) {
        ValidateInputs(numberOfArity, outputPath);
        var allClasses = new List<ClassWriter>();
        for (var arity = (ushort)Config.StartArity; arity <= numberOfArity; arity++) {
            var classes = GenerateForArity(arity);
            foreach (var @class in classes) {
                if (@class.Namespace is null) {
                    @class.Namespace = $"{Config.BaseNamespace}.{Config.SubNamespace}";
                }

                if (@class.Region is null) {
                    @class.Region = $"Arity {arity}";
                }
            }

            if (classes.Count == 0) {
                continue;
            }

            if (Config.FileOrganization == FileOrganizationMode.SingleFile) {
                allClasses.AddRange(classes);
            }
            else {
                WriteSeparatedFiles(classes, outputPath, arity);
            }
        }

        if (Config.FileOrganization == FileOrganizationMode.SingleFile) {
            WriteSingleFile(allClasses, outputPath);
        }
    }

    private void WriteSingleFile(IEnumerable<ClassWriter> classes,
                                 string                   targetPath) {
        foreach (var g in classes.GroupBy(x => x.Namespace ?? $"{Config.BaseNamespace}.{x.Name}")) {
            var ns   = g.Key;
            var file = new FileWriter(ns);
            file.AddClass(ClassWriter.Merge(g.ToArray()));
            var filePath = GetPathFromNamespace(targetPath, ns, $"{Config.ClassName}.g.cs");
            FileSystemHelper.WriteFile(file, filePath);
        }
    }

    private void WriteSeparatedFiles(IEnumerable<ClassWriter> classes,
                                     string                   targetPath,
                                     ushort                   arity) {
        foreach (var g in classes.GroupBy(x => GetFileName(arity, x))) {
            var fileName = g.Key;
            var ns = g.FirstOrDefault()
                     ?.Namespace ??
                     $"{Config.BaseNamespace}.{Config.ClassName}";
            var file = new FileWriter(ns);
            foreach (var @class in g) {
                file.AddClass(@class);
            }

            var filePath = GetPathFromNamespace(targetPath, ns, fileName);
            FileSystemHelper.WriteFile(file, filePath);
        }
    }

    private string GetPathFromNamespace(string targetPath,
                                        string @namespace,
                                        string filename) {
        var sub = @namespace[(Config.BaseNamespace.Length + 1)..]
           .Replace('.', Path.DirectorySeparatorChar);
        return Path.Combine(targetPath, sub, filename);
    }

    private string GetFileName(ushort      arity,
                               ClassWriter @class) {
        if (@class.UnderClass is not null) {
            var className = @class.Name;

            if (className.StartsWith(Config.ClassName)) {
                className = className[Config.ClassName.Length..];
            }
            else if (className.EndsWith(Config.ClassName)) {
                className = className[..^Config.ClassName.Length];
            }

            return Config.IsTest
                       ? $"{@class.UnderClass}.{className}.g.cs"
                       : $"{@class.UnderClass}.{arity}.{className}.g.cs";
        }

        return Config.IsTest
                   ? $"{@class.Name}.g.cs"
                   : $"{@class.Name}.{arity}.g.cs";
    }

    /// <summary>
    ///     Validates generation inputs.
    /// </summary>
    protected virtual void ValidateInputs(ushort numberOfArity,
                                          string outputPath) {
        if (numberOfArity < Config.StartArity) {
            throw new ArgumentOutOfRangeException(nameof(numberOfArity),
                                                  $"Arity must be >= {Config.StartArity}.");
        }

        if (string.IsNullOrWhiteSpace(outputPath)) {
            throw new ArgumentException("Output path cannot be null or whitespace.", nameof(outputPath));
        }
    }

    /// <summary>
    ///     Prepares the output directory structure.
    /// </summary>
    protected virtual string PrepareOutputDirectory(string outputPath) {
        if (string.IsNullOrEmpty(Config.SubNamespace)) {
            return outputPath;
        }

        return FileSystemHelper.CreateSubdirectory(outputPath, Config.SubNamespace);
    }

    /// <summary>
    ///     Generates code for all arities in the range.
    ///     Derived classes must implement this method.
    /// </summary>
    protected abstract IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity);
}
