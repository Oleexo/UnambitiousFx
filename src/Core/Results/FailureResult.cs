namespace UnambitiousFx.Core.Results;

using System.Linq;
using UnambitiousFx.Core.Results.Reasons;

internal sealed partial class FailureResult : Result, IFailureResult {
    private readonly Exception _error;

    /// <summary>
    /// The primary causal exception for this failure.
    /// </summary>
    public Exception PrimaryException => _error;

    internal FailureResult(Exception error, bool attachPrimaryExceptionalReason) {
        _error = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error) : this(error, true) { }

    public FailureResult(string message) : this(new Exception(message), true) { }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return failure(_error);
    }

    public override void IfSuccess(Action action) {
    }

    public override void IfFailure(Action<Exception> action) {
        action(_error);
    }

    public override bool Ok([System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) {
        error = _error;
        return false;
    }

    public override Result Bind(Func<Result> bind) {
        return new FailureResult(_error); // preserve primary exception and primary reason
    }

    public override Result MapError(Func<Exception, Exception> mapError) {
        return new FailureResult(mapError(_error));
    }

    public override Result Tap(Action action) {
        return new FailureResult(_error);
    }

    public override Result TapError(Action<Exception> tapError) {
        tapError(_error);
        return new FailureResult(_error);
    }

    public override void Deconstruct(out bool isSuccess, out Exception? error) {
        isSuccess = false;
        error = _error;
    }

    public override string ToString() {
        var firstNonExceptional = Reasons.OfType<IError>().FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>().FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny; // always at least ExceptionalError now
        var headerType = chosen switch {
            ExceptionalError => _error.GetType().Name,
            null => _error.GetType().Name,
            _ => chosen.GetType().Name
        };
        var headerMessage = chosen?.Message ?? _error.Message;
        var codePart = chosen is not null and not ExceptionalError ? " code=" + chosen.Code : string.Empty;
        var metaPart = Metadata.Count == 0 ? string.Empty : " meta=" + string.Join(",", Metadata.Take(2).Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Failure({headerType}: {headerMessage}){codePart} reasons={Reasons.Count}{metaPart}";
    }
}
