using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using UnambitiousFx.Mediator.Abstractions;
using IRequest = MediatR.IRequest;
using OurSender = UnambitiousFx.Mediator.Abstractions.ISender;
using OurPublisher = UnambitiousFx.Mediator.Abstractions.IPublisher;

namespace MediatorBenchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class MediatorVsMediatRBenchmarks {
    private IServiceProvider _ourBaseSp = default!;
    private IServiceProvider _our1BehSp = default!;
    private IServiceProvider _our3BehSp = default!;

    private IServiceProvider _mrBaseSp = default!;
    private IServiceProvider _mr1BehSp = default!;
    private IServiceProvider _mr3BehSp = default!;

    private OurSender _ourSenderBase = default!;
    private OurSender _ourSender1Beh = default!;
    private OurSender _ourSender3Beh = default!;

    private OurPublisher                                 _ourPublisherBase = default!;
    private UnambitiousFx.Mediator.Abstractions.IContext _ourContextBase   = default!;

    private IMediator _mrMediatorBase = default!;
    private IMediator _mrMediator1Beh = default!;
    private IMediator _mrMediator3Beh = default!;

    private static readonly RequestWithResponse    RrRequest = new(41, 1);
    private static readonly RequestWithoutResponse RqRequest = new();
    private static readonly OurEvent               OurEvt    = new();

    private static readonly MediatRRequestWithResponse    MrRrRequest = new(41, 1);
    private static readonly MediatRRequestWithoutResponse MrRqRequest = new();
    private static readonly MrNotification                MrEvt       = new();

    [GlobalSetup]
    public void GlobalSetup() {
        _ourBaseSp = BuildOurSp(behaviors: 0, handlersCountForEvent: 5);
        _our1BehSp = BuildOurSp(behaviors: 1, handlersCountForEvent: 5);
        _our3BehSp = BuildOurSp(behaviors: 3, handlersCountForEvent: 5);

        _ourSenderBase = _ourBaseSp.GetRequiredService<OurSender>();
        _ourSender1Beh = _our1BehSp.GetRequiredService<OurSender>();
        _ourSender3Beh = _our3BehSp.GetRequiredService<OurSender>();

        _ourPublisherBase = _ourBaseSp.GetRequiredService<OurPublisher>();
        _ourContextBase = _ourBaseSp.GetRequiredService<UnambitiousFx.Mediator.Abstractions.IContextFactory>()
                                    .Create();

        _mrBaseSp = BuildMediatRSp(behaviors: 0, handlersCountForEvent: 5);
        _mr1BehSp = BuildMediatRSp(behaviors: 1, handlersCountForEvent: 5);
        _mr3BehSp = BuildMediatRSp(behaviors: 3, handlersCountForEvent: 5);

        _mrMediatorBase = _mrBaseSp.GetRequiredService<IMediator>();
        _mrMediator1Beh = _mr1BehSp.GetRequiredService<IMediator>();
        _mrMediator3Beh = _mr3BehSp.GetRequiredService<IMediator>();
    }

    private static IServiceProvider BuildOurSp(int behaviors,
                                               int handlersCountForEvent) {
        var services = new ServiceCollection();
        services.AddMediator(cfg => {
            cfg.RegisterRequestHandler<RequestWithResponseHandler, RequestWithResponse, int>();
            cfg.RegisterRequestHandler<RequestWithoutResponseHandler, RequestWithoutResponse>();

            // Event handlers
            cfg.RegisterEventHandler<OurEventHandler1, OurEvent>();
            cfg.RegisterEventHandler<OurEventHandler2, OurEvent>();
            cfg.RegisterEventHandler<OurEventHandler3, OurEvent>();
            cfg.RegisterEventHandler<OurEventHandler4, OurEvent>();
            cfg.RegisterEventHandler<OurEventHandler5, OurEvent>();

            // Request pipeline behaviors (untyped so they apply to both)
            if (behaviors >= 1) cfg.RegisterRequestPipelineBehavior<OurNoOpBehavior1>();
            if (behaviors >= 2) cfg.RegisterRequestPipelineBehavior<OurNoOpBehavior2>();
            if (behaviors >= 3) cfg.RegisterRequestPipelineBehavior<OurNoOpBehavior3>();
        });
        return services.BuildServiceProvider();
    }

    private static IServiceProvider BuildMediatRSp(int behaviors,
                                                   int handlersCountForEvent) {
        var services = new ServiceCollection();
        // MediatR v13 performs a license check that resolves ILoggerFactory; register minimal no-op logging to satisfy DI
        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRRequestWithResponse>());

        // Explicitly register handlers in case assembly scanning misses nested types or file location
        services.AddTransient(typeof(MediatR.IRequestHandler<MediatorBenchmark.MediatorVsMediatRBenchmarks.MediatRRequestWithResponse, int>),
                              typeof(MediatorBenchmark.MediatorVsMediatRBenchmarks.MediatRRequestWithResponseHandler));
        services.AddTransient(typeof(MediatR.IRequestHandler<MediatorBenchmark.MediatorVsMediatRBenchmarks.MediatRRequestWithoutResponse>),
                              typeof(MediatorBenchmark.MediatorVsMediatRBenchmarks.MediatRRequestWithoutResponseHandler));

        // Event handlers
        services.AddTransient<INotificationHandler<MrNotification>, MrNotificationHandler1>();
        services.AddTransient<INotificationHandler<MrNotification>, MrNotificationHandler2>();
        services.AddTransient<INotificationHandler<MrNotification>, MrNotificationHandler3>();
        services.AddTransient<INotificationHandler<MrNotification>, MrNotificationHandler4>();
        services.AddTransient<INotificationHandler<MrNotification>, MrNotificationHandler5>();

        // Pipeline behaviors
        if (behaviors >= 1) {
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithResponse, int>),     typeof(MrNoOpBehavior1_RR));
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithoutResponse, Unit>), typeof(MrNoOpBehavior1_Void));
        }

        if (behaviors >= 2) {
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithResponse, int>),     typeof(MrNoOpBehavior2_RR));
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithoutResponse, Unit>), typeof(MrNoOpBehavior2_Void));
        }

        if (behaviors >= 3) {
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithResponse, int>),     typeof(MrNoOpBehavior3_RR));
            services.AddTransient(typeof(IPipelineBehavior<MediatRRequestWithoutResponse, Unit>), typeof(MrNoOpBehavior3_Void));
        }

        return services.BuildServiceProvider();
    }

    // 1) Send request with response
    [Benchmark(Baseline = true, Description = "Our Mediator - Send (response)")]
    public async Task<int> Our_Send_Response() {
        var res = await _ourSenderBase.SendAsync<RequestWithResponse, int>(RrRequest);
        return res.TryGet(out var v)
                   ? v!
                   : -1;
    }

    [Benchmark(Baseline = true, Description = "Our Mediator - Direct Send (response)")]
    public async Task<int> Our_Direct_Send_Response() {
        var handler        = _ourBaseSp.GetRequiredService<RequestWithResponseHandler>();
        var contextFactory = _ourBaseSp.GetRequiredService<IContextFactory>();
        var ctx            = contextFactory.Create();
        var res            = await handler.HandleAsync(ctx, RrRequest, CancellationToken.None);
        return res.TryGet(out var v)
                   ? v!
                   : -1;
    }

    [Benchmark(Description = "MediatR - Send (response)")]
    public async Task<int> MediatR_Send_Response() {
        return await _mrMediatorBase.Send(MrRrRequest);
    }

    // 2) Send request without response
    [Benchmark(Description = "Our Mediator - Send (void)")]
    public async Task<bool> Our_Send_Void() {
        var res = await _ourSenderBase.SendAsync(RqRequest);
        return res.IsSuccess;
    }

    [Benchmark(Description = "MediatR - Send (void)")]
    public async Task<bool> MediatR_Send_Void() {
        await _mrMediatorBase.Send(MrRqRequest);
        return true;
    }

    // 3) Publish notification with multiple handlers
    [Benchmark(Description = "Our Mediator - Publish (5 handlers)")]
    public async Task<bool> Our_Publish_5Handlers() {
        var res = await _ourPublisherBase.PublishAsync(_ourContextBase, OurEvt);
        return res.IsSuccess;
    }

    [Benchmark(Description = "MediatR - Publish (5 handlers)")]
    public async Task MediatR_Publish_5Handlers() {
        await _mrMediatorBase.Publish(MrEvt);
    }

    // 4) Send with 1 pipeline behavior
    [Benchmark(Description = "Our Mediator - Send (1 behavior)")]
    public async Task<int> Our_Send_Response_1Behavior() {
        var sender = _our1BehSp.GetRequiredService<OurSender>();
        var res    = await sender.SendAsync<RequestWithResponse, int>(RrRequest);
        return res.TryGet(out var v)
                   ? v!
                   : -1;
    }

    [Benchmark(Description = "MediatR - Send (1 behavior)")]
    public async Task<int> MediatR_Send_Response_1Behavior() {
        return await _mrMediator1Beh.Send(MrRrRequest);
    }

    // 5) Send with 3 pipeline behaviors
    [Benchmark(Description = "Our Mediator - Send (3 behaviors)")]
    public async Task<int> Our_Send_Response_3Behaviors() {
        var sender = _our3BehSp.GetRequiredService<OurSender>();
        var res    = await sender.SendAsync<RequestWithResponse, int>(RrRequest);
        return res.TryGet(out var v)
                   ? v!
                   : -1;
    }

    [Benchmark(Description = "MediatR - Send (3 behaviors)")]
    public async Task<int> MediatR_Send_Response_3Behaviors() {
        return await _mrMediator3Beh.Send(MrRrRequest);
    }

    // ===== Types for Our Mediator =====
    public sealed record RequestWithResponse(int A,
                                             int B) : UnambitiousFx.Mediator.Abstractions.IRequest<int>;

    public sealed record RequestWithoutResponse() : UnambitiousFx.Mediator.Abstractions.IRequest;

    public sealed class RequestWithResponseHandler : UnambitiousFx.Mediator.Abstractions.IRequestHandler<RequestWithResponse, int> {
        public ValueTask<Result<int>> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                                  RequestWithResponse request,
                                                  CancellationToken cancellationToken = default) => ValueTask.FromResult(Result.Success(request.A + request.B));
    }

    public sealed class RequestWithoutResponseHandler : UnambitiousFx.Mediator.Abstractions.IRequestHandler<RequestWithoutResponse> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             RequestWithoutResponse                       request,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    public sealed class OurNoOpBehavior1 : UnambitiousFx.Mediator.Abstractions.IRequestPipelineBehavior {
        public ValueTask<Result> HandleAsync<TRequest>(UnambitiousFx.Mediator.Abstractions.IContext               context,
                                                       TRequest                                                   request,
                                                       UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate next,
                                                       CancellationToken                                          cancellationToken = default)
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest => next();

        public ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(UnambitiousFx.Mediator.Abstractions.IContext                          context,
                                                                             TRequest                                                              request,
                                                                             UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate<TResponse> next,
                                                                             CancellationToken                                                     cancellationToken = default)
            where TResponse : notnull
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest<TResponse> => next();
    }

    public sealed class OurNoOpBehavior2 : UnambitiousFx.Mediator.Abstractions.IRequestPipelineBehavior {
        public ValueTask<Result> HandleAsync<TRequest>(UnambitiousFx.Mediator.Abstractions.IContext               context,
                                                       TRequest                                                   request,
                                                       UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate next,
                                                       CancellationToken                                          cancellationToken = default)
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest => next();

        public ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(UnambitiousFx.Mediator.Abstractions.IContext                          context,
                                                                             TRequest                                                              request,
                                                                             UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate<TResponse> next,
                                                                             CancellationToken                                                     cancellationToken = default)
            where TResponse : notnull
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest<TResponse> => next();
    }

    public sealed class OurNoOpBehavior3 : UnambitiousFx.Mediator.Abstractions.IRequestPipelineBehavior {
        public ValueTask<Result> HandleAsync<TRequest>(UnambitiousFx.Mediator.Abstractions.IContext               context,
                                                       TRequest                                                   request,
                                                       UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate next,
                                                       CancellationToken                                          cancellationToken = default)
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest => next();

        public ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(UnambitiousFx.Mediator.Abstractions.IContext                          context,
                                                                             TRequest                                                              request,
                                                                             UnambitiousFx.Mediator.Abstractions.RequestHandlerDelegate<TResponse> next,
                                                                             CancellationToken                                                     cancellationToken = default)
            where TResponse : notnull
            where TRequest : UnambitiousFx.Mediator.Abstractions.IRequest<TResponse> => next();
    }

    public sealed class OurEvent : UnambitiousFx.Mediator.Abstractions.IEvent {
    }

    public sealed class OurEventHandler1 : UnambitiousFx.Mediator.Abstractions.IEventHandler<OurEvent> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             OurEvent                                     @event,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    public sealed class OurEventHandler2 : UnambitiousFx.Mediator.Abstractions.IEventHandler<OurEvent> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             OurEvent                                     @event,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    public sealed class OurEventHandler3 : UnambitiousFx.Mediator.Abstractions.IEventHandler<OurEvent> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             OurEvent                                     @event,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    public sealed class OurEventHandler4 : UnambitiousFx.Mediator.Abstractions.IEventHandler<OurEvent> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             OurEvent                                     @event,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    public sealed class OurEventHandler5 : UnambitiousFx.Mediator.Abstractions.IEventHandler<OurEvent> {
        public ValueTask<Result> HandleAsync(UnambitiousFx.Mediator.Abstractions.IContext context,
                                             OurEvent                                     @event,
                                             CancellationToken                            cancellationToken = default) => ValueTask.FromResult(Result.Success());
    }

    // ===== Types for MediatR =====
    public sealed record MediatRRequestWithResponse(int A,
                                                    int B) : MediatR.IRequest<int>;

    public sealed record MediatRRequestWithoutResponse() : IRequest;

    public sealed class MediatRRequestWithResponseHandler : MediatR.IRequestHandler<MediatRRequestWithResponse, int> {
        public Task<int> Handle(MediatRRequestWithResponse request,
                                CancellationToken          cancellationToken) => Task.FromResult(request.A + request.B);
    }

    public sealed class MediatRRequestWithoutResponseHandler : MediatR.IRequestHandler<MediatRRequestWithoutResponse> {
        public Task Handle(MediatRRequestWithoutResponse request,
                           CancellationToken             cancellationToken) => Task.CompletedTask;
    }

    public sealed class MrNoOpBehavior1_RR : IPipelineBehavior<MediatRRequestWithResponse, int> {
        public Task<int> Handle(MediatRRequestWithResponse          request,
                                MediatR.RequestHandlerDelegate<int> next,
                                CancellationToken                   cancellationToken) => next();
    }

    public sealed class MrNoOpBehavior2_RR : IPipelineBehavior<MediatRRequestWithResponse, int> {
        public Task<int> Handle(MediatRRequestWithResponse          request,
                                MediatR.RequestHandlerDelegate<int> next,
                                CancellationToken                   cancellationToken) => next();
    }

    public sealed class MrNoOpBehavior3_RR : IPipelineBehavior<MediatRRequestWithResponse, int> {
        public Task<int> Handle(MediatRRequestWithResponse          request,
                                MediatR.RequestHandlerDelegate<int> next,
                                CancellationToken                   cancellationToken) => next();
    }

    public sealed class MrNoOpBehavior1_Void : IPipelineBehavior<MediatRRequestWithoutResponse, Unit> {
        public Task<Unit> Handle(MediatRRequestWithoutResponse        request,
                                 MediatR.RequestHandlerDelegate<Unit> next,
                                 CancellationToken                    cancellationToken) => next();
    }

    public sealed class MrNoOpBehavior2_Void : IPipelineBehavior<MediatRRequestWithoutResponse, Unit> {
        public Task<Unit> Handle(MediatRRequestWithoutResponse        request,
                                 MediatR.RequestHandlerDelegate<Unit> next,
                                 CancellationToken                    cancellationToken) => next();
    }

    public sealed class MrNoOpBehavior3_Void : IPipelineBehavior<MediatRRequestWithoutResponse, Unit> {
        public Task<Unit> Handle(MediatRRequestWithoutResponse        request,
                                 MediatR.RequestHandlerDelegate<Unit> next,
                                 CancellationToken                    cancellationToken) => next();
    }

    public sealed class MrNotification : INotification {
    }

    public sealed class MrNotificationHandler1 : INotificationHandler<MrNotification> {
        public Task Handle(MrNotification    notification,
                           CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public sealed class MrNotificationHandler2 : INotificationHandler<MrNotification> {
        public Task Handle(MrNotification    notification,
                           CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public sealed class MrNotificationHandler3 : INotificationHandler<MrNotification> {
        public Task Handle(MrNotification    notification,
                           CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public sealed class MrNotificationHandler4 : INotificationHandler<MrNotification> {
        public Task Handle(MrNotification    notification,
                           CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public sealed class MrNotificationHandler5 : INotificationHandler<MrNotification> {
        public Task Handle(MrNotification    notification,
                           CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
