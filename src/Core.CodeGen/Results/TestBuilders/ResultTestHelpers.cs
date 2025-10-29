namespace UnambitiousFx.Core.CodeGen.Results.TestBuilders;

/// <summary>
/// Provides helper methods for generating Result test data and code patterns.
/// </summary>
internal static class ResultTestHelpers {
    /// <summary>
    /// Gets the C# type name for a given position in the Result generic parameters.
    /// </summary>
    public static string GetTestType(int position) {
        return position switch {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "decimal",
            6 => "long",
            7 => "char",
            8 => "float",
            _ => throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 1 and 8.")
        };
    }

    /// <summary>
    /// Gets a test value for a given position in the Result generic parameters.
    /// </summary>
    public static string GetTestValue(int position) {
        return position switch {
            1 => "42",
            2 => "\"foo\"",
            3 => "true",
            4 => "3.14",
            5 => "99.99m",
            6 => "1000L",
            7 => "'x'",
            8 => "2.5f",
            _ => throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 1 and 8.")
        };
    }

    /// <summary>
    /// Gets the default value for a given type position.
    /// </summary>
    public static string GetDefaultValue(int position) {
        return position switch {
            1 => "0",
            2 => "null",
            3 => "false",
            4 => "0.0",
            5 => "0m",
            6 => "0L",
            7 => "'\\0'",
            8 => "0f",
            _ => throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 1 and 8.")
        };
    }

    /// <summary>
    /// Generates a Result.Success() call with appropriate test values.
    /// </summary>
    public static string GenerateSuccessCall(ushort arity) {
        var values = Enumerable.Range(1, arity)
                               .Select(GetTestValue)
                               .ToArray();
        return $"Result.Success({string.Join(", ", values)})";
    }

    /// <summary>
    /// Generates a Result.Failure() call with appropriate generic type parameters.
    /// </summary>
    public static string GenerateFailureCall(ushort arity) {
        var types = Enumerable.Range(1, arity)
                              .Select(GetTestType)
                              .ToArray();
        return $"Result.Failure<{string.Join(", ", types)}>(new Exception(\"boom\"))";
    }

    /// <summary>
    /// Generates parameter names for lambda expressions (v or v1, v2, ...).
    /// </summary>
    public static string GenerateValueParameters(ushort arity) {
        return string.Join(", ", Enumerable.Range(1, arity)
                                           .Select(i => arity == 1
                                                            ? "v"
                                                            : $"v{i}"));
    }

    /// <summary>
    /// Generates wildcard parameters for lambda expressions (_, _, ...).
    /// </summary>
    public static string GenerateWildcardParameters(ushort arity) {
        return string.Join(", ", Enumerable.Repeat("_", arity));
    }

    /// <summary>
    /// Generates assertions for value parameters, using Assert.True/False for booleans.
    /// </summary>
    public static string GenerateValueAssertions(ushort arity,
                                                 string indent = "        ") {
        var result = "";
        for (int i = 1; i <= arity; i++) {
            var varName = arity == 1
                              ? "v"
                              : $"v{i}";
            var testType  = GetTestType(i);
            var testValue = GetTestValue(i);

            if (testType == "bool") {
                result += $"{indent}Assert.{(testValue == "true" ? "True" : "False")}({varName});\n";
            }
            else {
                result += $"{indent}Assert.Equal({testValue}, {varName});\n";
            }
        }

        return result;
    }

    /// <summary>
    /// Checks if a type at the given position is boolean.
    /// </summary>
    public static bool IsBooleanType(int position) {
        return GetTestType(position) == "bool";
    }

    /// <summary>
    /// Generates an assertion for a single value, using Assert.True/False for booleans.
    /// </summary>
    public static string GenerateValueAssertion(int    position,
                                                string variableName) {
        var testType  = GetTestType(position);
        var testValue = GetTestValue(position);

        if (testType == "bool") {
            return $"Assert.{(testValue == "true" ? "True" : "False")}({variableName});";
        }

        return $"Assert.Equal({testValue}, {variableName});";
    }
}
