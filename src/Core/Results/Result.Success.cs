using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Results;

public abstract partial class Result {
    public static Result<TValue1> Success<TValue1>(TValue1 value1)
        where TValue1 : notnull {
        return new SuccessResult<TValue1>(value1);
    }

    public static Result<TValue1, TValue2> Success<TValue1, TValue2>(TValue1 value1,
                                                                     TValue2 value2)
        where TValue1 : notnull
        where TValue2 : notnull {
        return new SuccessResult<TValue1, TValue2>(value1, value2);
    }

    public static Result<TValue1, TValue2, TValue3> Success<TValue1, TValue2, TValue3>(TValue1 value1,
                                                                                       TValue2 value2,
                                                                                       TValue3 value3)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3>(value1, value2, value3);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Success<TValue1, TValue2, TValue3, TValue4>(TValue1 value1,
                                                                                                         TValue2 value2,
                                                                                                         TValue3 value3,
                                                                                                         TValue4 value4)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Success<TValue1, TValue2, TValue3, TValue4, TValue5>(TValue1 value1,
        TValue2                                                                                                                    value2,
        TValue3                                                                                                                    value3,
        TValue4                                                                                                                    value4,
        TValue5                                                                                                                    value5)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(TValue1 value1,
        TValue2                                                                                                                                      value2,
        TValue3                                                                                                                                      value3,
        TValue4                                                                                                                                      value4,
        TValue5                                                                                                                                      value5,
        TValue6                                                                                                                                      value6)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(TValue1 value1,
        TValue2                                                                                                                                                        value2,
        TValue3                                                                                                                                                        value3,
        TValue4                                                                                                                                                        value4,
        TValue5                                                                                                                                                        value5,
        TValue6                                                                                                                                                        value6,
        TValue7                                                                                                                                                        value7)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        TValue1 value1,
        TValue2 value2,
        TValue3 value3,
        TValue4 value4,
        TValue5 value5,
        TValue6 value6,
        TValue7 value7,
        TValue8 value8)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8);
    }
}

internal sealed class SuccessResult<TValue> : Result<TValue>, ISuccessResult
    where TValue : notnull {
    private readonly TValue _value;

    public SuccessResult(TValue value) {
        _value = value;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue>    success,
                               Action<Exception> failure) {
        success(_value);
    }

    public override TOut Match<TOut>(Func<TValue, TOut>    success,
                                     Func<Exception, TOut> failure) {
        return success(_value);
    }

    public override void IfSuccess(Action<TValue> action) {
        action(_value);
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool       isSuccess,
                                     out TValue?    value,
                                     out Exception? error) {
        isSuccess = true;
        value     = _value;
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out  TValue?    value,
                            [NotNullWhen(false)] out Exception? error) {
        value = _value;
        error = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value) {
        value = _value;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2> : Result<TValue1, TValue2>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;

    public SuccessResult(TValue1 value1,
                         TValue2 value2) {
        _value1 = value1;
        _value2 = value2;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2> success,
                               Action<Exception>        failure) {
        success(_value1, _value2);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TOut> success,
                                     Func<Exception, TOut>        failure) {
        return success(_value1, _value2);
    }

    public override void IfSuccess(Action<TValue1, TValue2> action) {
        action(_value1, _value2);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2) {
        value1 = _value1;
        value2 = _value2;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                isSuccess,
                                     out (TValue1, TValue2)? value,
                                     out Exception?          error) {
        isSuccess = true;
        value     = (_value1, _value2);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3> : Result<TValue1, TValue2, TValue3>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3> success,
                               Action<Exception>                 failure) {
        success(_value1, _value2, _value3);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TOut> success,
                                     Func<Exception, TOut>                 failure) {
        return success(_value1, _value2, _value3);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3> action) {
        action(_value1, _value2, _value3);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                         isSuccess,
                                     out (TValue1, TValue2, TValue3)? value,
                                     out Exception?                   error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4> : Result<TValue1, TValue2, TValue3, TValue4>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3,
                         TValue4 value4) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4> success,
                               Action<Exception>                          failure) {
        success(_value1, _value2, _value3, _value4);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TOut> success,
                                     Func<Exception, TOut>                          failure) {
        return success(_value1, _value2, _value3, _value4);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4> action) {
        action(_value1, _value2, _value3, _value4);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                                  isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4)? value,
                                     out Exception?                            error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3, _value4);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}, {_value4}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5> : Result<TValue1, TValue2, TValue3, TValue4, TValue5>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3,
                         TValue4 value4,
                         TValue5 value5) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5> success,
                               Action<Exception>                                   failure) {
        success(_value1, _value2, _value3, _value4, _value5);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut> success,
                                     Func<Exception, TOut>                                   failure) {
        return success(_value1, _value2, _value3, _value4, _value5);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5> action) {
        action(_value1, _value2, _value3, _value4, _value5);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                                           isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5)? value,
                                     out Exception?                                     error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3, _value4, _value5);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}, {_value4}, {_value5}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;
    private readonly TValue6 _value6;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3,
                         TValue4 value4,
                         TValue5 value5,
                         TValue6 value6) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
        _value6 = value6;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success,
                               Action<Exception>                                            failure) {
        success(_value1, _value2, _value3, _value4, _value5, _value6);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut> success,
                                     Func<Exception, TOut>                                            failure) {
        return success(_value1, _value2, _value3, _value4, _value5, _value6);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action) {
        action(_value1, _value2, _value3, _value4, _value5, _value6);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(true)] out  TValue6?   value6,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                                                    isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)? value,
                                     out Exception?                                              error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3, _value4, _value5, _value6);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}, {_value4}, {_value5}, {_value6}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>
    : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;
    private readonly TValue6 _value6;
    private readonly TValue7 _value7;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3,
                         TValue4 value4,
                         TValue5 value5,
                         TValue6 value6,
                         TValue7 value7) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
        _value6 = value6;
        _value7 = value7;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success,
                               Action<Exception>                                                     failure) {
        success(_value1, _value2, _value3, _value4, _value5, _value6, _value7);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut> success,
                                     Func<Exception, TOut>                                                     failure) {
        return success(_value1, _value2, _value3, _value4, _value5, _value6, _value7);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action) {
        action(_value1, _value2, _value3, _value4, _value5, _value6, _value7);
    }

    public override bool Ok([NotNullWhen(true)] out  TValue1?   value1,
                            [NotNullWhen(true)] out  TValue2?   value2,
                            [NotNullWhen(true)] out  TValue3?   value3,
                            [NotNullWhen(true)] out  TValue4?   value4,
                            [NotNullWhen(true)] out  TValue5?   value5,
                            [NotNullWhen(true)] out  TValue6?   value6,
                            [NotNullWhen(true)] out  TValue7?   value7,
                            [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        value7 = _value7;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6,
                            [NotNullWhen(true)] out TValue7? value7) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        value7 = _value7;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                                                             isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)? value,
                                     out Exception?                                                       error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3, _value4, _value5, _value6, _value7);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}, {_value4}, {_value5}, {_value6}, {_value7}) reasons={Reasons.Count}{metaPart}";
    }
}

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>
    : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull
    where TValue8 : notnull {
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;
    private readonly TValue6 _value6;
    private readonly TValue7 _value7;
    private readonly TValue8 _value8;

    public SuccessResult(TValue1 value1,
                         TValue2 value2,
                         TValue3 value3,
                         TValue4 value4,
                         TValue5 value5,
                         TValue6 value6,
                         TValue7 value7,
                         TValue8 value8) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
        _value6 = value6;
        _value7 = value7;
        _value8 = value8;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return success();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success,
                               Action<Exception>                                                              failure) {
        success(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
    }

    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut> success,
                                     Func<Exception, TOut>                                                              failure) {
        return success(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
    }

    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action) {
        action(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
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
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        value7 = _value7;
        value8 = _value8;
        error  = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue1? value1,
                            [NotNullWhen(true)] out TValue2? value2,
                            [NotNullWhen(true)] out TValue3? value3,
                            [NotNullWhen(true)] out TValue4? value4,
                            [NotNullWhen(true)] out TValue5? value5,
                            [NotNullWhen(true)] out TValue6? value6,
                            [NotNullWhen(true)] out TValue7? value7,
                            [NotNullWhen(true)] out TValue8? value8) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        value7 = _value7;
        value8 = _value8;
        return true;
    }

    public override void IfFailure(Action<Exception> action) {
    }

    public override void Deconstruct(out bool                                                                      isSuccess,
                                     out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? value,
                                     out Exception?                                                                error) {
        isSuccess = true;
        value     = (_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
        error     = null;
    }

    public override bool Ok([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>({_value1}, {_value2}, {_value3}, {_value4}, {_value5}, {_value6}, {_value7}, {_value8}) reasons={Reasons.Count}{metaPart}";
    }
}
