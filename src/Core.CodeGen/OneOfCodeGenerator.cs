using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

internal class OneOfCodeGenerator : ICodeGenerator {
    private readonly string _baseNamespace;
    private const    int    StartArity    = 2;
    private const    string ClassName     = "OneOf";
    private const    string DirectoryName = ClassName;

    public OneOfCodeGenerator(string baseNamespace) {
        _baseNamespace = baseNamespace;
    }

    public void Generate(ushort numberOfArity,
                         string outputPath) {
        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }

        for (ushort i = StartArity; i <= numberOfArity; i++) {
            GenerateOneOfBase(i, outputPath);
            GenerateOneOfImplementations(i, outputPath);
        }
    }

    private void GenerateOneOfBase(ushort arity,
                                   string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.{ClassName}");
        var classWriter = CreateOneOfBaseClass(arity);

        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.cs");

        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);

        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private void GenerateOneOfImplementations(ushort arity,
                                              string outputPath) {
        for (ushort i = 1; i <= arity; i++) {
            var fileWriter  = new FileWriter($"{_baseNamespace}.{ClassName}");
            var classWriter = CreateOneOfImplementation(arity, i);

            fileWriter.AddClass(classWriter);

            var ordinalName = GetOrdinalName(i);

            var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.{ordinalName}.cs");

            using var stringWriter   = new StringWriter();
            using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
            fileWriter.Write(indentedWriter);

            File.WriteAllText(fileName, stringWriter.ToString());
        }
    }

    private ClassWriter CreateOneOfBaseClass(ushort arity) {
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"T{GetOrdinalName(i)}", "notnull"))
                                      .ToArray();

        // Build class documentation
        var classDocBuilder = DocumentationWriter.Create()
                                                 .WithSummary(
                                                      $"Minimal discriminated union base abstraction representing a value that can be one of {arity} types.\nSpecific semantic unions (e.g. Either<TLeft,TRight>) can inherit from this to express intent\nwithout duplicating core shape.");

        for (int i = 0; i < genericParams.Length; i++) {
            classDocBuilder.WithTypeParameter(
                genericParams[i].Name,
                $"{GetOrdinalName(i + 1)} possible contained type."
            );
        }

        var classWriter = new ClassWriter(
            name: ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Abstract,
            genericParameters: genericParams,
            documentation: classDocBuilder.Build()
        );

        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
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

        var matchFuncParams = Enumerable.Range(1, arity)
                                        .Select(i => new MethodParameter($"Func<T{GetOrdinalName(i)}, TOut>", $"{GetOrdinalName(i).ToLower()}Func"))
                                        .ToArray();

        var matchFuncDocBuilder = DocumentationWriter.Create()
                                                     .WithSummary("Pattern match returning a value.")
                                                     .WithTypeParameter("TOut", "The type of value to return");

        foreach (var param in matchFuncParams) {
            var ordinalName = param.Name.Replace("Func", "");
            matchFuncDocBuilder.WithParameter(param.Name, $"Function to invoke when value is of type T{char.ToUpper(ordinalName[0]) + ordinalName.Substring(1)}");
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

        // Add Match method (void)
        var matchActionParams = Enumerable.Range(1, arity)
                                          .Select(i => new MethodParameter($"Action<T{GetOrdinalName(i)}>", $"{GetOrdinalName(i).ToLower()}Action"))
                                          .ToArray();

        var matchActionDocBuilder = DocumentationWriter.Create()
                                                       .WithSummary("Pattern match executing side-effect actions.");

        foreach (var param in matchActionParams) {
            var ordinalName = param.Name.Replace("Action", "");
            matchActionDocBuilder.WithParameter(param.Name, $"Action to invoke when value is of type T{char.ToUpper(ordinalName[0]) + ordinalName.Substring(1)}");
        }

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  visibility: Visibility.Public,
                                  parameters: matchActionParams,
                                  documentation: matchActionDocBuilder.Build()
                              ));

        // Add extraction methods (First, Second, etc.)
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
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

        // Add factory methods
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName   = GetOrdinalName(i);
            var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
            var factoryDoc = DocumentationWriter.Create()
                                                .WithSummary($"Creates a OneOf instance containing a {ordinalName.ToLower()} value.")
                                                .WithParameter("value", $"The {ordinalName.ToLower()} value")
                                                .WithReturns($"A OneOf instance containing the {ordinalName.ToLower()} value")
                                                .Build();

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}",
                                      returnType: $"OneOf<{allTypeParams}>",
                                      body: $"return new {ordinalName}OneOf<{allTypeParams}>(value);",
                                      visibility: Visibility.Public,
                                      modifier: MethodModifier.Static,
                                      parameters: [new MethodParameter($"T{ordinalName}", "value")],
                                      documentation: factoryDoc
                                  ));
        }

        // Add implicit operators
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName   = GetOrdinalName(i);
            var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
            var operatorDoc = DocumentationWriter.Create()
                                                 .WithSummary($"Implicit conversion from {ordinalName.ToLower()} type to OneOf.")
                                                 .WithParameter("value", $"The {ordinalName.ToLower()} value")
                                                 .WithReturns($"A OneOf instance containing the {ordinalName.ToLower()} value")
                                                 .Build();

            classWriter.AddMethod(new MethodWriter(
                                      name: $"implicit operator OneOf<{allTypeParams}>",
                                      returnType: "",
                                      body: $"return From{ordinalName}(value);",
                                      visibility: Visibility.Public,
                                      modifier: MethodModifier.Static,
                                      parameters: [new MethodParameter($"T{ordinalName}", "value")],
                                      documentation: operatorDoc
                                  ));
        }

        return classWriter;
    }

    private ClassWriter CreateOneOfImplementation(ushort arity,
                                                  ushort position) {
        var ordinalName = GetOrdinalName(position);
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"T{GetOrdinalName(i)}", "notnull"))
                                      .ToArray();

        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var className     = $"{ordinalName}OneOf";
        var baseClassName = $"OneOf<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: className,
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName)
        );

        classWriter.AddField(new FieldWriter(
                                 name: $"_{ordinalName.ToLower()}",
                                 type: $"T{ordinalName}",
                                 visibility: Visibility.Private,
                                 isReadonly: true
                             ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       className: $"{ordinalName}OneOf",
                                       body: $"_{ordinalName.ToLower()} = {ordinalName.ToLower()};",
                                       visibility: Visibility.Public,
                                       parameters: [new MethodParameter($"T{ordinalName}", ordinalName.ToLower())]
                                   ));

        for (ushort i = 1; i <= arity; i++) {
            var currentOrdinal = GetOrdinalName(i);
            classWriter.AddProperty(new PropertyWriter(
                                        name: $"Is{currentOrdinal}",
                                        type: "bool",
                                        visibility: Visibility.Public,
                                        getterBody: i == position
                                                        ? "true"
                                                        : "false",
                                        style: PropertyStyle.Override
                                    ));
        }

        var matchFuncParams = Enumerable.Range(1, arity)
                                        .Select(i => new MethodParameter($"Func<T{GetOrdinalName(i)}, TOut>", $"{GetOrdinalName(i).ToLower()}Func"))
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

        var matchActionParams = Enumerable.Range(1, arity)
                                          .Select(i => new MethodParameter($"Action<T{GetOrdinalName(i)}>", $"{GetOrdinalName(i).ToLower()}Action"))
                                          .ToArray();

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: $"{ordinalName.ToLower()}Action(_{ordinalName.ToLower()});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: matchActionParams
                              ));

        // Add extraction methods
        for (ushort i = 1; i <= arity; i++) {
            var    currentOrdinal = GetOrdinalName(i);
            string body;

            if (i == position) {
                body = $"{currentOrdinal.ToLower()} = _{ordinalName.ToLower()};\nreturn true;";
            }
            else {
                body = $"{currentOrdinal.ToLower()} = default;\nreturn false;";
            }

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

        return classWriter;
    }

    private string GetOrdinalName(int position) {
        return position switch {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }
}
