using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
/// Generator for OneOf types using Template Method pattern.
/// </summary>
internal sealed class OneOfCodeGenerator : BaseCodeGenerator
{
    public OneOfCodeGenerator(string baseNamespace)
        : base(new GenerationConfig(
            baseNamespace,
            startArity: 2,
            directoryName: "OneOf",
            className: "OneOf"))
    {
    }

    protected override void GenerateForArityRange(ushort numberOfArity, string outputPath)
    {
        for (ushort arity = (ushort)Config.StartArity; arity <= numberOfArity; arity++)
        {
            GenerateOneOfBase(arity, outputPath);
            GenerateOneOfImplementations(arity, outputPath);
        }
    }

    private void GenerateOneOfBase(ushort arity, string outputPath)
    {
        var fileWriter = new FileWriter($"{Config.BaseNamespace}.{Config.ClassName}");
        var classWriter = OneOfBaseClassBuilder.Build(arity, Config.BaseNamespace, Config.ClassName);
        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{Config.ClassName}{arity}.cs");
        FileSystemHelper.WriteFile(fileWriter, fileName);
    }

    private void GenerateOneOfImplementations(ushort arity, string outputPath)
    {
        for (ushort position = 1; position <= arity; position++)
        {
            var fileWriter = new FileWriter($"{Config.BaseNamespace}.{Config.ClassName}");
            var classWriter = OneOfImplementationBuilder.Build(arity, position);
            fileWriter.AddClass(classWriter);

            var ordinalName = OrdinalHelper.GetOrdinalName(position);
            var fileName = Path.Combine(outputPath, $"{Config.ClassName}{arity}.{ordinalName}.cs");
            FileSystemHelper.WriteFile(fileWriter, fileName);
        }
    }
}

/// <summary>
/// Builds the base OneOf class for a given arity.
/// </summary>
internal static class OneOfBaseClassBuilder
{
    public static ClassWriter Build(ushort arity, string baseNamespace, string className)
    {
        var genericParams = GenericTypeHelper.CreateOrdinalGenericParameters(arity, "T", "notnull");

        var classDocBuilder = DocumentationWriter.Create()
            .WithSummary($"Minimal discriminated union base abstraction representing a value that can be one of {arity} types.\n" +
                        "Specific semantic unions (e.g. Either<TLeft,TRight>) can inherit from this to express intent\n" +
                        "without duplicating core shape.");

        for (int i = 0; i < genericParams.Length; i++)
        {
            classDocBuilder.WithTypeParameter(
                genericParams[i].Name,
                $"{OrdinalHelper.GetOrdinalName(i + 1)} possible contained type."
            );
        }

        var classWriter = new ClassWriter(
            name: className,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Abstract,
            genericParameters: genericParams,
            documentation: classDocBuilder.Build()
        );

        AddIsProperties(classWriter, arity);
        AddMatchMethods(classWriter, arity);
        AddExtractionMethods(classWriter, arity);
        AddFactoryMethods(classWriter, arity, genericParams, className);
        AddImplicitOperators(classWriter, arity, genericParams, className);

        return classWriter;
    }

    private static void AddIsProperties(ClassWriter classWriter, ushort arity)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var propertyDoc = DocumentationWriter.Create()
                .WithSummary($"True when the instance currently holds a value of the {ordinalName.ToLower()} type.")
                .Build();

            classWriter.AddProperty(new PropertyWriter(
                name: $"Is{ordinalName}",
                type: "bool",
                visibility: Visibility.Public,
                hasGetter: true,
                hasSetter: false,
                style: PropertyStyle.Abstract,
                documentation: propertyDoc
            ));
        }
    }

    private static void AddMatchMethods(ClassWriter classWriter, ushort arity)
    {
        // Match with return value
        var matchFuncParams = Enumerable.Range(1, arity)
            .Select(i =>
            {
                var ordinal = OrdinalHelper.GetOrdinalName(i);
                return new MethodParameter($"Func<T{ordinal}, TOut>", $"{ordinal.ToLower()}Func");
            })
            .ToArray();

        var matchFuncDocBuilder = DocumentationWriter.Create()
            .WithSummary("Pattern match returning a value.")
            .WithTypeParameter("TOut", "The type of value to return");

        foreach (var param in matchFuncParams)
        {
            var ordinalName = char.ToUpper(param.Name[0]) + param.Name.Substring(1, param.Name.Length - 5); // Remove "Func"
            matchFuncDocBuilder.WithParameter(param.Name, $"Function to invoke when value is of type T{ordinalName}");
        }
        matchFuncDocBuilder.WithReturns("The result of invoking the appropriate function");

        classWriter.AddMethod(new AbstractMethodWriter(
            name: "Match",
            returnType: "TOut",
            visibility: Visibility.Public,
            parameters: matchFuncParams,
            genericParameters: [new GenericParameter("TOut", "")],
            documentation: matchFuncDocBuilder.Build()
        ));

        // Match with void
        var matchActionParams = Enumerable.Range(1, arity)
            .Select(i =>
            {
                var ordinal = OrdinalHelper.GetOrdinalName(i);
                return new MethodParameter($"Action<T{ordinal}>", $"{ordinal.ToLower()}Action");
            })
            .ToArray();

        var matchActionDocBuilder = DocumentationWriter.Create()
            .WithSummary("Pattern match executing side-effect actions.");

        foreach (var param in matchActionParams)
        {
            var ordinalName = char.ToUpper(param.Name[0]) + param.Name.Substring(1, param.Name.Length - 7); // Remove "Action"
            matchActionDocBuilder.WithParameter(param.Name, $"Action to invoke when value is of type T{ordinalName}");
        }

        classWriter.AddMethod(new AbstractMethodWriter(
            name: "Match",
            returnType: "void",
            visibility: Visibility.Public,
            parameters: matchActionParams,
            documentation: matchActionDocBuilder.Build()
        ));
    }

    private static void AddExtractionMethods(ClassWriter classWriter, ushort arity)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var extractDoc = DocumentationWriter.Create()
                .WithSummary($"Attempts to extract the {ordinalName.ToLower()} value.")
                .WithParameter(ordinalName.ToLower(), $"The {ordinalName.ToLower()} value if present")
                .WithReturns($"True if the value is of type T{ordinalName}, false otherwise")
                .Build();

            classWriter.AddMethod(new AbstractMethodWriter(
                name: ordinalName,
                returnType: "bool",
                visibility: Visibility.Public,
                parameters: [new MethodParameter($"[NotNullWhen(true)] out T{ordinalName}?", ordinalName.ToLower())],
                documentation: extractDoc,
                usings: ["System.Diagnostics.CodeAnalysis"]
            ));
        }
    }

    private static void AddFactoryMethods(ClassWriter classWriter, ushort arity, GenericParameter[] genericParams, string className)
    {
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));

        for (ushort i = 1; i <= arity; i++)
        {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var factoryDoc = DocumentationWriter.Create()
                .WithSummary($"Creates a {className} instance containing a {ordinalName.ToLower()} value.")
                .WithParameter("value", $"The {ordinalName.ToLower()} value")
                .WithReturns($"A {className} instance containing the {ordinalName.ToLower()} value")
                .Build();

            classWriter.AddMethod(new MethodWriter(
                name: $"From{ordinalName}",
                returnType: $"{className}<{allTypeParams}>",
                body: $"return new {ordinalName}OneOf<{allTypeParams}>(value);",
                visibility: Visibility.Public,
                modifier: MethodModifier.Static,
                parameters: [new MethodParameter($"T{ordinalName}", "value")],
                documentation: factoryDoc
            ));
        }
    }

    private static void AddImplicitOperators(ClassWriter classWriter, ushort arity, GenericParameter[] genericParams, string className)
    {
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));

        for (ushort i = 1; i <= arity; i++)
        {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var operatorDoc = DocumentationWriter.Create()
                .WithSummary($"Implicit conversion from {ordinalName.ToLower()} type to {className}.")
                .WithParameter("value", $"The {ordinalName.ToLower()} value")
                .WithReturns($"A {className} instance containing the {ordinalName.ToLower()} value")
                .Build();

            classWriter.AddMethod(new MethodWriter(
                name: $"implicit operator {className}<{allTypeParams}>",
                returnType: "",
                body: $"return From{ordinalName}(value);",
                visibility: Visibility.Public,
                modifier: MethodModifier.Static,
                parameters: [new MethodParameter($"T{ordinalName}", "value")],
                documentation: operatorDoc
            ));
        }
    }
}

/// <summary>
/// Builds a concrete OneOf implementation for a specific position.
/// </summary>
internal static class OneOfImplementationBuilder
{
    public static ClassWriter Build(ushort arity, ushort position)
    {
        var ordinalName = OrdinalHelper.GetOrdinalName(position);
        var genericParams = GenericTypeHelper.CreateOrdinalGenericParameters(arity, "T", "notnull");
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var className = $"{ordinalName}OneOf";
        var baseClassName = $"OneOf<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: className,
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName)
        );

        AddField(classWriter, ordinalName);
        AddConstructor(classWriter, className, ordinalName);
        AddIsProperties(classWriter, arity, position);
        AddMatchMethods(classWriter, arity, position, ordinalName);
        AddExtractionMethods(classWriter, arity, position, ordinalName);

        return classWriter;
    }

    private static void AddField(ClassWriter classWriter, string ordinalName)
    {
        classWriter.AddField(new FieldWriter(
            name: $"_{ordinalName.ToLower()}",
            type: $"T{ordinalName}",
            visibility: Visibility.Private,
            isReadonly: true
        ));
    }

    private static void AddConstructor(ClassWriter classWriter, string className, string ordinalName)
    {
        classWriter.AddConstructor(new ConstructorWriter(
            className: className,
            body: $"_{ordinalName.ToLower()} = {ordinalName.ToLower()};",
            visibility: Visibility.Public,
            parameters: [new MethodParameter($"T{ordinalName}", ordinalName.ToLower())]
        ));
    }

    private static void AddIsProperties(ClassWriter classWriter, ushort arity, ushort position)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var currentOrdinal = OrdinalHelper.GetOrdinalName(i);
            classWriter.AddProperty(new PropertyWriter(
                name: $"Is{currentOrdinal}",
                type: "bool",
                visibility: Visibility.Public,
                getterBody: i == position ? "true" : "false",
                style: PropertyStyle.Override
            ));
        }
    }

    private static void AddMatchMethods(ClassWriter classWriter, ushort arity, ushort position, string ordinalName)
    {
        // Match with return
        var matchFuncParams = Enumerable.Range(1, arity)
            .Select(i =>
            {
                var ordinal = OrdinalHelper.GetOrdinalName(i);
                return new MethodParameter($"Func<T{ordinal}, TOut>", $"{ordinal.ToLower()}Func");
            })
            .ToArray();

        classWriter.AddMethod(new MethodWriter(
            name: "Match",
            returnType: "TOut",
            body: $"return {ordinalName.ToLower()}Func(_{ordinalName.ToLower()});",
            visibility: Visibility.Public,
            modifier: MethodModifier.Override,
            parameters: matchFuncParams,
            genericParameters: [new GenericParameter("TOut", "")]
        ));

        // Match with void
        var matchActionParams = Enumerable.Range(1, arity)
            .Select(i =>
            {
                var ordinal = OrdinalHelper.GetOrdinalName(i);
                return new MethodParameter($"Action<T{ordinal}>", $"{ordinal.ToLower()}Action");
            })
            .ToArray();

        classWriter.AddMethod(new MethodWriter(
            name: "Match",
            returnType: "void",
            body: $"{ordinalName.ToLower()}Action(_{ordinalName.ToLower()});",
            visibility: Visibility.Public,
            modifier: MethodModifier.Override,
            parameters: matchActionParams
        ));
    }

    private static void AddExtractionMethods(ClassWriter classWriter, ushort arity, ushort position, string ordinalName)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var currentOrdinal = OrdinalHelper.GetOrdinalName(i);
            string body = i == position
                ? $"{currentOrdinal.ToLower()} = _{ordinalName.ToLower()};\nreturn true;"
                : $"{currentOrdinal.ToLower()} = default;\nreturn false;";

            classWriter.AddMethod(new MethodWriter(
                name: currentOrdinal,
                returnType: "bool",
                body: body,
                visibility: Visibility.Public,
                modifier: MethodModifier.Override,
                parameters: [new MethodParameter($"[NotNullWhen(true)] out T{currentOrdinal}?", currentOrdinal.ToLower())],
                usings: ["System.Diagnostics.CodeAnalysis"]
            ));
        }
    }
}
