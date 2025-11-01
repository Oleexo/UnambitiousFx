using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.OneOf;

/// <summary>
/// Builds a concrete OneOf implementation for a specific position.
/// </summary>
internal static class OneOfImplementationBuilder {
    public static ClassWriter Build(ushort arity,
                                    ushort position) {
        var ordinalName   = OrdinalHelper.GetOrdinalName(position);
        var genericParams = GenericTypeHelper.CreateOrdinalGenericParameters(arity, "T", "notnull");
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

        AddField(classWriter, ordinalName);
        AddConstructor(classWriter, className, ordinalName);
        AddIsProperties(classWriter, arity, position);
        AddMatchMethods(classWriter, arity, position, ordinalName);
        AddExtractionMethods(classWriter, arity, position, ordinalName);

        return classWriter;
    }

    private static void AddField(ClassWriter classWriter,
                                 string      ordinalName) {
        classWriter.AddField(new FieldWriter(
                                 name: $"_{ordinalName.ToLower()}",
                                 type: $"T{ordinalName}",
                                 visibility: Visibility.Private,
                                 isReadonly: true
                             ));
    }

    private static void AddConstructor(ClassWriter classWriter,
                                       string      className,
                                       string      ordinalName) {
        classWriter.AddConstructor(new ConstructorWriter(
                                       className: className,
                                       body: $"_{ordinalName.ToLower()} = {ordinalName.ToLower()};",
                                       visibility: Visibility.Public,
                                       parameters: [new MethodParameter($"T{ordinalName}", ordinalName.ToLower())]
                                   ));
    }

    private static void AddIsProperties(ClassWriter classWriter,
                                        ushort      arity,
                                        ushort      position) {
        for (ushort i = 1; i <= arity; i++) {
            var currentOrdinal = OrdinalHelper.GetOrdinalName(i);
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
    }

    private static void AddMatchMethods(ClassWriter classWriter,
                                        ushort      arity,
                                        ushort      position,
                                        string      ordinalName) {
        // Match with return
        var matchFuncParams = Enumerable.Range(1, arity)
                                        .Select(i => {
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
                                          .Select(i => {
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

    private static void AddExtractionMethods(ClassWriter classWriter,
                                             ushort      arity,
                                             ushort      position,
                                             string      ordinalName) {
        for (ushort i = 1; i <= arity; i++) {
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