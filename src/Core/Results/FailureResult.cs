using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

internal sealed class FailureResult : Result, IFailureResult {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason = true) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(string message)
        : this(new Exception(message)) {
    }

    /// <summary>
    ///     The primary causal exception for this failure.
    /// </summary>
    public Exception PrimaryException { get; }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action action) {
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }
    
    public override void Deconstruct(out bool       isSuccess,
                                     out Exception? error) {
        isSuccess = false;
        error     = PrimaryException;
    }

    public override string ToString() {
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny; // always at least ExceptionalError now
        var headerType = chosen switch {
            ExceptionalError => PrimaryException.GetType()
                                                .Name,
            null => PrimaryException.GetType()
                                    .Name,
            _ => chosen.GetType()
                       .Name
        };
        var headerMessage = chosen?.Message ?? PrimaryException.Message;
        var codePart = chosen is not null and not ExceptionalError
                           ? " code=" + chosen.Code
                           : string.Empty;
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Failure({headerType}: {headerMessage}){codePart} reasons={Reasons.Count}{metaPart}";
    }
}
