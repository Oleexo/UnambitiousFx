using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.OneOf;

/// <summary>
///     Builds a concrete OneOf implementation for a specific position.
/// </summary>
internal static class OneOfImplementationBuilder
{
    public static ClassWriter Build(ushort arity,
                                    ushort position)
    {
        var ordinalName = OrdinalHelper.GetOrdinalName(position);
        var genericParams = GenericTypeHelper.CreateOrdinalGenericParameters(arity);
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var className = $"{ordinalName}OneOf";
        var baseClassName = $"OneOf<{allTypeParams}>";

        var classWriter = new ClassWriter(
            className,
            Visibility.Internal,
            ClassModifier.Sealed,
            genericParams,
            new TypeDefinitionReference(baseClassName)
        );

        AddField(classWriter, ordinalName);
        AddConstructor(classWriter, className, ordinalName);
        AddIsProperties(classWriter, arity, position);
        AddMatchMethods(classWriter, arity, ordinalName);
        AddExtractionMethods(classWriter, arity, position, ordinalName);

        return classWriter;
    }

    private static void AddField(ClassWriter classWriter,
                                 string ordinalName)
    {
        classWriter.AddField(new FieldWriter(
                                 $"_{ordinalName.ToLower()}",
                                 $"T{ordinalName}"
                             ));
    }

    private static void AddConstructor(ClassWriter classWriter,
                                       string className,
                                       string ordinalName)
    {
        classWriter.AddConstructor(new ConstructorWriter(
                                       className,
                                       $"_{ordinalName.ToLower()} = {ordinalName.ToLower()};",
                                       Visibility.Public,
                                       [new MethodParameter($"T{ordinalName}", ordinalName.ToLower())]
                                   ));
    }

    private static void AddIsProperties(ClassWriter classWriter,
                                        ushort arity,
                                        ushort position)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var currentOrdinal = OrdinalHelper.GetOrdinalName(i);
            classWriter.AddProperty(new PropertyWriter(
                                        $"Is{currentOrdinal}",
                                        "bool",
                                        getterBody: i == position
                                                        ? "true"
                                                        : "false",
                                        style: PropertyStyle.Override
                                    ));
        }
    }

    private static void AddMatchMethods(ClassWriter classWriter,
                                        ushort arity,
                                        string ordinalName)
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
                                  "Match",
                                  "TOut",
                                  $"return {ordinalName.ToLower()}Func(_{ordinalName.ToLower()});",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  matchFuncParams,
                                  [new GenericParameter("TOut", "")]
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
                                  "Match",
                                  "void",
                                  $"{ordinalName.ToLower()}Action(_{ordinalName.ToLower()});",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  matchActionParams
                              ));
    }

    private static void AddExtractionMethods(ClassWriter classWriter,
                                             ushort arity,
                                             ushort position,
                                             string ordinalName)
    {
        for (ushort i = 1; i <= arity; i++)
        {
            var currentOrdinal = OrdinalHelper.GetOrdinalName(i);
            var body = i == position
                           ? $"{currentOrdinal.ToLower()} = _{ordinalName.ToLower()};\nreturn true;"
                           : $"{currentOrdinal.ToLower()} = default;\nreturn false;";

            classWriter.AddMethod(new MethodWriter(
                                      currentOrdinal,
                                      "bool",
                                      body,
                                      Visibility.Public,
                                      MethodModifier.Override,
                                      [new MethodParameter($"[NotNullWhen(true)] out T{currentOrdinal}?", currentOrdinal.ToLower())],
                                      usings: ["System.Diagnostics.CodeAnalysis"]
                                  ));
        }
    }
}
