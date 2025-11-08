using System.Runtime.CompilerServices;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

internal sealed class Sender : ISender
{
    private readonly IDependencyResolver _resolver;

    public Sender(IDependencyResolver resolver)
    {
        _resolver = resolver;
    }

    public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TRequest request,
                                                                       CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
    {
        var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return handler.HandleAsync(request, cancellationToken);
    }

    public ValueTask<Result> SendAsync<TRequest>(TRequest request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var handler = _resolver.GetRequiredService<IRequestHandler<TRequest>>();
        var result = handler.HandleAsync(request, cancellationToken);
        return result;
    }

    public async IAsyncEnumerable<Result<TItem>> SendStreamAsync<TRequest, TItem>(TRequest request,
                                                                                  [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TRequest : IStreamRequest<TItem>
        where TItem : notnull
    {
        var handler = _resolver.GetRequiredService<IStreamRequestHandler<TRequest, TItem>>();
        await foreach (var item in handler.HandleAsync(request, cancellationToken))
        {
            yield return item;
        }
    }
}
