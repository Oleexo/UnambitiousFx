using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public abstract partial class Result : BaseResult {
    public abstract Result MapError(Func<Exception, Exception> mapError);
    public abstract Result Tap(Action                          action);
    public abstract Result TapError(Action<Exception>          tapError);

    public static Result Success() {
        return new SuccessResult();
    }

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

    public abstract void Deconstruct(out bool       isSuccess,
                                     out Exception? error);

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
}

public abstract class Result<TValue1> : BaseResult
    where TValue1 : notnull {
    public abstract void Match(Action<TValue1>   success,
                               Action<Exception> failure);

    public abstract TOut Match<TOut>(Func<TValue1, TOut>   success,
                                     Func<Exception, TOut> failure);

    public abstract void IfSuccess(Action<TValue1> action);

    /// <summary>Implicitly lifts a value into a successful Result&lt;T&gt;.</summary>
    public static implicit operator Result<TValue1>(TValue1 value) {
        return Result.Success(value);
    }

    public abstract bool Ok([NotNullWhen(true)] out  TValue1?   value,
                            [NotNullWhen(false)] out Exception? error);

    public abstract bool Ok([NotNullWhen(true)] out TValue1? value);

    public abstract void Deconstruct(out bool       isSuccess,
                                     out TValue1?   value,
                                     out Exception? error);
}

public abstract class Result<TValue1, TValue2> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull {
    public abstract void Match(Action<TValue1, TValue2> success,
                               Action<Exception>        failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TOut> success,
                                     Func<Exception, TOut>        failure);

    public abstract void IfSuccess(Action<TValue1, TValue2> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2) value,
                            [NotNullWhen(false)] out Exception?                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2) value);

    public abstract void Deconstruct(out bool                isSuccess,
                                     out (TValue1, TValue2)? value,
                                     out Exception?          error);
}

public abstract class Result<TValue1, TValue2, TValue3> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3> success,
                               Action<Exception>                 failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TOut> success,
                                     Func<Exception, TOut>                 failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2, TValue3 value3) value,
                            [NotNullWhen(false)] out Exception?                                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3) value);

    public abstract void Deconstruct(out bool                         isSuccess,
                                     out (TValue1, TValue2, TValue3)? value,
                                     out Exception?                   error);
}

public abstract class Result<TValue1, TValue2, TValue3, TValue4> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4> success,
                               Action<Exception>                          failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TOut> success,
                                     Func<Exception, TOut>                          failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4) value,
                            [NotNullWhen(false)] out Exception?                                                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4) value);

    public abstract void Deconstruct(out bool                                  isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4)? value,
                                     out Exception?                            error);
}

public abstract class Result<TValue1, TValue2, TValue3, TValue4, TValue5> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5> success,
                               Action<Exception>                                   failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut> success,
                                     Func<Exception, TOut>                                   failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5) value,
                            [NotNullWhen(false)] out Exception?                                                                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5) value);

    public abstract void Deconstruct(out bool                                           isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5)? value,
                                     out Exception?                                     error);
}

public abstract class Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success,
                               Action<Exception>                                            failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut> success,
                                     Func<Exception, TOut>                                            failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6) value,
                            [NotNullWhen(false)] out Exception?                                                                                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6) value);

    public abstract void Deconstruct(out bool                                                    isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)? value,
                                     out Exception?                                              error);
}

public abstract class Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success,
                               Action<Exception>                                                     failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut> success,
                                     Func<Exception, TOut>                                                     failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action);

    public abstract bool Ok(out                      (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7) value,
                            [NotNullWhen(false)] out Exception?                                                                                                       error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7) value);

    public abstract void Deconstruct(out bool                                                             isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)? value,
                                     out Exception?                                                       error);
}

public abstract class Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull
    where TValue8 : notnull {
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success,
                               Action<Exception>                                                              failure);

    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut> success,
                                     Func<Exception, TOut>                                                              failure);

    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7, TValue8 value8) value,
                            [NotNullWhen(false)] out Exception? error);

    public abstract bool Ok(out (TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7, TValue8 value8) value);

    public abstract void Deconstruct(out bool                                                                      isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? value,
                                     out Exception?                                                                error);
}
