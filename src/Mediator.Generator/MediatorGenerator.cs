using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace UnambitiousFx.Mediator.Generator;

/// <summary>
///     MediatorGenerator is responsible for generating source code at compile-time
///     as part of the incremental source generation process within the compilation
///     pipeline. It interacts with the Roslyn API and implements the IIncrementalGenerator
///     interface to enable efficient and reusable code generation.
/// </summary>
/// <remarks>
///     This generator is primarily used to process input syntax and semantic information
///     from the compilation, to generate source files dynamically.
/// </remarks>
[Generator]
public class MediatorGenerator : IIncrementalGenerator {
    private const string BaseNamespace                    = "UnambitiousFx.Mediator";
    private const string AbstractionsNamespace            = "UnambitiousFx.Mediator.Abstractions";
    private const string ShortRequestHandlerAttributeName = "RequestHandler";
    private const string ShortEventHandlerAttributeName   = "EventHandler";
    private const string LongRequestHandlerAttributeName  = $"{ShortRequestHandlerAttributeName}Attribute";
    private const string LongEventHandlerAttributeName    = $"{ShortEventHandlerAttributeName}Attribute";
    private const string FullRequestHandlerAttributeName  = $"{BaseNamespace}.{LongRequestHandlerAttributeName}";
    private const string FullEventHandlerAttributeName    = $"{BaseNamespace}.{LongEventHandlerAttributeName}";

    private const string RequestHandlerAttributeCode =
        $$"""
          #nullable enable
          namespace {{BaseNamespace}};

          [AttributeUsage(AttributeTargets.Class)]
          public sealed class {{LongRequestHandlerAttributeName}}<TRequest, TResponse> : Attribute
              where TRequest : global::{{AbstractionsNamespace}}.IRequest<TResponse> 
              where TResponse : notnull {
          }

          [AttributeUsage(AttributeTargets.Class)]
          public sealed class {{LongRequestHandlerAttributeName}}<TRequest> : Attribute
              where TRequest : global::{{AbstractionsNamespace}}.IRequest {
          }
          """;

    private const string EventHandlerAttributeCode =
        $$"""
          #nullable enable
          namespace {{BaseNamespace}};

          [AttributeUsage(AttributeTargets.Class)]
          public sealed class {{LongEventHandlerAttributeName}}<TEvent> : Attribute 
              where TEvent : global::{{AbstractionsNamespace}}.IEvent {
          }
          """;

    /// <summary>
    ///     Initializes the MediatorGenerator by registering post-initialization output with the provided
    ///     <see cref="IncrementalGeneratorInitializationContext" />. This method is called during the
    ///     generator's setup phase to define the generator's behavior, such as adding generated source code.
    /// </summary>
    /// <param name="context">
    ///     The initialization context provided by the Roslyn API. It provides methods and registration points
    ///     that allow the generator to specify how it interacts with the compilation process.
    /// </param>
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.RegisterPostInitializationOutput(x => x.AddSource($"{LongRequestHandlerAttributeName}.g.cs", SourceText.From(RequestHandlerAttributeCode, Encoding.UTF8)));
        context.RegisterPostInitializationOutput(x => x.AddSource($"{LongEventHandlerAttributeName}.g.cs",   SourceText.From(EventHandlerAttributeCode,   Encoding.UTF8)));
    }
}
