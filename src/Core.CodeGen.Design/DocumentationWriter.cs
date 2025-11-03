using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents XML documentation comments for C# code elements.
/// </summary>
public sealed class DocumentationWriter {
    private readonly IReadOnlyList<string>               _examples;
    private readonly IReadOnlyDictionary<string, string> _exceptions;
    private readonly IReadOnlyDictionary<string, string> _parameters;
    private readonly string?                             _remarks;
    private readonly string?                             _returns;
    private readonly string?                             _summary;
    private readonly IReadOnlyDictionary<string, string> _typeParameters;

    private DocumentationWriter(string?                             summary,
                                IReadOnlyDictionary<string, string> parameters,
                                IReadOnlyDictionary<string, string> typeParameters,
                                string?                             returns,
                                string?                             remarks,
                                IReadOnlyList<string>               examples,
                                IReadOnlyDictionary<string, string> exceptions) {
        _summary        = summary;
        _parameters     = parameters;
        _typeParameters = typeParameters;
        _returns        = returns;
        _remarks        = remarks;
        _examples       = examples;
        _exceptions     = exceptions;
    }

    /// <inheritdoc />
    public void Write(IndentedTextWriter writer) {
        if (string.IsNullOrWhiteSpace(_summary) &&
            _parameters.Count     == 0          &&
            _typeParameters.Count == 0          &&
            string.IsNullOrWhiteSpace(_returns) &&
            string.IsNullOrWhiteSpace(_remarks) &&
            _examples.Count   == 0              &&
            _exceptions.Count == 0) {
            return;
        }

        writer.WriteLine("/// <summary>");
        if (!string.IsNullOrWhiteSpace(_summary)) {
            WriteMultilineDocumentation(writer, _summary, "");
        }

        writer.WriteLine("/// </summary>");

        foreach (var typeParam in _typeParameters) {
            writer.WriteLine($"/// <typeparam name=\"{EscapeXmlAttribute(typeParam.Key)}\">{EscapeXml(typeParam.Value)}</typeparam>");
        }

        foreach (var param in _parameters) {
            writer.WriteLine($"/// <param name=\"{EscapeXmlAttribute(param.Key)}\">{EscapeXml(param.Value)}</param>");
        }

        if (!string.IsNullOrWhiteSpace(_returns)) {
            writer.WriteLine($"/// <returns>{EscapeXml(_returns)}</returns>");
        }

        foreach (var exception in _exceptions) {
            writer.WriteLine($"/// <exception cref=\"{EscapeXmlAttribute(exception.Key)}\">{EscapeXml(exception.Value)}</exception>");
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

    private void WriteMultilineDocumentation(IndentedTextWriter writer,
                                             string             text,
                                             string             prefix) {
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
        if (string.IsNullOrEmpty(text)) {
            return text;
        }

        return text.Replace("&", "&amp;")
                   .Replace("<", "&lt;")
                   .Replace(">", "&gt;");
    }

    private static string EscapeXmlAttribute(string text) {
        if (string.IsNullOrEmpty(text)) {
            return text;
        }

        return text.Replace("&", "&amp;")
                   .Replace("<",  "&lt;")
                   .Replace(">",  "&gt;")
                   .Replace("\"", "&quot;")
                   .Replace("'",  "&apos;");
    }

    /// <summary>
    ///     Creates a new builder for constructing documentation.
    /// </summary>
    /// <returns>A new <see cref="Builder" /> instance.</returns>
    public static Builder Create() {
        return new Builder();
    }

    /// <inheritdoc />
    public sealed class Builder {
        private readonly List<string>               _examples   = new();
        private readonly Dictionary<string, string> _exceptions = new();
        private readonly Dictionary<string, string> _parameters = new();
        private          string?                    _remarks;
        private          string?                    _returns;
        private          string?                    _summary;
        private readonly Dictionary<string, string> _typeParameters = new();

        internal Builder() {
        }

        /// <summary>
        ///     Sets the summary documentation.
        /// </summary>
        public Builder WithSummary(string summary) {
            _summary = summary;
            return this;
        }

        /// <summary>
        ///     Adds a parameter documentation.
        /// </summary>
        public Builder WithParameter(string name,
                                     string description) {
            _parameters[name] = description;
            return this;
        }

        /// <summary>
        ///     Adds multiple parameter documentations.
        /// </summary>
        public Builder WithParameters(params (string name, string description)[] parameters) {
            foreach (var (name, description) in parameters) {
                _parameters[name] = description;
            }

            return this;
        }

        /// <summary>
        ///     Adds a type parameter documentation.
        /// </summary>
        public Builder WithTypeParameter(string name,
                                         string description) {
            _typeParameters[name] = description;
            return this;
        }

        /// <summary>
        ///     Adds multiple type parameter documentations.
        /// </summary>
        public Builder WithTypeParameters(params (string name, string description)[] typeParameters) {
            foreach (var (name, description) in typeParameters) {
                _typeParameters[name] = description;
            }

            return this;
        }

        /// <summary>
        ///     Sets the returns documentation.
        /// </summary>
        public Builder WithReturns(string returns) {
            _returns = returns;
            return this;
        }

        /// <summary>
        ///     Sets the remarks documentation.
        /// </summary>
        public Builder WithRemarks(string remarks) {
            _remarks = remarks;
            return this;
        }

        /// <summary>
        ///     Adds an example documentation.
        /// </summary>
        public Builder WithExample(string example) {
            _examples.Add(example);
            return this;
        }

        /// <summary>
        ///     Adds an exception documentation.
        /// </summary>
        public Builder WithException<T>(string description)
            where T : Exception {
            _exceptions[typeof(T).Name] = description;
            return this;
        }

        /// <summary>
        ///     Adds an exception documentation by type name.
        /// </summary>
        public Builder WithException(string exceptionTypeName,
                                     string description) {
            _exceptions[exceptionTypeName] = description;
            return this;
        }

        /// <summary>
        ///     Adds multiple exception documentations.
        /// </summary>
        public Builder WithExceptions(params (string exceptionTypeName, string description)[] exceptions) {
            foreach (var (exceptionTypeName, description) in exceptions) {
                _exceptions[exceptionTypeName] = description;
            }

            return this;
        }

        /// <summary>
        ///     Builds the documentation writer with the configured settings.
        /// </summary>
        /// <returns>A new <see cref="DocumentationWriter" /> instance.</returns>
        public DocumentationWriter Build() {
            return new DocumentationWriter(
                _summary,
                _parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                _typeParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                _returns,
                _remarks,
                _examples.ToArray(),
                _exceptions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );
        }
    }
}
