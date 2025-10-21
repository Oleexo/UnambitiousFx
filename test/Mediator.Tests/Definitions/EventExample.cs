using System.Diagnostics;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

[DebuggerDisplay("{Name}")]
public sealed record EventExample(string Name) : IEvent;
