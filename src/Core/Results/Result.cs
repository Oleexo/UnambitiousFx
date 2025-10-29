using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public abstract partial class Result : BaseResult {
    public abstract void Deconstruct(out bool                 isSuccess,
                                     out IEnumerable<IError>? error);

    public override string ToString() {
        if (IsSuccess) {
            var metaPart = Metadata.Count == 0
                               ? string.Empty
                               : " meta=" +
                                 string.Join(",", Metadata.Take(2)
                                                          .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
            return $"Success reasons={Reasons.Count}{metaPart}";
        }

        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var    chosen = firstNonExceptional ?? firstAny;
        string headerType;
        string headerMessage;
        if (chosen is ExceptionalError exErr) {
            var ex = exErr.Exception;
            if (ex is null) {
                headerType    = "Failure";
                headerMessage = "Error";
                return $"Failure({headerType}: {headerMessage})";
            }

            headerType = ex.GetType()
                           .Name;
            headerMessage = ex.Message;
        }
        else if (chosen is not null) {
            headerType = chosen.GetType()
                               .Name;
            headerMessage = chosen.Message;
        }
        else {
            headerType    = "Failure";
            headerMessage = "Error";
        }

        var codePart = chosen is not null and not ExceptionalError
                           ? " code=" + chosen.Code
                           : string.Empty;
        var metaFailurePart = Metadata.Count == 0
                                  ? string.Empty
                                  : " meta=" +
                                    string.Join(",", Metadata.Take(2)
                                                             .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Failure({headerType}: {headerMessage}){codePart} reasons={Reasons.Count}{metaFailurePart}";
    }

    #region Static Success

    public static Result Success() {
        return new SuccessResult();
    }

    #endregion

    #region Static Failure

    public static Result Failure(Exception error) {
        return new FailureResult(error);
    }

    public static Result Failure(IError error) {
        var r = new FailureResult(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result Failure(string message) {
        return new FailureResult(message);
    }

    public static Result Failure(IEnumerable<IError> errors) {
        return new FailureResult(errors);
    }
    #endregion


}
