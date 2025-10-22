using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public abstract partial class Result {
    public static Result<TValue1> Failure<TValue1>(Exception error)
        where TValue1 : notnull {
        return new FailureResult<TValue1>(error);
    }

    public static Result<TValue1> Failure<TValue1>(IError error)
        where TValue1 : notnull {
        var r = new FailureResult<TValue1>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1> Failure<TValue1>(string message)
        where TValue1 : notnull {
        return new FailureResult<TValue1>(new Exception(message));
    }

    public static Result<TValue1, TValue2> Failure<TValue1, TValue2>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull {
        return new FailureResult<TValue1, TValue2>(error);
    }

    public static Result<TValue1, TValue2> Failure<TValue1, TValue2>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull {
        var r = new FailureResult<TValue1, TValue2>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3> Failure<TValue1, TValue2, TValue3>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3>(error);
    }

    public static Result<TValue1, TValue2, TValue3> Failure<TValue1, TValue2, TValue3>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Failure<TValue1, TValue2, TValue3, TValue4>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4>(error);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Failure<TValue1, TValue2, TValue3, TValue4>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5>(error);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(error);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(error.Exception ?? new Exception(error.Message),
                                                                                        false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(error);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(error.Exception ?? new Exception(error.Message),
                                                                                                 false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>
        Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(Exception error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(error);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        IError error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(error.Exception ?? new Exception(error.Message),
                                                                                                          false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }
}

internal sealed class FailureResult<TValue> : Result<TValue>, IFailureResult
    where TValue : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue>    success,
                               Action<Exception> failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue, TOut>    success,
                                     Func<Exception, TOut> failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue> action) {
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool       isSuccess,
                                     out TValue?    value,
                                     out Exception? error) {
        isSuccess = false;
        value     = default;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out  TValue?    value,
                            [NotNullWhen(false)] out Exception? error) {
        value = default;
        error = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value) {
        value = default;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2> : Result<TValue1, TValue2>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2> success,
                               Action<Exception>        failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TOut> success,
                                     Func<Exception, TOut>        failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2> action) {
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                isSuccess,
                                     out (TValue1, TValue2)? value,
                                     out Exception?          error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2) {
        value1 = default;
        value2 = default;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3> : Result<TValue1, TValue2, TValue3>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3> success,
                               Action<Exception>                 failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TOut> success,
                                     Func<Exception, TOut>                 failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3) {
        value1 = default;
        value2 = default;
        value3 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                         isSuccess,
                                     out (TValue1, TValue2, TValue3)? value,
                                     out Exception?                   error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3, TValue4> : Result<TValue1, TValue2, TValue3, TValue4>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4> success,
                               Action<Exception>                          failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TOut> success,
                                     Func<Exception, TOut>                          failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                                  isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4)? value,
                                     out Exception?                            error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5> : Result<TValue1, TValue2, TValue3, TValue4, TValue5>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5> success,
                               Action<Exception>                                   failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut> success,
                                     Func<Exception, TOut>                                   failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                                           isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5)? value,
                                     out Exception?                                     error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success,
                               Action<Exception>                                            failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut> success,
                                     Func<Exception, TOut>                                            failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(true)] out  TValue6?   value6,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                                                    isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)? value,
                                     out Exception?                                              error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>
    : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success,
                               Action<Exception>                                                     failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut> success,
                                     Func<Exception, TOut>                                                     failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(true)] out  TValue6?   value6,
                            [NotNullWhen(true)] out  TValue7?   value7,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        value7 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6,
                            [NotNullWhen(true)] out TValue7? value7) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        value7 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                                                             isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)? value,
                                     out Exception?                                                       error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}

internal sealed class FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>
    : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull
    where TValue8 : notnull {
    internal FailureResult(Exception error,
                           bool      attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }

    public FailureResult(Exception error)
        : this(error, true) {
    }

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

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success,
                               Action<Exception>                                                              failure) {
        failure(PrimaryException);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut> success,
                                     Func<Exception, TOut>                                                              failure) {
        return failure(PrimaryException);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action) {
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(true)] out  TValue6?   value6,
                            [NotNullWhen(true)] out  TValue7?   value7,
                            [NotNullWhen(true)] out  TValue8?   value8,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        value7 = default;
        value8 = default;
        error  = PrimaryException;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6,
                            [NotNullWhen(true)] out TValue7? value7,
                            [NotNullWhen(true)] out TValue8? value8) {
        value1 = default;
        value2 = default;
        value3 = default;
        value4 = default;
        value5 = default;
        value6 = default;
        value7 = default;
        value8 = default;
        return false;
    }

    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }

    public override void Deconstruct(out bool                                                                      isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? value,
                                     out Exception?                                                                error) {
        isSuccess = false;
        value     = null;
        error     = PrimaryException;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }

    public override string ToString() {
        string FormatType(Type t) {
            return t == typeof(int)
                       ? "int"
                       : t == typeof(string)
                           ? "string"
                           : t == typeof(bool)
                               ? "bool"
                               : t == typeof(long)
                                   ? "long"
                                   : t == typeof(short)
                                       ? "short"
                                       : t == typeof(byte)
                                           ? "byte"
                                           : t == typeof(char)
                                               ? "char"
                                               : t == typeof(decimal)
                                                   ? "decimal"
                                                   : t == typeof(double)
                                                       ? "double"
                                                       : t == typeof(float)
                                                           ? "float"
                                                           : t == typeof(object)
                                                               ? "object"
                                                               : t.IsGenericType
                                                                   ? t.Name.Substring(0, t.Name.IndexOf('`'))
                                                                   : t.Name;
        }

        var typeArgs = GetType()
           .GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
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
        return $"Failure<{typeList}>({headerType}: {headerMessage}){codePart} reasons={{Reasons.Count}}{metaPart}";
    }
}
