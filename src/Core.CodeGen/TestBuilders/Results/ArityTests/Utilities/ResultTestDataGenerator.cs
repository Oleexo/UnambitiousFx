using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Utilities;

/// <summary>
///     Generates test data for Result types across different arities and scenarios.
/// </summary>
internal sealed class ResultTestDataGenerator
{
    /// <summary>
    ///     Generates test data for successful Result scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <returns>Test data for success scenarios.</returns>
    public TestData GenerateSuccessData(ushort arity)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateSuccessSetupCode(arity);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for failed Result scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <returns>Test data for failure scenarios.</returns>
    public TestData GenerateFailureData(ushort arity)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateFailureSetupCode(arity);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Failure,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for exception Result scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <returns>Test data for exception scenarios.</returns>
    public TestData GenerateExceptionData(ushort arity)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateExceptionSetupCode(arity);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Exception,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for edge case Result scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <returns>Test data for edge case scenarios.</returns>
    public TestData GenerateEdgeCaseData(ushort arity)
    {
        var typeValuePairs = GenerateEdgeCaseTypeValuePairs(arity);
        var setupCode = GenerateEdgeCaseSetupCode(arity);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.EdgeCase,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for async Result scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <returns>Test data for async scenarios.</returns>
    public TestData GenerateAsyncData(ushort arity)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateAsyncSetupCode(arity);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for different Result value types and combinations.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="useNullableTypes">Whether to include nullable types.</param>
    /// <param name="useReferenceTypes">Whether to include reference types.</param>
    /// <returns>Test data with different type combinations.</returns>
    public TestData GenerateTypeCoverageData(ushort arity,
                                             bool useNullableTypes = false,
                                             bool useReferenceTypes = false)
    {
        var typeValuePairs = GenerateTypeCoverageTypeValuePairs(arity, useNullableTypes, useReferenceTypes);
        var setupCode = GenerateTypeCoverageSetupCode(arity, useNullableTypes, useReferenceTypes);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for factory method scenarios (Success, Failure, Try).
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="factoryMethod">The factory method name (Success, Failure, Try).</param>
    /// <returns>Test data for factory method scenarios.</returns>
    public TestData GenerateFactoryMethodData(ushort arity,
                                              string factoryMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateFactoryMethodSetupCode(arity, factoryMethod);
        var expectedState = factoryMethod switch
        {
            "Success" => ResultState.Success,
            "Failure" => ResultState.Failure,
            "Try" => ResultState.Success, // Try can succeed or fail, but we'll test success case
            _ => ResultState.Success
        };

        return new TestData(
            arity,
            typeValuePairs,
            expectedState,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for transformation method scenarios (Map, Bind, Flatten, etc.).
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="transformationMethod">The transformation method name.</param>
    /// <returns>Test data for transformation scenarios.</returns>
    public TestData GenerateTransformationData(ushort arity,
                                               string transformationMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateTransformationSetupCode(arity, transformationMethod);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for validation method scenarios (Ensure).
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="validationMethod">The validation method name.</param>
    /// <returns>Test data for validation scenarios.</returns>
    public TestData GenerateValidationData(ushort arity,
                                           string validationMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateValidationSetupCode(arity, validationMethod);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for side effect method scenarios (Tap, TapBoth, TapError).
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="sideEffectMethod">The side effect method name.</param>
    /// <returns>Test data for side effect scenarios.</returns>
    public TestData GenerateSideEffectData(ushort arity,
                                           string sideEffectMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateSideEffectSetupCode(arity, sideEffectMethod);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for error handling method scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="errorHandlingMethod">The error handling method name.</param>
    /// <returns>Test data for error handling scenarios.</returns>
    public TestData GenerateErrorHandlingData(ushort arity,
                                              string errorHandlingMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateErrorHandlingSetupCode(arity, errorHandlingMethod);
        var expectedState = errorHandlingMethod.StartsWith("Has") || errorHandlingMethod.StartsWith("Find")
                                ? ResultState.Success
                                : ResultState.Failure;

        return new TestData(
            arity,
            typeValuePairs,
            expectedState,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for value access method scenarios.
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="valueAccessMethod">The value access method name.</param>
    /// <returns>Test data for value access scenarios.</returns>
    public TestData GenerateValueAccessData(ushort arity,
                                            string valueAccessMethod)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateValueAccessSetupCode(arity, valueAccessMethod);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    /// <summary>
    ///     Generates test data for direct Result method scenarios (Match, IfSuccess, IfFailure, TryGet).
    /// </summary>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="directMethod">The direct method name.</param>
    /// <param name="isAsync">Whether this is for async variant testing.</param>
    /// <returns>Test data for direct method scenarios.</returns>
    public TestData GenerateDirectMethodData(ushort arity,
                                             string directMethod,
                                             bool isAsync = false)
    {
        var typeValuePairs = GenerateTypeValuePairs(arity);
        var setupCode = GenerateDirectMethodSetupCode(arity, directMethod, isAsync);

        return new TestData(
            arity,
            typeValuePairs,
            ResultState.Success,
            setupCode
        );
    }

    private IEnumerable<TypeValuePair> GenerateTypeValuePairs(ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var typeName = TestTypeHelper.GetTestType(i);
            var testValue = TestTypeHelper.GetTestValue(i);
            var isReferenceType = IsReferenceType(typeName);

            yield return new TypeValuePair(
                typeName,
                testValue,
                false,
                isReferenceType
            );
        }
    }

    private IEnumerable<TypeValuePair> GenerateEdgeCaseTypeValuePairs(ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var typeName = TestTypeHelper.GetTestType(i);
            var edgeValue = GetEdgeCaseValue(typeName);
            var isReferenceType = IsReferenceType(typeName);

            yield return new TypeValuePair(
                typeName,
                edgeValue,
                false,
                isReferenceType
            );
        }
    }

    private IEnumerable<TypeValuePair> GenerateTypeCoverageTypeValuePairs(ushort arity,
                                                                          bool useNullableTypes,
                                                                          bool useReferenceTypes)
    {
        for (var i = 1; i <= arity; i++)
        {
            string typeName;
            string testValue;
            var isNullable = false;
            var isReferenceType = false;

            if (useReferenceTypes && i % 2 == 0)
            {
                // Use reference types for even positions
                typeName = GetReferenceType(i);
                testValue = GetReferenceTypeValue(typeName);
                isReferenceType = true;
            }
            else if (useNullableTypes && i % 3 == 0)
            {
                // Use nullable types for positions divisible by 3
                typeName = TestTypeHelper.GetTestType(i) + "?";
                testValue = TestTypeHelper.GetTestValue(i);
                isNullable = true;
            }
            else
            {
                // Use standard types
                typeName = TestTypeHelper.GetTestType(i);
                testValue = TestTypeHelper.GetTestValue(i);
                isReferenceType = IsReferenceType(typeName);
            }

            yield return new TypeValuePair(
                typeName,
                testValue,
                isNullable,
                isReferenceType
            );
        }
    }

    private string GenerateSuccessSetupCode(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        if (arity == 1)
        {
            return $"// Given: A successful Result with value {values[0]}\nvar result = Result.Success({values[0]});";
        }

        var valuesList = string.Join(", ", values);
        return $"// Given: A successful Result with values ({valuesList})\nvar result = Result.Success({valuesList});";
    }

    private string GenerateFailureSetupCode(ushort arity)
    {
        var typeString = GenerateResultTypeString(arity);
        return $"// Given: A failed Result with exception\nvar exception = new Exception(\"Test failure\");\nvar result = Result.Failure<{typeString}>(exception);";
    }

    private string GenerateExceptionSetupCode(ushort arity)
    {
        var typeString = GenerateResultTypeString(arity);
        return $"// Given: A Result with exception\nvar exception = new InvalidOperationException(\"Test exception\");\nvar result = Result.Failure<{typeString}>(exception);";
    }

    private string GenerateEdgeCaseSetupCode(ushort arity)
    {
        var edgeValues = Enumerable.Range(1, arity)
                                   .Select(i => GetEdgeCaseValue(TestTypeHelper.GetTestType(i)))
                                   .ToArray();

        if (arity == 1)
        {
            return $"// Given: A Result with edge case value {edgeValues[0]}\nvar result = Result.Success({edgeValues[0]});";
        }

        var valuesList = string.Join(", ", edgeValues);
        return $"// Given: A Result with edge case values ({valuesList})\nvar result = Result.Success({valuesList});";
    }

    private string GenerateAsyncSetupCode(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        var setupCode = "// Given: An async scenario with cancellation token\nvar cancellationToken = CancellationToken.None;\n";

        if (arity == 1)
        {
            setupCode += $"var result = Task.FromResult(Result.Success({values[0]}));";
        }
        else
        {
            var valuesList = string.Join(", ", values);
            setupCode += $"var result = Task.FromResult(Result.Success({valuesList}));";
        }

        return setupCode;
    }

    private string GenerateTypeCoverageSetupCode(ushort arity,
                                                 bool useNullableTypes,
                                                 bool useReferenceTypes)
    {
        var setupLines = new List<string> { "// Setup for type coverage scenario" };

        if (useReferenceTypes)
        {
            setupLines.Add("var customObject = new CustomTestClass();");
        }

        if (useNullableTypes)
        {
            setupLines.Add("// Nullable types are included in this test");
        }

        return string.Join("\n", setupLines);
    }

    private string GetEdgeCaseValue(string typeName)
    {
        return typeName switch
        {
            "int" => "int.MaxValue",
            "string" => "string.Empty",
            "bool" => "false",
            "double" => "double.MaxValue",
            "decimal" => "decimal.MaxValue",
            "long" => "long.MaxValue",
            "char" => "'\\0'",
            "float" => "float.MaxValue",
            _ => TestTypeHelper.GetTestValue(1) // fallback to default
        };
    }

    private string GetReferenceType(int position)
    {
        return position switch
        {
            2 => "string",
            4 => "object",
            6 => "CustomTestClass",
            8 => "List<int>",
            _ => "string" // fallback
        };
    }

    private string GetReferenceTypeValue(string typeName)
    {
        return typeName switch
        {
            "string" => "\"reference type test\"",
            "object" => "new object()",
            "CustomTestClass" => "new CustomTestClass()",
            "List<int>" => "new List<int> { 1, 2, 3 }",
            _ => "null"
        };
    }

    private bool IsReferenceType(string typeName)
    {
        return typeName switch
        {
            "string" => true,
            "object" => true,
            _ when typeName.Contains("Class") => true,
            _ when typeName.Contains("List") => true,
            _ => false
        };
    }

    private string GenerateResultTypeString(ushort arity)
    {
        var types = Enumerable.Range(1, arity)
                              .Select(i => TestTypeHelper.GetTestType(i))
                              .ToArray();

        return string.Join(", ", types);
    }

    private string GenerateFactoryMethodSetupCode(ushort arity,
                                                  string factoryMethod)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        return factoryMethod switch
        {
            "Success" => GenerateSuccessSetupCode(arity),
            "Failure" => GenerateFailureSetupCode(arity),
            "Try" => GenerateTrySetupCode(arity, values),
            _ => GenerateSuccessSetupCode(arity)
        };
    }

    private string GenerateTransformationSetupCode(ushort arity,
                                                   string transformationMethod)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        var baseSetup = GenerateSuccessSetupCode(arity);
        var transformationSetup = transformationMethod switch
        {
            "Map" => GenerateMapSetupCode(arity),
            "Bind" => GenerateBindSetupCode(arity),
            "Flatten" => GenerateFlattenSetupCode(arity),
            "Zip" => GenerateZipSetupCode(arity),
            "Try" => GenerateTrySetupCode(arity, values),
            _ => "// Transformation setup"
        };

        return $"{baseSetup}\n{transformationSetup}";
    }

    private string GenerateValidationSetupCode(ushort arity,
                                               string validationMethod)
    {
        var baseSetup = GenerateSuccessSetupCode(arity);
        var validationSetup = validationMethod switch
        {
            "Ensure" => GenerateEnsureSetupCode(arity),
            "EnsureNotNull" => "// Validation: Ensure not null predicate\nFunc<object, bool> predicate = x => x != null;",
            "EnsureNotEmpty" => "// Validation: Ensure not empty predicate\nFunc<string, bool> predicate = x => !string.IsNullOrEmpty(x);",
            _ => "// Validation setup"
        };

        return $"{baseSetup}\n{validationSetup}";
    }

    private string GenerateTrySetupCode(ushort arity,
                                        string[] values)
    {
        if (arity == 1)
        {
            return $"// Given: A Try operation that succeeds\nFunc<{TestTypeHelper.GetTestType(1)}> operation = () => {values[0]};";
        }

        var typeString = GenerateResultTypeString(arity);
        var valuesList = string.Join(", ", values);
        return $"// Given: A Try operation that succeeds\nFunc<({typeString})> operation = () => ({valuesList});";
    }

    private string GenerateMapSetupCode(ushort arity)
    {
        if (arity == 1)
        {
            return $"// When: Mapping the value\nFunc<{TestTypeHelper.GetTestType(1)}, {TestTypeHelper.GetTestType(1)}> mapper = x => x;";
        }

        var typeString = GenerateResultTypeString(arity);
        return $"// When: Mapping the values\nFunc<({typeString}), ({typeString})> mapper = x => x;";
    }

    private string GenerateBindSetupCode(ushort arity)
    {
        var typeString = GenerateResultTypeString(arity);
        if (arity == 1)
        {
            return $"// When: Binding to another Result\nFunc<{TestTypeHelper.GetTestType(1)}, Result<{TestTypeHelper.GetTestType(1)}>> binder = x => Result.Success(x);";
        }

        return $"// When: Binding to another Result\nFunc<({typeString}), Result<{typeString}>> binder = x => Result.Success(x.Item1, x.Item2);";
    }

    private string GenerateFlattenSetupCode(ushort arity)
    {
        return "// When: Flattening nested Result";
    }

    private string GenerateZipSetupCode(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        if (arity >= 2)
        {
            return $"// When: Zipping with another Result\nvar other = Result.Success({values[1]});";
        }

        return $"// When: Zipping with another Result\nvar other = Result.Success({values[0]});";
    }

    private string GenerateEnsureSetupCode(ushort arity)
    {
        if (arity == 1)
        {
            return $"// When: Ensuring with predicate\nFunc<{TestTypeHelper.GetTestType(1)}, bool> predicate = x => true;";
        }

        var typeString = GenerateResultTypeString(arity);
        return $"// When: Ensuring with predicate\nFunc<({typeString}), bool> predicate = x => true;";
    }

    private string GenerateSideEffectSetupCode(ushort arity,
                                               string sideEffectMethod)
    {
        var baseSetup = GenerateSuccessSetupCode(arity);
        var sideEffectSetup = sideEffectMethod switch
        {
            "Tap" => GenerateTapSetupCode(arity),
            "TapBoth" => GenerateTapBothSetupCode(arity),
            "TapError" => GenerateTapErrorSetupCode(arity),
            _ => "// Side effect setup"
        };

        return $"{baseSetup}\n{sideEffectSetup}";
    }

    private string GenerateErrorHandlingSetupCode(ushort arity,
                                                  string errorHandlingMethod)
    {
        var baseSetup = GenerateFailureSetupCode(arity); // Start with failure for error handling
        var errorSetup = errorHandlingMethod switch
        {
            "MapError" => "// When: Mapping errors\nFunc<Exception, Exception> errorMapper = ex => new ArgumentException(ex.Message);",
            "MapErrors" =>
                "// When: Mapping multiple errors\nFunc<IEnumerable<Exception>, IEnumerable<Exception>> errorMapper = errors => errors.Select(e => new ArgumentException(e.Message));",
            "PrependError" => "// When: Prepending error\nvar newError = new ArgumentException(\"Prepended error\");",
            "AppendError" => "// When: Appending error\nvar newError = new ArgumentException(\"Appended error\");",
            "HasError" => "// When: Checking for specific error type",
            "HasException" => "// When: Checking for exception",
            "FindError" => "// When: Finding specific error\nFunc<Exception, bool> predicate = ex => ex is InvalidOperationException;",
            "MatchError" => "// When: Matching error\nFunc<Exception, string> errorMatcher = ex => ex.Message;",
            "FilterError" => "// When: Filtering errors\nFunc<Exception, bool> filter = ex => ex is InvalidOperationException;",
            "Recover" => GenerateRecoverSetupCode(arity),
            _ => "// Error handling setup"
        };

        return $"{baseSetup}\n{errorSetup}";
    }

    private string GenerateValueAccessSetupCode(ushort arity,
                                                string valueAccessMethod)
    {
        var baseSetup = GenerateSuccessSetupCode(arity);
        var accessSetup = valueAccessMethod switch
        {
            "ValueOr" => GenerateValueOrSetupCode(arity),
            "ValueOrThrow" => "// When: Getting value or throwing",
            "ToNullable" => "// When: Converting to nullable",
            _ => "// Value access setup"
        };

        return $"{baseSetup}\n{accessSetup}";
    }

    private string GenerateTapSetupCode(ushort arity)
    {
        if (arity == 1)
        {
            return $"// When: Tapping into success values\nvar tapped = false;\nAction<{TestTypeHelper.GetTestType(1)}> tapAction = x => {{ tapped = true; }};";
        }

        var typeString = GenerateResultTypeString(arity);
        return $"// When: Tapping into success values\nvar tapped = false;\nAction<({typeString})> tapAction = x => {{ tapped = true; }};";
    }

    private string GenerateTapBothSetupCode(ushort arity)
    {
        if (arity == 1)
        {
            return
                $"// When: Tapping into both success and failure\nvar successTapped = false;\nvar errorTapped = false;\nAction<{TestTypeHelper.GetTestType(1)}> successAction = x => {{ successTapped = true; }};\nAction<Exception> errorAction = ex => {{ errorTapped = true; }};";
        }

        var typeString = GenerateResultTypeString(arity);
        return
            $"// When: Tapping into both success and failure\nvar successTapped = false;\nvar errorTapped = false;\nAction<({typeString})> successAction = x => {{ successTapped = true; }};\nAction<Exception> errorAction = ex => {{ errorTapped = true; }};";
    }

    private string GenerateTapErrorSetupCode(ushort arity)
    {
        return "// When: Tapping into error\nvar errorTapped = false;\nAction<Exception> errorAction = ex => { errorTapped = true; };";
    }

    private string GenerateRecoverSetupCode(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => TestTypeHelper.GetTestValue(i))
                               .ToArray();

        if (arity == 1)
        {
            return $"// When: Recovering from failure\nFunc<Exception, {TestTypeHelper.GetTestType(1)}> recovery = ex => {values[0]};";
        }

        var typeString = GenerateResultTypeString(arity);
        var valuesList = string.Join(", ", values);
        return $"// When: Recovering from failure\nFunc<Exception, ({typeString})> recovery = ex => ({valuesList});";
    }

    private string GenerateValueOrSetupCode(ushort arity)
    {
        var fallbackValues = Enumerable.Range(1, arity)
                                       .Select(i => GetFallbackValue(TestTypeHelper.GetTestType(i)))
                                       .ToArray();

        if (arity == 1)
        {
            return $"// When: Getting value or fallback\nvar fallback = {fallbackValues[0]};";
        }

        var valuesList = string.Join(", ", fallbackValues);
        return $"// When: Getting values or fallback\nvar fallback = ({valuesList});";
    }

    private string GetFallbackValue(string typeName)
    {
        return typeName switch
        {
            "int" => "0",
            "string" => "\"fallback\"",
            "bool" => "false",
            "double" => "0.0",
            "decimal" => "0m",
            "long" => "0L",
            "char" => "'f'",
            "float" => "0f",
            _ => "default"
        };
    }

    private string GenerateDirectMethodSetupCode(ushort arity,
                                                 string directMethod,
                                                 bool isAsync)
    {
        var baseSetup = GenerateSuccessSetupCode(arity);
        var methodSetup = directMethod switch
        {
            "Match" => GenerateMatchSetupCode(arity, isAsync),
            "IfSuccess" => GenerateIfSuccessSetupCode(arity, isAsync),
            "IfFailure" => GenerateIfFailureSetupCode(arity, isAsync),
            "TryGet" => GenerateTryGetSetupCode(arity, isAsync),
            _ => "// Direct method setup"
        };

        return $"{baseSetup}\n{methodSetup}";
    }

    private string GenerateMatchSetupCode(ushort arity,
                                          bool isAsync)
    {
        var asyncModifier = isAsync
                                ? "async "
                                : "";
        var returnType = isAsync
                             ? "Task<string>"
                             : "string";
        var awaitKeyword = isAsync
                               ? "await "
                               : "";

        if (arity == 1)
        {
            return
                $"// When: Matching Result values\nFunc<{TestTypeHelper.GetTestType(1)}, {returnType}> onSuccess = {asyncModifier}x => {awaitKeyword}\"success\";\nFunc<Exception, {returnType}> onFailure = {asyncModifier}ex => {awaitKeyword}\"failure\";";
        }

        var typeString = GenerateResultTypeString(arity);
        return
            $"// When: Matching Result values\nFunc<({typeString}), {returnType}> onSuccess = {asyncModifier}x => {awaitKeyword}\"success\";\nFunc<Exception, {returnType}> onFailure = {asyncModifier}ex => {awaitKeyword}\"failure\";";
    }

    private string GenerateIfSuccessSetupCode(ushort arity,
                                              bool isAsync)
    {
        var asyncModifier = isAsync
                                ? "async "
                                : "";
        var taskType = isAsync
                           ? "Task"
                           : "";
        var awaitKeyword = isAsync
                               ? "await "
                               : "";

        if (arity == 1)
        {
            return
                $"// When: Executing action on success\nvar executed = false;\nFunc<{TestTypeHelper.GetTestType(1)}, {taskType}> action = {asyncModifier}x => {{ executed = true; {awaitKeyword}Task.CompletedTask; }};";
        }

        var typeString = GenerateResultTypeString(arity);
        return
            $"// When: Executing action on success\nvar executed = false;\nFunc<({typeString}), {taskType}> action = {asyncModifier}x => {{ executed = true; {awaitKeyword}Task.CompletedTask; }};";
    }

    private string GenerateIfFailureSetupCode(ushort arity,
                                              bool isAsync)
    {
        var asyncModifier = isAsync
                                ? "async "
                                : "";
        var taskType = isAsync
                           ? "Task"
                           : "";
        var awaitKeyword = isAsync
                               ? "await "
                               : "";

        return
            $"// When: Executing action on failure\nvar executed = false;\nFunc<Exception, {taskType}> action = {asyncModifier}ex => {{ executed = true; {awaitKeyword}Task.CompletedTask; }};";
    }

    private string GenerateTryGetSetupCode(ushort arity,
                                           bool isAsync)
    {
        if (arity == 1)
        {
            return $"// When: Trying to get value\n{TestTypeHelper.GetTestType(1)} outValue;";
        }

        var typeString = GenerateResultTypeString(arity);
        return $"// When: Trying to get values\n({typeString}) outValue;";
    }
}
