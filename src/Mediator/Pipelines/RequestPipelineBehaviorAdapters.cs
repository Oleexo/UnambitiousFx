using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Pipelines;

internal sealed class RequestTypedBehaviorAdapter<TRequest> : IRequestPipelineBehavior
    where TRequest : IRequest {
    private readonly IRequestPipelineBehavior<TRequest> _inner;

    public RequestTypedBehaviorAdapter(IRequestPipelineBehavior<TRequest> inner) { _inner = inner; }

    public ValueTask<Result> HandleAsync<TReq>(IContext context, TReq request, RequestHandlerDelegate next, CancellationToken cancellationToken = default)
        where TReq : IRequest {
        if (request is TRequest typed) {
            return _inner.HandleAsync(context, typed, next, cancellationToken);
        }
        return next();
    }

    public ValueTask<Result<TResponse>> HandleAsync<TReq, TResponse>(IContext context, TReq request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TReq : IRequest<TResponse> {
        // This typed behavior only applies to requests without response.
        return next();
    }
}

internal sealed class RequestTypedBehaviorAdapter<TRequest, TResponse> : IRequestPipelineBehavior
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly IRequestPipelineBehavior<TRequest, TResponse> _inner;

    public RequestTypedBehaviorAdapter(IRequestPipelineBehavior<TRequest, TResponse> inner) { _inner = inner; }

    public ValueTask<Result> HandleAsync<TReq>(IContext context, TReq request, RequestHandlerDelegate next, CancellationToken cancellationToken = default)
        where TReq : IRequest {
        // This typed behavior only applies to requests with a response.
        return next();
    }

    public ValueTask<Result<TRes>> HandleAsync<TReq, TRes>(IContext context, TReq request, RequestHandlerDelegate<TRes> next, CancellationToken cancellationToken = default)
        where TRes : notnull
        where TReq : IRequest<TRes> {
        if (request is TRequest typed && typeof(TRes) == typeof(TResponse)) {
            // Cast delegates/results through object (safe because we checked type match above).
            var castedNext = (RequestHandlerDelegate<TResponse>)(object)next;
            var vt         = _inner.HandleAsync(context, typed, castedNext, cancellationToken);
            return (ValueTask<Result<TRes>>)(object)vt;
        }
        return next();
    }
}

internal sealed class ConditionalUntypedBehaviorWrapper : IRequestPipelineBehavior {
    private readonly IRequestPipelineBehavior     _inner;
    private readonly Func<IContext, object, bool> _predicate;

    public ConditionalUntypedBehaviorWrapper(IRequestPipelineBehavior inner, Func<IContext, object, bool> predicate) {
        _inner     = inner;
        _predicate = predicate;
    }

    public ValueTask<Result> HandleAsync<TRequest>(IContext context, TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken = default) where TRequest : IRequest {
        if (_predicate(context, request)) {
            return _inner.HandleAsync(context, request, next, cancellationToken);
        }
        return next();
    }

    public ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(IContext context, TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default) where TResponse : notnull where TRequest : IRequest<TResponse> {
        if (_predicate(context, request)) {
            return _inner.HandleAsync(context, request, next, cancellationToken);
        }
        return next();
    }
}

internal sealed class ConditionalTypedBehaviorWrapper : IRequestPipelineBehavior {
    private readonly IRequestPipelineBehavior      _innerAdapter;
    private readonly Func<IContext, object, bool>  _predicate;

    public ConditionalTypedBehaviorWrapper(IRequestPipelineBehavior innerAdapter, Func<IContext, object, bool> predicate) {
        _innerAdapter = innerAdapter;
        _predicate    = predicate;
    }

    public ValueTask<Result> HandleAsync<TReq>(IContext context, TReq request, RequestHandlerDelegate next, CancellationToken cancellationToken = default) where TReq : IRequest {
        if (_predicate(context, request)) {
            return _innerAdapter.HandleAsync(context, request, next, cancellationToken);
        }
        return next();
    }

    public ValueTask<Result<TResponse>> HandleAsync<TReq, TResponse>(IContext context, TReq request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default) where TResponse : notnull where TReq : IRequest<TResponse> {
        if (_predicate(context, request)) {
            return _innerAdapter.HandleAsync(context, request, next, cancellationToken);
        }
        return next();
    }
}
