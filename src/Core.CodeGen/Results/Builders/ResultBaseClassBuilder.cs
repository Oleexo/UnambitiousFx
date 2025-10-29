using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.Builders;

internal static class ResultBaseClassBuilder
{
    public static ClassWriter Build(ushort arity)
    {
        var genericParams = Enumerable.Range(1, arity)
            .Select(i => new GenericParameter($"TValue{i}", "notnull"))
            .ToArray();

        var classDocBuilder = DocumentationWriter.Create()
            .WithSummary($"Represents the result of an operation that can succeed with {arity} value(s) or fail with an exception.");

        for (int i = 0; i < genericParams.Length; i++)
        {
            classDocBuilder.WithTypeParameter(
                genericParams[i].Name,
                $"The type of the {ResultArityHelpers.GetOrdinalName(i + 1).ToLower()} value.");
        }

        var classWriter = new ClassWriter(
            name: "Result",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Abstract,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference("BaseResult"),
            documentation: classDocBuilder.Build()
        );

        // Generate Match method with value parameters
        var valueParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"));

        var matchActionDoc = DocumentationWriter.Create()
            .WithSummary("Pattern matches the result, executing the appropriate action.")
            .WithParameter("success", "Action to execute if the result is successful")
            .WithParameter("failure", "Action to execute if the result is a failure")
            .Build();

        classWriter.AddMethod(new AbstractMethodWriter(
            name: "Match",
            returnType: "void",
            visibility: Visibility.Public,
            parameters: new[]
            {
                new MethodParameter($"Action<{valueParams}>", "success"),
                new MethodParameter("Action<IEnumerable<IError>>", "failure"),
            },
            documentation: matchActionDoc
        ));

        // Generate Match method with Func parameters
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
            parameters: new[]
            {
                new MethodParameter($"Func<{valueParams}, TOut>", "success"),
                new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure"),
            },
            genericParameters: new[] { new GenericParameter("TOut", "") },
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
            parameters: new[] { new MethodParameter($"Action<{valueParams}>", "action") },
            documentation: ifSuccessDoc
        ));

        // Generate TryGet method with error
        var okParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++)
        {
            okParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }
        okParams.Add(new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "error"));

        var okDoc = DocumentationWriter.Create()
            .WithSummary("Attempts to extract the success values and error.");

        for (int i = 1; i <= arity; i++)
        {
            okDoc.WithParameter($"value{i}", $"The {ResultArityHelpers.GetOrdinalName(i).ToLower()} value if successful");
        }

        okDoc.WithParameter("error", "The exception if failed")
             .WithReturns("True if successful, false otherwise");

        classWriter.AddMethod(new AbstractMethodWriter(
            name: "TryGet",
            returnType: "bool",
            visibility: Visibility.Public,
            parameters: okParams.ToArray(),
            documentation: okDoc.Build(),
            usings: new[] { "System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons" }
        ));

        // Generate TryGet method without error
        var okWithoutErrorParams = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++)
        {
            okWithoutErrorParams.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var okWithoutErrorDoc = DocumentationWriter.Create()
            .WithSummary("Attempts to extract the success values.");

        for (int i = 1; i <= arity; i++)
        {
            okWithoutErrorDoc.WithParameter($"value{i}", $"The {ResultArityHelpers.GetOrdinalName(i).ToLower()} value if successful");
        }

        okWithoutErrorDoc.WithReturns("True if successful, false otherwise");

        classWriter.AddMethod(new AbstractMethodWriter(
            name: "TryGet",
            returnType: "bool",
            visibility: Visibility.Public,
            parameters: okWithoutErrorParams.ToArray(),
            documentation: okWithoutErrorDoc.Build(),
            usings: new[] { "System.Diagnostics.CodeAnalysis" }
        ));

        // Generate Deconstruct method
        var deconstructParams = new List<MethodParameter> { new("out bool", "isSuccess") };
        if (arity == 1)
        {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else
        {
            var tupleType = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"TValue{i}"))})";
            deconstructParams.Add(new MethodParameter($"out {tupleType}?", "value"));
        }
        deconstructParams.Add(new MethodParameter("out IEnumerable<IError>?", "error"));

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
            documentation: deconstructDoc,
            usings: new[] { "UnambitiousFx.Core.Results.Reasons" }
        ));

        return classWriter;
    }
}