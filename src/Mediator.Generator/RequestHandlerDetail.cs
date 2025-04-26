namespace UnambitiousFx.Mediator.Generator;

public readonly record struct RequestHandlerDetail {
    public RequestHandlerDetail(string  className,
                                string  @namespace,
                                string  requestType,
                                string? responseType) {
        ClassName    = className;
        Namespace    = @namespace;
        RequestType  = requestType;
        ResponseType = responseType;
    }

    public string  ClassName          { get; }
    public string  Namespace          { get; }
    public string  RequestType        { get; }
    public string? ResponseType       { get; }
    public string  RequestHandlerType => $"{Namespace}.{ClassName}";
}
