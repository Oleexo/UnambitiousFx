using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Examples.ConsoleApp.Commands;

public sealed record HighVolumeCommand : IRequest {
    public required string Data { get; init; }
}
