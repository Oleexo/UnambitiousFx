using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.OneOf;

/// <summary>
/// Builds the base OneOf class for a given arity.
/// </summary>
internal static class OneOfBaseClassBuilder {
    public static ClassWriter Build(ushort arity,
                                    string baseNamespace,
                                    string className) {
        var genericParams = GenericTypeHelper.CreateOrdinalGenericParameters(arity, "T", "notnull");

        var classDocBuilder = DocumentationWriter.Create()
                                                 .WithSummary($"Minimal discriminated union base abstraction representing a value that can be one of {arity} types.\n" +
                                                              "Specific semantic unions (e.g. Either<TLeft,TRight>) can inherit from this to express intent\n"         +
                                                              "without duplicating core shape.");

        for (int i = 0; i < genericParams.Length; i++) {
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

    private static void AddIsProperties(ClassWriter classWriter,
                                        ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
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

    private static void AddMatchMethods(ClassWriter classWriter,
                                        ushort      arity) {
        // Match with return value
        var matchFuncParams = Enumerable.Range(1, arity)
                                        .Select(i => {
                                             var ordinal = OrdinalHelper.GetOrdinalName(i);
                                             return new MethodParameter($"Func<T{ordinal}, TOut>", $"{ordinal.ToLower()}Func");
                                         })
                                        .ToArray();

        var matchFuncDocBuilder = DocumentationWriter.Create()
                                                     .WithSummary("Pattern match returning a value.")
                                                     .WithTypeParameter("TOut", "The type of value to return");

        foreach (var param in matchFuncParams) {
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
                                          .Select(i => {
                                               var ordinal = OrdinalHelper.GetOrdinalName(i);
                                               return new MethodParameter($"Action<T{ordinal}>", $"{ordinal.ToLower()}Action");
                                           })
                                          .ToArray();

        var matchActionDocBuilder = DocumentationWriter.Create()
                                                       .WithSummary("Pattern match executing side-effect actions.");

        foreach (var param in matchActionParams) {
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

    private static void AddExtractionMethods(ClassWriter classWriter,
                                             ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
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

    private static void AddFactoryMethods(ClassWriter        classWriter,
                                          ushort             arity,
                                          GenericParameter[] genericParams,
                                          string             className) {
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));

        for (ushort i = 1; i <= arity; i++) {
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

    private static void AddImplicitOperators(ClassWriter        classWriter,
                                             ushort             arity,
                                             GenericParameter[] genericParams,
                                             string             className) {
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));

        for (ushort i = 1; i <= arity; i++) {
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