using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

internal sealed class ResultValueAccessExtensionsCodeGenerator : ICodeGenerator
{
    private readonly string _baseNamespace;
    private const int StartArity = 1;
    private const string DirectoryName = "Results/Extensions/ValueAccess";
    private const string ClassName = "ResultValueAccessExtensions";

    public ResultValueAccessExtensionsCodeGenerator(string baseNamespace)
    {
        if (string.IsNullOrWhiteSpace(baseNamespace))
            throw new ArgumentException("Base namespace cannot be null or whitespace.", nameof(baseNamespace));
        _baseNamespace = baseNamespace;
    }

    // Duplicate Generate method removed

    /// <summary>
    /// Generates extension methods for accessing values from Result types with varying arity.
    /// </summary>
    /// <param name="numberOfArity">Maximum arity to generate for.</param>
    /// <param name="outputPath">Root output directory for generated files.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="numberOfArity"/> is less than <c>StartArity</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="outputPath"/> is null or whitespace.</exception>
    public void Generate(ushort numberOfArity, string outputPath)
    {
        ValidateGenerateArguments(numberOfArity, outputPath);
        var (mainOutput, tasksOutput, valueTasksOutput) = Results.Extensions.ValueAccess.DirectorySetup.Prepare(outputPath, DirectoryName);
        Results.Extensions.ValueAccess.PartialGenerators.GenerateAllPartials(this, numberOfArity, mainOutput, tasksOutput, valueTasksOutput);
        GenerateAritySpecificFiles(numberOfArity, mainOutput, tasksOutput, valueTasksOutput);
    }

    /// <summary>
    /// Validates arguments for the Generate method.
    /// </summary>
    private static void ValidateGenerateArguments(ushort numberOfArity, string outputPath)
    {
        if (numberOfArity < StartArity)
            throw new ArgumentOutOfRangeException(nameof(numberOfArity), $"Arity must be >= {StartArity}.");
        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Output path cannot be null or whitespace.", nameof(outputPath));
    }

    // Directory setup and partial generation logic moved to Results/Extensions/ValueAccess helpers

    /// <summary>
    /// Generates arity-specific extension files for each arity.
    /// </summary>
    private void GenerateAritySpecificFiles(ushort numberOfArity, string mainOutput, string tasksOutput, string valueTasksOutput)
    {
        for (ushort i = StartArity; i <= numberOfArity; i++)
        {
            GenerateValueAccessExtensions(i, mainOutput);
            GenerateTaskValueAccessExtensions(i, tasksOutput);
            GenerateValueTaskValueAccessExtensions(i, valueTasksOutput);
        }
    }
    // ...existing code...

    private void GenerateValueAccessExtensions(ushort arity, string outputPath)
    {
        var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
        var classWriter = BuildValueAccessExtensionClass(arity);
        fileWriter.AddClass(classWriter);
        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.cs");
        WriteFile(fileWriter, fileName);
    }

    internal void GenerateToNullablePartialFiles(ushort numberOfArity, string outputPath)
    {
        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var classWriter = new ClassWriter(
                name: "ResultToNullableExtensions",
                visibility: Visibility.Public,
                classModifiers: ClassModifier.Static | ClassModifier.Partial
            );

            // Build method for this arity.
            classWriter.AddMethod(BuildStandaloneToNullableMethod(arity));
            fileWriter.AddClass(classWriter);

            var fileName = Path.Combine(outputPath, $"ResultToNullableExtensions.Arity{arity}.cs");
            WriteFile(fileWriter, fileName);
        }
    }

    private MethodWriter BuildStandaloneToNullableMethod(ushort arity)
    {
        // Arity-specific generic type list
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));

        // Determine return type tuple or single
        string returnType;
        if (arity == 1)
        {
            returnType = "TValue1?";
        }
        else
        {
            // Build tuple type (TValue1, TValue2, ...)?
            var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
            returnType = $"({tupleInner})?";
        }

        // Build method signature result type
        var resultType = $"Result<{genericTypes}>";

        // Build body replicating existing logic
        var bodyBuilder = new System.Text.StringBuilder();
        if (arity == 1)
        {
            bodyBuilder.AppendLine("return result.TryGet(out var value)");
            bodyBuilder.AppendLine("           ? (TValue1?)value");
            bodyBuilder.AppendLine("           : default;");
        }
        else
        {
            bodyBuilder.AppendLine("if (!result.IsSuccess) {");
            bodyBuilder.AppendLine("    return null;");
            bodyBuilder.AppendLine("}");
            bodyBuilder.AppendLine();
            // out variables
            var outVars = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
            bodyBuilder.AppendLine($"return result.TryGet(out var value1, out var value2{(arity >= 3 ? ", out var value3" : string.Empty)}");
            if (arity > 3)
            {
                for (int n = 4; n <= arity; n++)
                {
                    bodyBuilder.AppendLine($", out var value{n}");
                }
            }
            bodyBuilder.AppendLine($")\n           ? ({outVars})\n           : default;");
        }

        var methodName = "ToNullable";
        var docBuilder = DocumentationWriter.Create()
            .WithSummary(arity == 1 ? "Returns the value as nullable if success; otherwise default." : "Returns the tuple of values as nullable if success; otherwise default.")
            .WithParameter("result", "The result instance.")
            .WithReturns("The nullable value or null/default.");
        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }
        var doc = docBuilder.Build();
        var methodWriter = new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            documentation: doc
        );
        return methodWriter;
    }



    private void GenerateTaskValueAccessExtensions(ushort arity, string outputPath)
    {
        var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
        var classWriter = BuildTaskValueAccessExtensionClass(arity);
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.Task.cs");
        WriteFile(fileWriter, fileName);
    }

    private void GenerateValueTaskValueAccessExtensions(ushort arity, string outputPath)
    {
        var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
        var classWriter = BuildValueTaskValueAccessExtensionClass(arity);
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.ValueTask.cs");
        WriteFile(fileWriter, fileName);
    }

    private static void WriteFile(FileWriter fileWriter, string fileName)
    {
        using var stringWriter = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);
        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private ClassWriter BuildValueAccessExtensionClass(ushort arity)
    {
        var classWriter = new ClassWriter(
            name: $"{ClassName}{arity}",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static
        );

        for (int i = 1; i <= arity; i++)
        {
            classWriter.AddMethod(BuildValueAccessMethod(arity, i));
            // ToNullable moved to partial files.
            // ValueOr and ValueOrThrow moved to partial files.
        }

        return classWriter;
    }

    private MethodWriter BuildValueAccessMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"GetValue{valueIndex}";
        var returnType = $"T{valueIndex}?";

        var body = $$"""
            return result.IsSuccess ? result.Value{{valueIndex}} : default;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")]
        );
    }

    private MethodWriter BuildToNullableMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ToNullable{valueIndex}";
        var returnType = $"T{valueIndex}?";

        var body = $$"""
            return result.IsSuccess ? result.Value{{valueIndex}} : default;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")]
        );
    }


    internal void GenerateValueOrPartialFiles(ushort numberOfArity, string outputPath)
    {
        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var classWriter = new ClassWriter(
                name: "ResultValueOrExtensions",
                visibility: Visibility.Public,
                classModifiers: ClassModifier.Static | ClassModifier.Partial
            );

            classWriter.AddMethod(BuildStandaloneValueOrDefaultMethod(arity));
            classWriter.AddMethod(BuildStandaloneValueOrFactoryMethod(arity));
            fileWriter.AddClass(classWriter);

            var fileName = Path.Combine(outputPath, $"ResultValueOrExtensions.Arity{arity}.cs");
            WriteFile(fileWriter, fileName);
        }
    }

    internal void GenerateValueOrThrowPartialFiles(ushort numberOfArity, string outputPath)
    {
        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            var fileWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var classWriter = new ClassWriter(
                name: "ResultValueOrThrowExtensions",
                visibility: Visibility.Public,
                classModifiers: ClassModifier.Static | ClassModifier.Partial
            );

            classWriter.AddMethod(BuildStandaloneValueOrThrowDefaultMethod(arity));
            classWriter.AddMethod(BuildStandaloneValueOrThrowFactoryMethod(arity));
            fileWriter.AddClass(classWriter);

            var fileName = Path.Combine(outputPath, $"ResultValueOrThrowExtensions.Arity{arity}.cs");
            WriteFile(fileWriter, fileName);
        }
    }

    private MethodWriter BuildStandaloneValueOrDefaultMethod(ushort arity)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var fallbackParameters = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n} fallback{n}"));
        var fallbackArgs = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"fallback{n}"));
        var methodName = "ValueOr";
        var bodyBuilder = new System.Text.StringBuilder();
        if (arity == 1)
        {
            bodyBuilder.AppendLine("return result.Match<TValue1>(value1 => value1, _ => fallback1);");
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
            bodyBuilder.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => ({fallbackArgs}));");
        }
        var parameters = new List<MethodParameter> { new MethodParameter($"this {resultType}", "result") };
        parameters.AddRange(Enumerable.Range(1, arity).Select(n => new MethodParameter($"TValue{n}", $"fallback{n}")));
        var docValueOrDefaultBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained values when successful; otherwise provided fallback(s).")
            .WithParameter("result", "The result instance.")
            .WithReturns("The value(s) or fallback(s).");
        foreach (var i in Enumerable.Range(1, arity))
        {
            docValueOrDefaultBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
            docValueOrDefaultBuilder.WithParameter($"fallback{i}", $"Fallback value {i}.");
        }
        var doc = docValueOrDefaultBuilder.Build();
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            documentation: doc
        );
    }

    private MethodWriter BuildStandaloneValueOrFactoryMethod(ushort arity)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var methodName = "ValueOr";
        var factoryReturn = arity == 1 ? "TValue1" : $"({tupleInner})";
        var bodyBuilder = new System.Text.StringBuilder();
        if (arity == 1)
        {
            bodyBuilder.AppendLine("return result.Match<TValue1>(value1 => value1, _ => fallbackFactory());");
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
            bodyBuilder.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => fallbackFactory());");
        }
        var docFactoryBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained values when successful; otherwise value(s) from factory.")
            .WithParameter("result", "The result instance.")
            .WithParameter("fallbackFactory", "Factory producing fallback value(s).")
            .WithReturns("The value(s) or factory value(s).");
        foreach (var i in Enumerable.Range(1, arity))
        {
            docFactoryBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }
        var docFactory = docFactoryBuilder.Build();
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result"), new MethodParameter($"Func<{factoryReturn}>", "fallbackFactory")],
            genericParameters: genericParams,
            usings: ["System"],
            documentation: docFactory
        );
    }

    private MethodWriter BuildStandaloneValueOrThrowDefaultMethod(ushort arity)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var methodName = "ValueOrThrow";
        var bodyBuilder = new System.Text.StringBuilder();
        if (arity == 1)
        {
            bodyBuilder.AppendLine("return result.ValueOrThrow(errors => throw errors.ToException());");
        }
        else
        {
            bodyBuilder.AppendLine("return result.ValueOrThrow(errors => throw errors.ToException());");
        }
        var docThrowDefaultBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained value(s); throws aggregated exception when failure.")
            .WithParameter("result", "The result instance.")
            .WithReturns("The value(s) or throws.");
        foreach (var i in Enumerable.Range(1, arity))
        {
            docThrowDefaultBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }
        var docThrowDefault = docThrowDefaultBuilder.Build();
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"],
            documentation: docThrowDefault
        );
    }

    private MethodWriter BuildStandaloneValueOrThrowFactoryMethod(ushort arity)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var methodName = "ValueOrThrow";
        var bodyBuilder = new System.Text.StringBuilder();
        if (arity == 1)
        {
            bodyBuilder.AppendLine("return result.Match<TValue1>(value1 => value1, e => throw exceptionFactory(e));");
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
            bodyBuilder.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), e => throw exceptionFactory(e));");
        }
        var docThrowFactoryBuilder = DocumentationWriter.Create()
            .WithSummary("Returns contained value(s); otherwise throws exception from factory.")
            .WithParameter("result", "The result instance.")
            .WithParameter("exceptionFactory", "Factory creating exception from errors.")
            .WithReturns("The value(s) or throws custom exception.");
        foreach (var i in Enumerable.Range(1, arity))
        {
            docThrowFactoryBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }
        var docThrowFactory = docThrowFactoryBuilder.Build();
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result"), new MethodParameter("Func<IEnumerable<IError>, Exception>", "exceptionFactory")],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"],
            documentation: docThrowFactory
        );
    }

    // Async partial generation for ValueOr / ValueOrThrow
    internal void GenerateValueOrAsyncPartialFiles(ushort numberOfArity, string tasksPath, string valueTasksPath)
    {
        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            // Task partial
            var taskWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var taskClass = new ClassWriter("ResultValueOrExtensions", Visibility.Public, ClassModifier.Static | ClassModifier.Partial);
            taskClass.AddMethod(BuildStandaloneValueOrDefaultAsyncMethod(arity, isValueTask: false));
            taskClass.AddMethod(BuildStandaloneValueOrFactoryAsyncMethod(arity, isValueTask: false));
            taskWriter.AddClass(taskClass);
            WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultValueOrExtensions.Arity{arity}.Task.cs"));

            // ValueTask partial
            var vtWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var vtClass = new ClassWriter("ResultValueOrExtensions", Visibility.Public, ClassModifier.Static | ClassModifier.Partial);
            vtClass.AddMethod(BuildStandaloneValueOrDefaultAsyncMethod(arity, isValueTask: true));
            vtClass.AddMethod(BuildStandaloneValueOrFactoryAsyncMethod(arity, isValueTask: true));
            vtWriter.AddClass(vtClass);
            WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultValueOrExtensions.Arity{arity}.ValueTask.cs"));
        }
    }

    internal void GenerateValueOrThrowAsyncPartialFiles(ushort numberOfArity, string tasksPath, string valueTasksPath)
    {
        for (ushort arity = StartArity; arity <= numberOfArity; arity++)
        {
            var taskWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var taskClass = new ClassWriter("ResultValueOrThrowExtensions", Visibility.Public, ClassModifier.Static | ClassModifier.Partial);
            taskClass.AddMethod(BuildStandaloneValueOrThrowDefaultAsyncMethod(arity, isValueTask: false));
            taskClass.AddMethod(BuildStandaloneValueOrThrowFactoryAsyncMethod(arity, isValueTask: false));
            taskWriter.AddClass(taskClass);
            WriteFile(taskWriter, Path.Combine(tasksPath, $"ResultValueOrThrowExtensions.Arity{arity}.Task.cs"));

            var vtWriter = new FileWriter($"{_baseNamespace}.Results.Extensions.ValueAccess");
            var vtClass = new ClassWriter("ResultValueOrThrowExtensions", Visibility.Public, ClassModifier.Static | ClassModifier.Partial);
            vtClass.AddMethod(BuildStandaloneValueOrThrowDefaultAsyncMethod(arity, isValueTask: true));
            vtClass.AddMethod(BuildStandaloneValueOrThrowFactoryAsyncMethod(arity, isValueTask: true));
            vtWriter.AddClass(vtClass);
            WriteFile(vtWriter, Path.Combine(valueTasksPath, $"ResultValueOrThrowExtensions.Arity{arity}.ValueTask.cs"));
        }
    }

    // Async builders
    private MethodWriter BuildStandaloneValueOrDefaultAsyncMethod(ushort arity, bool isValueTask)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var taskResultType = $"Task<{resultType}>";
        var vtResultType = $"ValueTask<{resultType}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var parameters = new List<MethodParameter> { new MethodParameter($"this {(isValueTask ? vtResultType : taskResultType)}", "resultTask") };
        parameters.AddRange(Enumerable.Range(1, arity).Select(n => new MethodParameter($"TValue{n}", $"fallback{n}")));
        var body = new System.Text.StringBuilder();
        body.AppendLine("var result = await resultTask.ConfigureAwait(false);");
        if (arity == 1)
        {
            body.AppendLine("return result.Match<TValue1>(value1 => value1, _ => fallback1);");
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
            var fallbackArgs = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"fallback{n}"));
            body.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => ({fallbackArgs}));");
        }
        var asyncReturn = (isValueTask ? $"ValueTask<{returnType}>" : $"Task<{returnType}>");
        return new MethodWriter(
            name: "ValueOr",
            returnType: asyncReturn,
            body: body.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: parameters.ToArray(),
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: DocumentationWriter.Create().WithSummary("Async ValueOr returning fallback(s) when failure.").Build()
        );
    }

    private MethodWriter BuildStandaloneValueOrFactoryAsyncMethod(ushort arity, bool isValueTask)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var taskResultType = $"Task<{resultType}>";
        var vtResultType = $"ValueTask<{resultType}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
        var factoryReturn = arity == 1 ? "TValue1" : $"({tupleInner})";
        var body = new System.Text.StringBuilder();
        body.AppendLine("var result = await resultTask.ConfigureAwait(false);");
        body.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), _ => fallbackFactory());");
        var asyncReturn = (isValueTask ? $"ValueTask<{returnType}>" : $"Task<{returnType}>");
        return new MethodWriter(
            name: "ValueOr",
            returnType: asyncReturn,
            body: body.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this {(isValueTask ? vtResultType : taskResultType)}", "resultTask"), new MethodParameter($"Func<{factoryReturn}>", "fallbackFactory")],
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: DocumentationWriter.Create().WithSummary("Async ValueOr using fallback factory when failure.").Build()
        );
    }

    private MethodWriter BuildStandaloneValueOrThrowDefaultAsyncMethod(ushort arity, bool isValueTask)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var taskResultType = $"Task<{resultType}>";
        var vtResultType = $"ValueTask<{resultType}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var body = new System.Text.StringBuilder();
        body.AppendLine("var result = await resultTask.ConfigureAwait(false);");
        body.AppendLine("return result.ValueOrThrow(errors => throw errors.ToException());");
        var asyncReturn = (isValueTask ? $"ValueTask<{returnType}>" : $"Task<{returnType}>");
        return new MethodWriter(
            name: "ValueOrThrow",
            returnType: asyncReturn,
            body: body.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this {(isValueTask ? vtResultType : taskResultType)}", "resultTask")],
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks", $"{_baseNamespace}.Results.Reasons"],
            documentation: DocumentationWriter.Create().WithSummary("Async ValueOrThrow throwing aggregated exception when failure.").Build()
        );
    }

    private MethodWriter BuildStandaloneValueOrThrowFactoryAsyncMethod(ushort arity, bool isValueTask)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"TValue{n}", "notnull"));
        var tupleInner = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"TValue{n}"));
        var resultType = $"Result<{genericTypes}>";
        var taskResultType = $"Task<{resultType}>";
        var vtResultType = $"ValueTask<{resultType}>";
        var returnType = arity == 1 ? "TValue1" : $"({tupleInner})";
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"value{n}"));
        var body = new System.Text.StringBuilder();
        body.AppendLine("var result = await resultTask.ConfigureAwait(false);");
        body.AppendLine($"return result.Match<{returnType}>(({valueParams}) => ({valueParams}), e => throw exceptionFactory(e));");
        var asyncReturn = (isValueTask ? $"ValueTask<{returnType}>" : $"Task<{returnType}>");
        return new MethodWriter(
            name: "ValueOrThrow",
            returnType: asyncReturn,
            body: body.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this {(isValueTask ? vtResultType : taskResultType)}", "resultTask"), new MethodParameter("Func<IEnumerable<IError>, Exception>", "exceptionFactory")],
            genericParameters: genericParams,
            usings: ["System", "System.Threading.Tasks", $"{_baseNamespace}.Results.Reasons"],
            documentation: DocumentationWriter.Create().WithSummary("Async ValueOrThrow using exception factory when failure.").Build()
        );
    }

    private ClassWriter BuildTaskValueAccessExtensionClass(ushort arity)
    {
        var classWriter = new ClassWriter(
            name: $"{ClassName}{arity}Task",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static
        );

        for (int i = 1; i <= arity; i++)
        {
            classWriter.AddMethod(BuildTaskToNullableMethod(arity, i));
            classWriter.AddMethod(BuildTaskValueOrWithDefaultMethod(arity, i));
            classWriter.AddMethod(BuildTaskValueOrWithFactoryMethod(arity, i));
            classWriter.AddMethod(BuildTaskValueOrThrowMethod(arity, i));
            classWriter.AddMethod(BuildTaskValueOrThrowWithFactoryMethod(arity, i));
        }

        return classWriter;
    }

    private ClassWriter BuildValueTaskValueAccessExtensionClass(ushort arity)
    {
        var classWriter = new ClassWriter(
            name: $"{ClassName}{arity}ValueTask",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Static
        );

        for (int i = 1; i <= arity; i++)
        {
            classWriter.AddMethod(BuildValueTaskToNullableMethod(arity, i));
            classWriter.AddMethod(BuildValueTaskValueOrWithDefaultMethod(arity, i));
            classWriter.AddMethod(BuildValueTaskValueOrWithFactoryMethod(arity, i));
            classWriter.AddMethod(BuildValueTaskValueOrThrowMethod(arity, i));
            classWriter.AddMethod(BuildValueTaskValueOrThrowWithFactoryMethod(arity, i));
        }

        return classWriter;
    }

    // Task<Result<...>> extension methods
    private MethodWriter BuildTaskToNullableMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ToNullable{valueIndex}";
        var returnType = $"T{valueIndex}?";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : default;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"Task<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this Task<{resultType}>", "resultTask")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildTaskValueOrWithDefaultMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOr{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : defaultValue;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"Task<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this Task<{resultType}>", "resultTask"), new MethodParameter(returnType, "defaultValue")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildTaskValueOrWithFactoryMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOr{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : factory();
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"Task<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this Task<{resultType}>", "resultTask"), new MethodParameter($"Func<{returnType}>", "factory")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildTaskValueOrThrowMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOrThrow{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : throw new InvalidOperationException("Result is not in success state.");
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"Task<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this Task<{resultType}>", "resultTask")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildTaskValueOrThrowWithFactoryMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOrThrow{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : throw exceptionFactory();
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"Task<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this Task<{resultType}>", "resultTask"), new MethodParameter("Func<Exception>", "exceptionFactory")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    // ValueTask<Result<...>> extension methods
    private MethodWriter BuildValueTaskToNullableMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ToNullable{valueIndex}";
        var returnType = $"T{valueIndex}?";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : default;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"ValueTask<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this ValueTask<{resultType}>", "resultTask")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildValueTaskValueOrWithDefaultMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOr{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : defaultValue;
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"ValueTask<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this ValueTask<{resultType}>", "resultTask"), new MethodParameter(returnType, "defaultValue")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildValueTaskValueOrWithFactoryMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOr{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : factory();
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"ValueTask<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this ValueTask<{resultType}>", "resultTask"), new MethodParameter($"Func<{returnType}>", "factory")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildValueTaskValueOrThrowMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOrThrow{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : throw new InvalidOperationException("Result is not in success state.");
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"ValueTask<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this ValueTask<{resultType}>", "resultTask")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }

    private MethodWriter BuildValueTaskValueOrThrowWithFactoryMethod(ushort arity, int valueIndex)
    {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(n => $"T{n}"));
        var genericParams = Enumerable.Range(1, arity).Select(n => new GenericParameter($"T{n}", ""));
        var resultType = $"Result<{genericTypes}>";
        var methodName = $"ValueOrThrow{valueIndex}";
        var returnType = $"T{valueIndex}";

        var body = $$"""
            var result = await resultTask.ConfigureAwait(false);
            return result.IsSuccess ? result.Value{{valueIndex}} : throw exceptionFactory();
            """;

        return new MethodWriter(
            name: methodName,
            returnType: $"ValueTask<{returnType}>",
            body: body,
            visibility: Visibility.Public,
            modifier: MethodModifier.Async,
            parameters: [new MethodParameter($"this ValueTask<{resultType}>", "resultTask"), new MethodParameter("Func<Exception>", "exceptionFactory")],
            genericParameters: genericParams,
            attributes: [new AttributeReference("MethodImpl", "MethodImplOptions.AggressiveInlining", "System.Runtime.CompilerServices")],
            usings: ["System.Threading.Tasks"]
        );
    }
}
