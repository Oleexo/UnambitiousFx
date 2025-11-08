using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed record PublisherOptions
{
    public PublishMode DefaultMode { get; set; } = PublishMode.Now;
}
