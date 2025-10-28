using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

internal class ResultCodeGenerator : ICodeGenerator {
    private readonly string _baseNamespace;
    private const    int    StartArity    = 1;
    private const    string ClassName     = "Result";
    private const    string DirectoryName = "Results";

    public ResultCodeGenerator(string baseNamespace) {
        _baseNamespace = baseNamespace;
    }

    public void Generate(ushort numberOfArity,
                         string outputPath) {
        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }

        for (ushort i = StartArity; i <= numberOfArity; i++) {
            GenerateResultBase(i, outputPath);
            GenerateSuccessImplementation(i, outputPath);
            GenerateFailureImplementation(i, outputPath);
        }
    }

    private void GenerateResultBase(ushort arity,
                                    string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.Results");
        var classWriter = CreateResultBaseClass(arity);

        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.cs");

        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);

        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private void GenerateSuccessImplementation(ushort arity,
                                               string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.Results");
        var classWriter = CreateSuccessImplementation(arity);

        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.Success.cs");

        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);

        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private void GenerateFailureImplementation(ushort arity,
                                               string outputPath) {
        var fileWriter  = new FileWriter($"{_baseNamespace}.Results");
        var classWriter = CreateFailureImplementation(arity);

        fileWriter.AddClass(classWriter);

        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}.Failure.cs");

        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);

        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private ClassWriter CreateResultBaseClass(ushort arity) {
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"TValue{i}", "notnull"))
                                      .ToArray();

        var classDocBuilder = DocumentationWriter.Create()
                                                 .WithSummary($"Represents the result of an operation that can succeed with {arity} value(s) or fail with an exception.");

        for (int i = 0; i < genericParams.Length; i++) {
            classDocBuilder.WithTypeParameter(
                genericParams[i].Name,
                $"The type of the {GetOrdinalName(i + 1).ToLower()} value."
            );
        }

        var classWriter = new ClassWriter(
            name: ClassName,
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Abstract,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference("BaseResult"),
            documentation: classDocBuilder.Build()
        );

        // Generate Match method with value parameters
        var matchActionParams = new List<MethodParameter>();
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"));
        matchActionParams.Add(new MethodParameter($"Action<{valueParams}>", "success"));
        matchActionParams.Add(new MethodParameter("Action<Exception>", "failure"));

        var matchActionDoc = DocumentationWriter.Create()
                                                .WithSummary("Pattern matches the result, executing the appropriate action.")
                                                .WithParameter("success", "Action to execute if the result is successful")
                                                .WithParameter("failure", "Action to execute if the result is a failure")
                                                .Build();

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  visibility: Visibility.Public,
                                  parameters: matchActionParams.ToArray(),
                                  documentation: matchActionDoc
                              ));

        // Generate Match method with Func parameters
        var matchFuncParams = new List<MethodParameter>();
        matchFuncParams.Add(new MethodParameter($"Func<{valueParams}, TOut>", "success"));
        matchFuncParams.Add(new MethodParameter("Func<Exception, TOut>", "failure"));

        var matchFuncDoc = DocumentationWriter.Create()
                                              .WithSummary("Pattern matches the result, returning a value from the appropriate function.")
                                              .WithTypeParameter("TOut", "The type of value to return")
                                              .WithParameter("success", "Function to invoke if the result is successful")
                                              .WithParameter("failure", "Function to invoke if the result is a failure")
                                              .WithReturns("The result of invoking the appropriate function")
                                              .Build();

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  visibility: Visibility.Public,
                                  parameters: matchFuncParams.ToArray(),
                                  genericParameters: [new GenericParameter("TOut", "")],
                                  documentation: matchFuncDoc
                              ));

        // Generate IfSuccess method
        var ifSuccessDoc = DocumentationWriter.Create()
                                              .WithSummary("Executes the action if the result is successful.")
                                              .WithParameter("action", "Action to execute with the success values")
                                              .Build();

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  visibility: Visibility.Public,
                                  parameters: [new MethodParameter($"Action<{valueParams}>", "action")],
                                  documentation: ifSuccessDoc
                              ));

        // Generate Ok method with error
        var okParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }
        okParams.Add(new MethodParameter("[NotNullWhen(false)] out Exception?", "error"));

        var okDoc = DocumentationWriter.Create()
                                       .WithSummary("Attempts to extract the success values and error.");

        for (int i = 1; i <= arity; i++) {
            okDoc.WithParameter($"value{i}", $"The {GetOrdinalName(i).ToLower()} value if successful");
        }

        okDoc.WithParameter("error", "The exception if failed")
             .WithReturns("True if successful, false otherwise");

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  visibility: Visibility.Public,
                                  parameters: okParams.ToArray(),
                                  documentation: okDoc.Build(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Generate Ok method without error
        var okWithoutErrorParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okWithoutErrorParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var okWithoutErrorDoc = DocumentationWriter.Create()
                                                   .WithSummary("Attempts to extract the success values.");

        for (int i = 1; i <= arity; i++) {
            okWithoutErrorDoc.WithParameter($"value{i}", $"The {GetOrdinalName(i).ToLower()} value if successful");
        }
        okWithoutErrorDoc.WithReturns("True if successful, false otherwise") ;

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  visibility: Visibility.Public,
                                  parameters: okWithoutErrorParams.ToArray(),
                                  documentation: okWithoutErrorDoc.Build(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Generate Deconstruct method
        var deconstructParams = new List<MethodParameter>();
        deconstructParams.Add(new MethodParameter("out bool", "isSuccess"));

        if (arity == 1) {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else {
            var tupleType = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"))})";
            deconstructParams.Add(new MethodParameter($"out {tupleType}?", "value"));
        }
        deconstructParams.Add(new MethodParameter("out Exception?", "error"));

        var deconstructDoc = DocumentationWriter.Create()
                                                .WithSummary("Deconstructs the result into its components.")
                                                .WithParameter("isSuccess", "Whether the result is successful")
                                                .WithParameter("value", "The success value(s) if successful")
                                                .WithParameter("error", "The exception if failed")
                                                .Build();

        classWriter.AddMethod(new AbstractMethodWriter(
                                  name: "Deconstruct",
                                  returnType: "void",
                                  visibility: Visibility.Public,
                                  parameters: deconstructParams.ToArray(),
                                  documentation: deconstructDoc
                              ));

        return classWriter;
    }

    private ClassWriter CreateSuccessImplementation(ushort arity) {
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"TValue{i}", "notnull"))
                                      .ToArray();

        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var baseClassName = $"Result<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: $"SuccessResult",
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName),
            interfaces: [new TypeDefinitionReference("ISuccessResult")]
        );

        // Add fields
        for (int i = 1; i <= arity; i++) {
            classWriter.AddField(new FieldWriter(
                                     name: $"_value{i}",
                                     type: $"TValue{i}",
                                     visibility: Visibility.Private,
                                     isReadonly: true
                                 ));
        }

        // Add constructor
        var constructorParams = Enumerable.Range(1, arity)
                                          .Select(i => new MethodParameter($"TValue{i}", $"value{i}"))
                                          .ToArray();
        var constructorBody = string.Join("\n", Enumerable.Range(1, arity).Select(i => $"_value{i} = value{i};"));

        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "SuccessResult",
                                       body: constructorBody,
                                       visibility: Visibility.Public,
                                       parameters: constructorParams
                                   ));

        // Add properties
        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsFaulted",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));

        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsSuccess",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "true",
                                    style: PropertyStyle.Override
                                ));

        // Add Match methods from BaseResult
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "success();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Action", "success"),
                                      new MethodParameter("Action<Exception>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: "return success();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Func<TOut>", "success"),
                                      new MethodParameter("Func<Exception, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: "action();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action", "action")]
                              ));

        // Add Match method with values
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"));
        var valueArgs = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"_value{i}"));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: $"success({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Action<{valueParams}>", "success"),
                                      new MethodParameter("Action<Exception>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: $"return success({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Func<{valueParams}, TOut>", "success"),
                                      new MethodParameter("Func<Exception, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: $"action({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfFailure",
                                  returnType: "void",
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action<Exception>", "action")]
                              ));

        // Add Ok method from BaseResult
        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: "error = null;\nreturn true;",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("[NotNullWhen(false)] out Exception?", "error")],
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Ok method with values and error
        var okParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }
        okParams.Add(new MethodParameter("[NotNullWhen(false)] out Exception?", "error"));

        var okBody = string.Join("\n", Enumerable.Range(1, arity).Select(i => $"value{i} = _value{i};")) + "\nerror = null;\nreturn true;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: okBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: okParams.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Ok method without error
        var okWithoutErrorParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okWithoutErrorParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var okWithoutErrorBody = string.Join("\n", Enumerable.Range(1, arity).Select(i => $"value{i} = _value{i};")) + "\nreturn true;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: okWithoutErrorBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: okWithoutErrorParams.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Deconstruct method
        var deconstructParams = new List<MethodParameter>();
        deconstructParams.Add(new MethodParameter("out bool", "isSuccess"));

        if (arity == 1) {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else {
            var tupleType = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"))})";
            deconstructParams.Add(new MethodParameter($"out {tupleType}?", "value"));
        }
        deconstructParams.Add(new MethodParameter("out Exception?", "error"));

        string deconstructBody;
        if (arity == 1) {
            deconstructBody = "isSuccess = true;\nvalue = _value1;\nerror = null;";
        }
        else {
            var tupleValues = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"_value{i}"));
            deconstructBody = $"isSuccess = true;\nvalue = ({tupleValues});\nerror = null;";
        }

        classWriter.AddMethod(new MethodWriter(
                                  name: "Deconstruct",
                                  returnType: "void",
                                  body: deconstructBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: deconstructParams.ToArray()
                              ));

        // Add ToString method
        var toStringBody = GenerateSuccessToStringBody(arity);
        classWriter.AddMethod(new MethodWriter(
                                  name: "ToString",
                                  returnType: "string",
                                  body: toStringBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: []
                              ));

        return classWriter;
    }

    private ClassWriter CreateFailureImplementation(ushort arity) {
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"TValue{i}", "notnull"))
                                      .ToArray();

        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var baseClassName = $"Result<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: "FailureResult",
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName),
            interfaces: [new TypeDefinitionReference("IFailureResult")]
        );

        // Add constructor
        var constructorBody = "PrimaryException = error;\nif (attachPrimaryExceptionalReason) {\n    AddReason(new ExceptionalError(error));\n}";
        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "FailureResult",
                                       body: constructorBody,
                                       visibility: Visibility.Internal,
                                       parameters: [
                                           new MethodParameter("Exception", "error"),
                                           new MethodParameter("bool", "attachPrimaryExceptionalReason")
                                       ]
                                   ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "FailureResult",
                                       body: "",
                                       visibility: Visibility.Public,
                                       parameters: [new MethodParameter("Exception", "error")],
                                       baseCall: "this(error, true)"
                                   ));

        // Add PrimaryException property
        classWriter.AddProperty(new PropertyWriter(
                                    name: "PrimaryException",
                                    type: "Exception",
                                    visibility: Visibility.Public,
                                    hasGetter: true,
                                    hasSetter: false,
                                    style: PropertyStyle.AutoProperty
                                ));

        // Add properties
        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsFaulted",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "true",
                                    style: PropertyStyle.Override
                                ));

        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsSuccess",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));

        // Add Match methods from BaseResult
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "failure(PrimaryException);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Action", "success"),
                                      new MethodParameter("Action<Exception>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: "return failure(PrimaryException);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Func<TOut>", "success"),
                                      new MethodParameter("Func<Exception, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action", "action")]
                              ));

        // Add Match method with values
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "failure(PrimaryException);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Action<{valueParams}>", "success"),
                                      new MethodParameter("Action<Exception>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: "return failure(PrimaryException);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Func<{valueParams}, TOut>", "success"),
                                      new MethodParameter("Func<Exception, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfFailure",
                                  returnType: "void",
                                  body: "action(PrimaryException);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action<Exception>", "action")]
                              ));

        // Add Ok method from BaseResult
        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: "error = PrimaryException;\nreturn false;",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("[NotNullWhen(false)] out Exception?", "error")],
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Ok method with values and error
        var okParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }
        okParams.Add(new MethodParameter("[NotNullWhen(false)] out Exception?", "error"));

        var okBody = string.Join("\n", Enumerable.Range(1, arity).Select(i => $"value{i} = default;")) + "\nerror = PrimaryException;\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: okBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: okParams.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Ok method without error
        var okWithoutErrorParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) {
            okWithoutErrorParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var okWithoutErrorBody = string.Join("\n", Enumerable.Range(1, arity).Select(i => $"value{i} = default;")) + "\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Ok",
                                  returnType: "bool",
                                  body: okWithoutErrorBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: okWithoutErrorParams.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        // Add Deconstruct method
        var deconstructParams = new List<MethodParameter>();
        deconstructParams.Add(new MethodParameter("out bool", "isSuccess"));

        if (arity == 1) {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else {
            var tupleType = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"))})";
            deconstructParams.Add(new MethodParameter($"out {tupleType}?", "value"));
        }
        deconstructParams.Add(new MethodParameter("out Exception?", "error"));

        var deconstructBody = "isSuccess = false;\nvalue = null;\nerror = PrimaryException;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Deconstruct",
                                  returnType: "void",
                                  body: deconstructBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: deconstructParams.ToArray()
                              ));

        // Add ToString method
        var toStringBody = GenerateFailureToStringBody(arity);
        classWriter.AddMethod(new MethodWriter(
                                  name: "ToString",
                                  returnType: "string",
                                  body: toStringBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: []
                              ));

        return classWriter;
    }

    private string GenerateSuccessToStringBody(ushort arity) {
        var formatTypeMethod = @"string FormatType(Type t) {
    return t == typeof(int)
               ? ""int""
               : t == typeof(string)
                   ? ""string""
                   : t == typeof(bool)
                       ? ""bool""
                       : t == typeof(long)
                           ? ""long""
                           : t == typeof(short)
                               ? ""short""
                               : t == typeof(byte)
                                   ? ""byte""
                                   : t == typeof(char)
                                       ? ""char""
                                       : t == typeof(decimal)
                                           ? ""decimal""
                                           : t == typeof(double)
                                               ? ""double""
                                               : t == typeof(float)
                                                   ? ""float""
                                                   : t == typeof(object)
                                                       ? ""object""
                                                       : t.IsGenericType
                                                           ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                           : t.Name;
}";

        var valueList = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"_value{i}"));

        return $@"{formatTypeMethod}

var typeArgs = GetType().GetGenericArguments();
var typeList = string.Join("", "", typeArgs.Select(FormatType));
var metaPart = Metadata.Count == 0
                   ? string.Empty
                   : "" meta="" +
                     string.Join("","", Metadata.Take(2)
                                              .Select(kv => kv.Key + "":"" + (kv.Value ?? ""null"")));
return $""Success<{{typeList}}>({valueList}) reasons={{Reasons.Count}}{{metaPart}}"";";
    }

    private string GenerateFailureToStringBody(ushort arity) {
        var formatTypeMethod = @"string FormatType(Type t) {
    return t == typeof(int)
               ? ""int""
               : t == typeof(string)
                   ? ""string""
                   : t == typeof(bool)
                       ? ""bool""
                       : t == typeof(long)
                           ? ""long""
                           : t == typeof(short)
                               ? ""short""
                               : t == typeof(byte)
                                   ? ""byte""
                                   : t == typeof(char)
                                       ? ""char""
                                       : t == typeof(decimal)
                                           ? ""decimal""
                                           : t == typeof(double)
                                               ? ""double""
                                               : t == typeof(float)
                                                   ? ""float""
                                                   : t == typeof(object)
                                                       ? ""object""
                                                       : t.IsGenericType
                                                           ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                           : t.Name;
}";

        return $@"{formatTypeMethod}

var typeArgs = GetType().GetGenericArguments();
var typeList = string.Join("", "", typeArgs.Select(FormatType));
var firstNonExceptional = Reasons.OfType<IError>()
                                 .FirstOrDefault(r => r is not ExceptionalError);
var firstAny = Reasons.OfType<IError>()
                      .FirstOrDefault();
var chosen = firstNonExceptional ?? firstAny;
var headerType = chosen switch {{
    ExceptionalError => PrimaryException.GetType().Name,
    null => PrimaryException.GetType().Name,
    _ => chosen.GetType().Name
}};
var headerMessage = chosen?.Message ?? PrimaryException.Message;
var codePart = chosen is not null and not ExceptionalError
                   ? "" code="" + chosen.Code
                   : string.Empty;
var metaPart = Metadata.Count == 0
                   ? string.Empty
                   : "" meta="" +
                     string.Join("","", Metadata.Take(2)
                                              .Select(kv => kv.Key + "":"" + (kv.Value ?? ""null"")));
return $""Failure<{{typeList}}>({{headerType}}: {{headerMessage}}){{codePart}} reasons={{{{Reasons.Count}}}}{{metaPart}}"";";
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
