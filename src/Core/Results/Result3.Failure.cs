using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Results;

internal sealed class FailureResult<TValue1, TValue2, TValue3> : Result<TValue1, TValue2, TValue3>IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
{
    internal FailureResult(Exception error, bool attachPrimaryExceptionalReason) {
        PrimaryException = error;
        if (attachPrimaryExceptionalReason) {
            AddReason(new ExceptionalError(error));
        }
    }
    
    public FailureResult(Exception error) : this(error, true) {
    }
    
    public Exception PrimaryException { get; }
    public override bool IsFaulted => true;
    public override bool IsSuccess => false;
    
    public override void Match(Action success, Action<Exception> failure) {
        failure(PrimaryException);
    }
    
    public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) {
        return failure(PrimaryException);
    }
    
    public override void IfSuccess(Action action) {
    }
    
    public override void Match(Action<TValue1, TValue2, TValue3> success, Action<Exception> failure) {
        failure(PrimaryException);
    }
    
    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TOut> success, Func<Exception, TOut> failure) {
        return failure(PrimaryException);
    }
    
    public override void IfSuccess(Action<TValue1, TValue2, TValue3> action) {
    }
    
    public override void IfFailure(Action<Exception> action) {
        action(PrimaryException);
    }
    
    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = PrimaryException;
        return false;
    }
    
    public override bool Ok([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(false)] out Exception? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        error = PrimaryException;
        return false;
    }
    
    public override bool Ok([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3) {
        value1 = default;
        value2 = default;
        value3 = default;
        return false;
    }
    
    public override void Deconstruct(out bool isSuccess, out (TValue1, TValue2, TValue3)? value, out Exception? error) {
        isSuccess = false;
        value = null;
        error = PrimaryException;
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
        var typeArgs = GetType().GetGenericArguments();
        var typeList = string.Join(", ", typeArgs.Select(FormatType));
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstAny = Reasons.OfType<IError>()
                              .FirstOrDefault();
        var chosen = firstNonExceptional ?? firstAny;
        var headerType = chosen switch {
            ExceptionalError => PrimaryException.GetType().Name,
            null => PrimaryException.GetType().Name,
            _ => chosen.GetType().Name
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
