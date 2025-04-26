using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Mediator.Generator;

internal static class RegisterGroupFactory {
    public static SourceText Create(string?                               rootNamespace,
                                    string                                abstractionsNamespace,
                                    ImmutableArray<RequestHandlerDetail?> details) {
        var sb = new StringBuilder();

        sb.AppendLine($"namespace {rootNamespace};");

        sb.AppendLine($"public sealed class RegisterGroup : global::{abstractionsNamespace}.IRegisterGroup");
        sb.AppendLine("{");
        sb.AppendLine($"    public void Register(global::{abstractionsNamespace}.IDependencyInjectionBuilder builder)");
        sb.AppendLine("    {");
        foreach (var detail in details) {
            if (detail is null) {
                continue;
            }

            if (detail.Value.ResponseType is null) {
                sb.AppendLine($"        builder.RegisterRequestHandler<{GlobalizeType(detail.Value.RequestHandlerType)}, {GlobalizeType(detail.Value.RequestType)}>();");
            }
            else {
                sb.AppendLine(
                    $"        builder.RegisterRequestHandler<{GlobalizeType(detail.Value.RequestHandlerType)},{GlobalizeType(detail.Value.RequestType)}, {GlobalizeType(detail.Value.ResponseType)}>();");
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
        return SourceText.From(sb.ToString(), Encoding.UTF8);
    }

    private static string GlobalizeType(string input) {
        if (input.Contains("<")) {
            var genericType = input.Substring(0, input.IndexOf("<", StringComparison.Ordinal));
            var underlyingType = input.Substring(input.IndexOf("<", StringComparison.Ordinal) + 1,
                                                 input.IndexOf(">", StringComparison.Ordinal) - input.IndexOf("<", StringComparison.Ordinal) - 1);
            return $"global::{genericType}<global::{underlyingType}>";
        }

        if (input.StartsWith("global::")) {
            return input;
        }

        return $"global::{input}";
    }
}
