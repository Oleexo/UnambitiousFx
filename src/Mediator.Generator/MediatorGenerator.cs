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
    private const string ShortContextAttributeName        = "Context";
    private const string LongRequestHandlerAttributeName  = $"{ShortRequestHandlerAttributeName}Attribute";
    private const string LongEventHandlerAttributeName    = $"{ShortEventHandlerAttributeName}Attribute";
    private const string LongContextAttributeName         = $"{ShortContextAttributeName}Attribute";
    private const string FullRequestHandlerAttributeName  = $"{BaseNamespace}.{LongRequestHandlerAttributeName}";
    private const string FullEventHandlerAttributeName    = $"{BaseNamespace}.{LongEventHandlerAttributeName}";
    private const string FullContextAttributeName         = $"{BaseNamespace}.{LongContextAttributeName}";

    private const string ContextAttributeCode =
        $$"""
          #nullable enable
          namespace {{BaseNamespace}};

          [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
          public sealed class {{LongContextAttributeName}} : Attribute {
          }
          """;

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
        context.RegisterPostInitializationOutput(static x => {
            x.AddSource($"{LongRequestHandlerAttributeName}.g.cs", SourceText.From(RequestHandlerAttributeCode, Encoding.UTF8));
            x.AddSource($"{LongEventHandlerAttributeName}.g.cs",   SourceText.From(EventHandlerAttributeCode,   Encoding.UTF8));
            x.AddSource($"{LongContextAttributeName}.g.cs",        SourceText.From(ContextAttributeCode,        Encoding.UTF8));
        });

        // Get the compilation
        var compilationProvider = context.CompilationProvider;

        // Transform the compilation to extract the root namespace
        var rootNamespaceProvider = compilationProvider
           .Select(static (compilation,
                           _) => compilation.GetRootNamespaceFromAssemblyAttributes());

        var contextDetails = context.SyntaxProvider.ForAttributeWithMetadataName(FullContextAttributeName, static (node,
                                                                                                                   _) => node is ClassDeclarationSyntax
                                                                                     or InterfaceDeclarationSyntax,
                                                                                 GetContextDetail);

        var requestHandlerWithResponseDetails = context.SyntaxProvider.ForAttributeWithMetadataName($"{FullRequestHandlerAttributeName}`2",
                                                                                                    static (node,
                                                                                                            _) => node is ClassDeclarationSyntax,
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
                                                                 .Select(static (tuple,
                                                                                 _) => tuple.Left.AddRange(tuple.Right))
                                                                 .Combine(eventHandlerDetails.Collect())
                                                                 .Select(static (tuple,
                                                                                 _) => tuple.Left.AddRange(tuple.Right));

        var combinedProvider = rootNamespaceProvider
                              .Combine(contextDetails.Collect())
                              .Select((tuple,
                                       _) => {
                                   return (tuple.Left, tuple.Right.FirstOrDefault(x => x.HasValue));
                               })
                              .Combine(allHandlerDetails)
                              .Select((tuple,
                                       _) => (tuple.Left.Left, tuple.Left.Item2, tuple.Right));

        context.RegisterSourceOutput(combinedProvider, static (ctx,
                                                               tuple) => {
            var (rootNamespace, contextDetail, handlerDetails) = tuple;
            ctx.ReportDiagnostic(Diagnostic.Create(
                                     new DiagnosticDescriptor(
                                         "MDG005",
                                         "RegisterGroup generation started",
                                         "RegisterGroup generation started with {0} handlers and root namespace {1}",
                                         "Mediator.Generator",
                                         DiagnosticSeverity.Info,
                                         true),
                                     Location.None,
                                     handlerDetails.Length, rootNamespace));

            if (string.IsNullOrEmpty(rootNamespace)) {
                ctx.ReportDiagnostic(Diagnostic.Create(
                                         new DiagnosticDescriptor(
                                             "MDG001",
                                             "Root namespace not found",
                                             "Root namespace could not be determined. Please ensure assembly has a root namespace defined.",
                                             "Mediator.Generator",
                                             DiagnosticSeverity.Error,
                                             true),
                                         Location.None));
                return;
            }

            if (handlerDetails.Length == 0) {
                ctx.ReportDiagnostic(Diagnostic.Create(
                                         new DiagnosticDescriptor(
                                             "MDG002",
                                             "No handler found",
                                             "No handler found in this assembly. Use RequestHandlerAttribute or EventHandlerAttribute to mark a class as a handler.",
                                             "Mediator.Generator",
                                             DiagnosticSeverity.Info,
                                             true),
                                         Location.None));
            }
            else {
                foreach (var detail in handlerDetails) {
                    if (detail is null) {
                        ctx.ReportDiagnostic(Diagnostic.Create(
                                                 new DiagnosticDescriptor(
                                                     "MDG004",
                                                     "Null handler found",
                                                     "Null handler found in this assembly. Use RequestHandlerAttribute or EventHandlerAttribute to mark a class as a handler.",
                                                     "Mediator.Generator",
                                                     DiagnosticSeverity.Warning,
                                                     true),
                                                 Location.None));
                    }
                    else {
                        ctx.ReportDiagnostic(Diagnostic.Create(
                                                 new DiagnosticDescriptor(
                                                     "MDG003",
                                                     "Handler found",
                                                     $"Handler {detail.Value.ClassName}",
                                                     "Mediator.Generator",
                                                     DiagnosticSeverity.Info,
                                                     true),
                                                 detail.Value.Location ?? Location.None));
                    }
                }
            }

            contextDetail ??= new ContextDetail(AbstractionsNamespace, "IContext");

            ctx.AddSource("RegisterGroup.g.cs", RegisterGroupFactory.Create(rootNamespace, AbstractionsNamespace, contextDetail.Value, handlerDetails));
        });
    }

    private static ContextDetail? GetContextDetail(GeneratorAttributeSyntaxContext ctx,
                                                   CancellationToken               cancellationToken) {
        foreach (var attribute in ctx.Attributes) {
            if (cancellationToken.IsCancellationRequested) {
                return null;
            }

            if (!(attribute.AttributeClass?.Name is LongContextAttributeName or ShortContextAttributeName)) {
                // wrong attribute
                continue;
            }


            var semanticModel = ctx.SemanticModel;
            var symbol        = semanticModel.GetDeclaredSymbol(ctx.TargetNode);
            var parts         = symbol?.ToDisplayParts();
            if (parts is null) {
                return null;
            }

            return new ContextDetail(parts.Value);
        }

        return null;
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

            var location = classDeclaration.GetLocation();
            return new HandlerDetail(HandlerType.RequestHandler, className, @namespace, requestType, responseType, location);
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

            var location = classDeclaration.GetLocation();

            return new HandlerDetail(HandlerType.EventHandler, className, @namespace, requestType, responseType, location);
        }

        return null;
    }

    private static ( string RequestType, string? ResponseType) GetRequestInfo(AttributeData attribute) {
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
