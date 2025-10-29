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

    public void Generate(ushort numberOfArity, string outputPath)
    {
        if (numberOfArity < StartArity)
            throw new ArgumentOutOfRangeException(nameof(numberOfArity), $"Arity must be >= {StartArity}.");
        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Output path cannot be null or whitespace.", nameof(outputPath));

        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var tasksOutputPath = Path.Combine(outputPath, "Tasks");
        var valueTasksOutputPath = Path.Combine(outputPath, "ValueTasks");
        if (!Directory.Exists(tasksOutputPath))
        {
            Directory.CreateDirectory(tasksOutputPath);
        }
        if (!Directory.Exists(valueTasksOutputPath))
        {
            Directory.CreateDirectory(valueTasksOutputPath);
        }

        // Generate individual partial files for ToNullable, ValueOr, ValueOrThrow methods
        GenerateToNullablePartialFiles(numberOfArity, outputPath);
        GenerateValueOrPartialFiles(numberOfArity, outputPath);
        GenerateValueOrThrowPartialFiles(numberOfArity, outputPath);

        for (ushort i = StartArity; i <= numberOfArity; i++)
        {
            GenerateValueAccessExtensions(i, outputPath);
            GenerateTaskValueAccessExtensions(i, tasksOutputPath);
            GenerateValueTaskValueAccessExtensions(i, valueTasksOutputPath);
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

    private void GenerateToNullablePartialFiles(ushort numberOfArity, string outputPath)
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
        var methodWriter = new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams
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


    private void GenerateValueOrPartialFiles(ushort numberOfArity, string outputPath)
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

    private void GenerateValueOrThrowPartialFiles(ushort numberOfArity, string outputPath)
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
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: parameters.ToArray(),
            genericParameters: genericParams
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
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result"), new MethodParameter($"Func<{factoryReturn}>", "fallbackFactory")],
            genericParameters: genericParams,
            usings: ["System"]
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
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result")],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"]
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
        return new MethodWriter(
            name: methodName,
            returnType: returnType,
            body: bodyBuilder.ToString(),
            visibility: Visibility.Public,
            modifier: MethodModifier.Static,
            parameters: [new MethodParameter($"this {resultType}", "result"), new MethodParameter("Func<IEnumerable<IError>, Exception>", "exceptionFactory")],
            genericParameters: genericParams,
            usings: ["System", $"{_baseNamespace}.Results.Reasons"]
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
