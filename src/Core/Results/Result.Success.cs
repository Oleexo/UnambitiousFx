using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

internal sealed class SuccessResult : Result, ISuccessResult {
    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action                      success,
                               Action<IEnumerable<IError>> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>                      success,
                                     Func<IEnumerable<IError>, TOut> failure) {
        return success();
    }

    public override bool TryGet([NotNullWhen(false)] out IEnumerable<IError>? errors) {
        errors = null;
        return true;
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void IfFailure(Action<IEnumerable<IError>> action) {
    }

    public override void Deconstruct(out bool                 isSuccess,
                                     out IEnumerable<IError>? error) {
        isSuccess = true;
        error     = null;
    }

    public override string ToString() {
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success reasons={Reasons.Count}{metaPart}";
    }
}
