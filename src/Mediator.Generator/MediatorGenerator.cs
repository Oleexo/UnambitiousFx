using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        // Get the compilation
        var compilationProvider = context.CompilationProvider;

        // Transform the compilation to extract the root namespace
        var rootNamespaceProvider = compilationProvider
           .Select((compilation,
                    _) => compilation.GetRootNamespaceFromAssemblyAttributes());


        var requestHandlerWithResponseDetails = context.SyntaxProvider.ForAttributeWithMetadataName($"{FullRequestHandlerAttributeName}`2", static (node,
                                                                                                        _) => {
                                                                                                        var isClass = node is ClassDeclarationSyntax;

                                                                                                        return isClass;
                                                                                                    },
                                                                                                    static (ctx,
                                                                                                            _) => GetRequestHandlerDetail(ctx));
        var requestHandlerWithoutResponseDetails = context.SyntaxProvider.ForAttributeWithMetadataName($"{FullRequestHandlerAttributeName}`1", static (node,
                                                                                                           _) => {
                                                                                                           var isClass = node is ClassDeclarationSyntax;

                                                                                                           return isClass;
                                                                                                       },
                                                                                                       static (ctx,
                                                                                                               _) => GetRequestHandlerDetail(ctx));
        var eventHandlerDetails = context.SyntaxProvider.ForAttributeWithMetadataName($"{FullEventHandlerAttributeName}`1", static (node,
                                                                                          _) => {
                                                                                          var isClass = node is ClassDeclarationSyntax;

                                                                                          return isClass;
                                                                                      },
                                                                                      static (ctx,
                                                                                              _) => GetEventHandlerDetail(ctx));

        var allHandlerDetails = requestHandlerWithResponseDetails.Collect()
                                                                 .Combine(requestHandlerWithoutResponseDetails.Collect())
                                                                 .Select((tuple,
                                                                          _) => tuple.Left.AddRange(tuple.Right))
                                                                 .Combine(eventHandlerDetails.Collect())
                                                                 .Select((tuple,
                                                                          _) => tuple.Left.AddRange(tuple.Right));

        var combinedProvider = allHandlerDetails.Combine(rootNamespaceProvider);

        context.RegisterSourceOutput(combinedProvider, (ctx,
                                                        tuple) => {
            var (details, rootNamespace) = tuple;
            ctx.AddSource("RegisterGroup.g.cs", RegisterGroupFactory.Create(rootNamespace, AbstractionsNamespace, details));
        });
    }

    private static HandlerDetail? GetRequestHandlerDetail(GeneratorAttributeSyntaxContext ctx) {
        foreach (var attribute in ctx.Attributes) {
            if (!(attribute.AttributeClass?.Name is LongRequestHandlerAttributeName or ShortRequestHandlerAttributeName)) {
                // wrong attribute
                continue;
            }

            if (ctx.TargetNode is not ClassDeclarationSyntax classDeclaration) {
                // not a class
                continue;
            }

            var className  = classDeclaration.Identifier.ValueText;
            var @namespace = classDeclaration.GetNamespace();
            var (requestType, responseType) = GetRequestInfo(attribute);

            return new HandlerDetail(HandlerType.RequestHandler, className, @namespace, requestType, responseType);
        }

        return null;
    }

    private static HandlerDetail? GetEventHandlerDetail(GeneratorAttributeSyntaxContext ctx) {
        foreach (var attribute in ctx.Attributes) {
            if (!(attribute.AttributeClass?.Name is LongEventHandlerAttributeName or ShortEventHandlerAttributeName)) {
                // wrong attribute
                continue;
            }

            if (ctx.TargetNode is not ClassDeclarationSyntax classDeclaration) {
                // not a class
                continue;
            }

            var className  = classDeclaration.Identifier.ValueText;
            var @namespace = classDeclaration.GetNamespace();
            var (requestType, responseType) = GetRequestInfo(attribute);

            return new HandlerDetail(HandlerType.EventHandler, className, @namespace, requestType, responseType);
        }

        return null;
    }

    private static (string RequestType, string? ResponseType) GetRequestInfo(AttributeData attribute) {
        // Get the attribute constructor's type arguments
        var typeArgs = attribute.AttributeClass?.TypeArguments;
        if (typeArgs is null ||
            typeArgs.Value.Length == 0) {
            return (string.Empty, null);
        }


        // Get the fully qualified name of the request type
        var requestType = typeArgs.Value[0]
                                  .ToDisplayString();

        // Check if there's a response type (generic attribute with 2 type parameters)
        string? responseType = null;
        if (typeArgs.Value.Length > 1) {
            responseType = typeArgs.Value[1]
                                   .ToDisplayString();
        }

        return (requestType, responseType);
    }
}
