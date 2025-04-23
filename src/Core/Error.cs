namespace Oleexo.UnambitiousFx.Core;

public record Error : IError {
    private Dictionary<string, string>? _additionalInfo;
    private IEnumerable<IError>?        _children;

    public Error(string message) {
        Message = message;
    }

    public Error(string message,
                 string code) {
        Message = message;
        Code    = code;
    }

    public Error(Exception exception)
        : this(exception.Message) {
        Exception = exception;
    }

    public IEnumerable<IError> Children => _children ??= [];

    public string Message { get; }
    public string Code    { get; } = string.Empty;

    public Exception?                 Exception      { get; }
    public Dictionary<string, string> AdditionalInfo => _additionalInfo ??= new Dictionary<string, string>();

    public void AddChildren(IEnumerable<IError> errors) {
        _children = _children is null
                        ? errors
                        : _children.Concat(errors);
    }

    public static implicit operator Error(Exception exception) {
        return new Error(exception);
    }
}
