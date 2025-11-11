using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ContextAccessor(IContextFactory contextFactory) : IContextAccessor
{
    private IContext? _context;

    public IContext Context
    {
        get
        {
            _context ??= contextFactory.Create();
            return _context;
        }
        set => _context = value;
    }
}
