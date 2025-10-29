using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Results;

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
{
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;
    private readonly TValue6 _value6;
    
    public SuccessResult(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6) {
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
        _value6 = value6;
    }
    
    public override bool IsFaulted => false;
    public override bool IsSuccess => true;
    
    public override void Match(Action success, Action<Exception> failure) {
        success();
    }
    
    public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) {
        return success();
    }
    
    public override void IfSuccess(Action action) {
        action();
    }
    
    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success, Action<Exception> failure) {
        success(_value1, _value2, _value3, _value4, _value5, _value6);
    }
    
    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut> success, Func<Exception, TOut> failure) {
        return success(_value1, _value2, _value3, _value4, _value5, _value6);
    }
    
    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action) {
        action(_value1, _value2, _value3, _value4, _value5, _value6);
    }
    
    public override void IfFailure(Action<Exception> action) {
    }
    
    public override bool TryGet([NotNullWhen(false)] out Exception? error) {
        error = null;
        return true;
    }
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6, [NotNullWhen(false)] out Exception? error) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        error = null;
        return true;
    }
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        return true;
    }
    
    public override void Deconstruct(out bool isSuccess, out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)? value, out Exception? error) {
        isSuccess = true;
        value = (_value1, _value2, _value3, _value4, _value5, _value6);
        error = null;
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
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" +
                             string.Join(",", Metadata.Take(2)
                                                      .Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Success<{typeList}>(_value1, _value2, _value3, _value4, _value5, _value6) reasons={Reasons.Count}{metaPart}";
    }
    
}
