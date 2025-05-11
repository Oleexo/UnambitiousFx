namespace Common.Application;

public class AppContext : IAppContext {
    public Guid           CorrelationId { get; } = Guid.NewGuid();
    public DateTimeOffset OccuredAt     { get; } = DateTimeOffset.UtcNow;
}
