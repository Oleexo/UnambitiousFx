using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Represents XML documentation comments for C# code elements.
/// </summary>
internal sealed class DocumentationWriter {
    private readonly string?              _summary;
    private readonly Dictionary<string, string> _parameters;
    private readonly Dictionary<string, string> _typeParameters;
    private readonly string?              _returns;
    private readonly string?              _remarks;
    private readonly List<string>         _examples;

    private DocumentationWriter(string?                        summary,
                                Dictionary<string, string>     parameters,
                                Dictionary<string, string>     typeParameters,
                                string?                        returns,
                                string?                        remarks,
                                List<string>                   examples) {
        _summary        = summary;
        _parameters     = parameters;
        _typeParameters = typeParameters;
        _returns        = returns;
        _remarks        = remarks;
        _examples       = examples;
    }

    public void Write(IndentedTextWriter writer) {
        if (string.IsNullOrWhiteSpace(_summary) &&
            _parameters.Count == 0 &&
            _typeParameters.Count == 0 &&
            string.IsNullOrWhiteSpace(_returns) &&
            string.IsNullOrWhiteSpace(_remarks) &&
            _examples.Count == 0) {
            return;
        }

        writer.WriteLine("/// <summary>");
        if (!string.IsNullOrWhiteSpace(_summary)) {
            WriteMultilineDocumentation(writer, _summary, "");
        }
        writer.WriteLine("/// </summary>");

        foreach (var typeParam in _typeParameters) {
            writer.WriteLine($"/// <typeparam name=\"{typeParam.Key}\">{EscapeXml(typeParam.Value)}</typeparam>");
        }

        foreach (var param in _parameters) {
            writer.WriteLine($"/// <param name=\"{param.Key}\">{EscapeXml(param.Value)}</param>");
        }

        if (!string.IsNullOrWhiteSpace(_returns)) {
            writer.WriteLine($"/// <returns>{EscapeXml(_returns)}</returns>");
        }

        if (!string.IsNullOrWhiteSpace(_remarks)) {
            writer.WriteLine("/// <remarks>");
            WriteMultilineDocumentation(writer, _remarks, "");
            writer.WriteLine("/// </remarks>");
        }

        foreach (var example in _examples) {
            writer.WriteLine("/// <example>");
            WriteMultilineDocumentation(writer, example, "");
            writer.WriteLine("/// </example>");
        }
    }

    private void WriteMultilineDocumentation(IndentedTextWriter writer, string text, string prefix) {
        var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        foreach (var line in lines) {
            if (string.IsNullOrEmpty(line)) {
                writer.WriteLine("///");
            }
            else {
                writer.WriteLine($"///{prefix} {EscapeXml(line)}");
            }
        }
    }

    private static string EscapeXml(string text) {
        return text.Replace("&", "&amp;")
                   .Replace("<", "&lt;")
                   .Replace(">", "&gt;")
                   .Replace("\"", "&quot;")
                   .Replace("'", "&apos;");
    }

    public static Builder Create() => new Builder();

    public sealed class Builder {
        private string?                        _summary;
        private Dictionary<string, string>     _parameters     = new();
        private Dictionary<string, string>     _typeParameters = new();
        private string?                        _returns;
        private string?                        _remarks;
        private List<string>                   _examples       = new();

        internal Builder() { }

        /// <summary>
        /// Sets the summary documentation.
        /// </summary>
        public Builder WithSummary(string summary) {
            _summary = summary;
            return this;
        }

        /// <summary>
        /// Adds a parameter documentation.
        /// </summary>
        public Builder WithParameter(string name, string description) {
            _parameters[name] = description;
            return this;
        }

        /// <summary>
        /// Adds multiple parameter documentations.
        /// </summary>
        public Builder WithParameters(params (string name, string description)[] parameters) {
            foreach (var (name, description) in parameters) {
                _parameters[name] = description;
            }
            return this;
        }

        /// <summary>
        /// Adds a type parameter documentation.
        /// </summary>
        public Builder WithTypeParameter(string name, string description) {
            _typeParameters[name] = description;
            return this;
        }

        /// <summary>
        /// Adds multiple type parameter documentations.
        /// </summary>
        public Builder WithTypeParameters(params (string name, string description)[] typeParameters) {
            foreach (var (name, description) in typeParameters) {
                _typeParameters[name] = description;
            }
            return this;
        }

        /// <summary>
        /// Sets the returns documentation.
        /// </summary>
        public Builder WithReturns(string returns) {
            _returns = returns;
            return this;
        }

        /// <summary>
        /// Sets the remarks documentation.
        /// </summary>
        public Builder WithRemarks(string remarks) {
            _remarks = remarks;
            return this;
        }

        /// <summary>
        /// Adds an example documentation.
        /// </summary>
        public Builder WithExample(string example) {
            _examples.Add(example);
            return this;
        }

        public DocumentationWriter Build() {
            return new DocumentationWriter(
                _summary,
                _parameters,
                _typeParameters,
                _returns,
                _remarks,
                _examples
            );
        }
    }
}
