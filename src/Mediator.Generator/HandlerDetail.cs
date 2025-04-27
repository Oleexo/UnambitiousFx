namespace UnambitiousFx.Mediator.Generator;

public readonly record struct HandlerDetail {
    public HandlerDetail(HandlerType handlerType,
                         string      className,
                         string      @namespace,
                         string      fullTargetTypeName,
                         string?     fullResponseType) {
        HandlerType        = handlerType;
        ClassName          = className;
        Namespace          = @namespace;
        FullTargetTypeName = fullTargetTypeName;
        FullResponseType   = fullResponseType;
    }

    public string      ClassName           { get; }
    public string      Namespace           { get; }
    public string      FullTargetTypeName  { get; }
    public string?     FullResponseType    { get; }
    public HandlerType HandlerType         { get; }
    public string      FullHandlerTypeName => $"{Namespace}.{ClassName}";
}
