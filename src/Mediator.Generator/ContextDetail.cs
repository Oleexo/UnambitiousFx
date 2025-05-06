using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace UnambitiousFx.Mediator.Generator;

public readonly record struct ContextDetail {
    public ContextDetail(ImmutableArray<SymbolDisplayPart> parts) {
        ClassName = [..parts.Select(x => x.ToString())];
    }

    public ContextDetail(string @namespace,
                         string className) {
        ClassName = [@namespace, ".", className];
    }

    public ImmutableArray<string> ClassName { get; }
}
