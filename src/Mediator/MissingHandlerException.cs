namespace Oleexo.UnambitiousFx.Mediator;

public sealed class MissingHandlerException : Exception {
    public MissingHandlerException(Type requestType)
        : base($"Missing handler for {requestType.Name}") {
    }
}
