using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Results;

internal sealed partial class SuccessResult : Result {
    public override bool IsFaulted => false;
    public override bool IsSuccess => true;
    public override void Match(Action success, Action<Exception> failure)
    {
        success();
    }
    public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure)
    {
        return success();
    }
    public override void IfSuccess(Action action)
    {
        action();
    }
    public override void IfFailure(Action<Exception> action)
    {
    }
    public override bool Ok([NotNullWhen(false)] out Exception? error)
    {
        error = null;
        return true;
    }
    public override Result Bind(Func<Result> bind)
    {
        return bind();
    }

    public override Result MapError(Func<Exception, Exception> mapError) {
        return new SuccessResult();
    }

    public override Result Tap(Action action) {
        action();
        return new SuccessResult();
    }

    public override Result TapError(Action<Exception> tapError) {
        return new SuccessResult();
    }

    public override void Deconstruct(out bool isSuccess, out Exception? error)
    {
        isSuccess = true;
        error = null;
    }

    public override string ToString() {
        var metaPart = Metadata.Count == 0 ? string.Empty : " meta=" + string.Join(",", Metadata.Take(2).Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success reasons={Reasons.Count}{metaPart}";
    }
}
