using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

internal sealed class FailureResult : Result, IFailureResult {
    public FailureResult(Exception error,
                         bool      attachPrimaryExceptionalReason = true) {
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(IEnumerable<IError> errors) {
        AddReasons(errors);
    }

    public FailureResult(string message)
        : this(new Exception(message)) {
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action                      success,
                               Action<IEnumerable<IError>> failure) {
        failure(Errors);
    }

    public override TOut Match<TOut>(Func<TOut>                      success,
                                     Func<IEnumerable<IError>, TOut> failure) {
        return failure(Errors);
    }

    public override bool TryGet([NotNullWhen(false)] out IEnumerable<IError>? errors) {
        errors = Errors;
        return false;
    }

    public override void IfSuccess(Action action) {
    }

    public override void IfFailure(Action<IEnumerable<IError>> action) {
        action(Errors);
    }

    public override void Deconstruct(out IEnumerable<IError>? error) {
        error = Errors;
    }
}
