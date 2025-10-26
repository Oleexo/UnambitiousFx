using System.Text;

namespace UnambitiousFx.Core.CodeGen;

internal class OneOfCodeGenerator : ICodeGenerator 
{
    public void Generate(ushort numberOfArity, string outputPath) 
    {
        for (ushort i = 1; i <= numberOfArity; i++) 
        {
            var className = $"OneOf{i}";
            var content = GenerateOneOfClass(i);
            var fileName = Path.Combine(outputPath, $"{className}.cs");
            
            Directory.CreateDirectory(outputPath);
            File.WriteAllText(fileName, content);
        }
    }

    private string GenerateOneOfClass(ushort arity)
    {
        var sb = new StringBuilder();
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));
        var className = $"OneOf{arity}";

        // Using raw string literal for template (C# 11+)
        var template = $$"""
            using System;

            namespace UnambitiousFx.Core;

            /// <summary>
            /// Represents a union type that can hold one of {{arity}} possible types.
            /// </summary>
            {{GenerateGenericConstraints(arity)}}
            public readonly struct {{className}}<{{genericTypes}}>
            {
                private readonly object? _value;
                private readonly int _index;

            {{GenerateConstructors(arity)}}

            {{GenerateProperties(arity)}}

            {{GenerateImplicitOperators(arity)}}

            {{GenerateMatchMethods(arity)}}

                public override string ToString() => _value?.ToString() ?? string.Empty;
                
                public override bool Equals(object? obj) => 
                    obj is {{className}}<{{genericTypes}}> other && 
                    _index == other._index && 
                    Equals(_value, other._value);
                
                public override int GetHashCode() => HashCode.Combine(_value, _index);
            }
            """;

        return template;
    }

    private string GenerateGenericConstraints(ushort arity)
    {
        // Add generic constraints if needed
        return string.Empty;
    }

    private string GenerateConstructors(ushort arity)
    {
        var sb = new StringBuilder();
        
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($"""
                private {$"OneOf{arity}"}(T{i} value)
                {{
                    _value = value;
                    _index = {i - 1};
                }}

            """);
        }

        return sb.ToString();
    }

    private string GenerateProperties(ushort arity)
    {
        var sb = new StringBuilder();
        
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($"""
                public bool IsT{i} => _index == {i - 1};
                public T{i} AsT{i} => _index == {i - 1} ? (T{i})_value! : throw new InvalidOperationException($"Cannot return T{i} when index is {{_index}}");

            """);
        }

        return sb.ToString();
    }

    private string GenerateImplicitOperators(ushort arity)
    {
        var sb = new StringBuilder();
        var className = $"OneOf{arity}";
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));
        
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($"""
                public static implicit operator {className}<{genericTypes}>(T{i} value) => new(value);

            """);
        }

        return sb.ToString();
    }

    private string GenerateMatchMethods(ushort arity)
    {
        var sb = new StringBuilder();
        var funcParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"Func<T{i}, TResult> f{i}"));
        var actionParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"Action<T{i}> f{i}"));
        
        // Generate Match method with return value
        sb.AppendLine($"""
            public TResult Match<TResult>({funcParams})
            {{
                return _index switch
                {{
        """);
        
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($"            {i - 1} => f{i}((T{i})_value!),");
        }
        
        sb.AppendLine($"""
                    _ => throw new InvalidOperationException($"Invalid index: {{_index}}")
                }};
            }}

        """);

        // Generate Switch method without return value
        sb.AppendLine($"""
            public void Switch({actionParams})
            {{
                switch (_index)
                {{
        """);
        
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($"""
                    case {i - 1}:
                        f{i}((T{i})_value!);
                        break;
            """);
        }
        
        sb.AppendLine($"""
                    default:
                        throw new InvalidOperationException($"Invalid index: {{_index}}");
                }}
            }}
        """);

        return sb.ToString();
    }
}
