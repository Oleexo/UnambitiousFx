using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

internal sealed class SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> : Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>, ISuccessResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull
    where TValue8 : notnull
{
    private readonly TValue1 _value1;
    private readonly TValue2 _value2;
    private readonly TValue3 _value3;
    private readonly TValue4 _value4;
    private readonly TValue5 _value5;
    private readonly TValue6 _value6;
    private readonly TValue7 _value7;
    private readonly TValue8 _value8;
    
    public SuccessResult(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7, TValue8 value8) {
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
    
    public override void Match(Action success, Action<IEnumerable<IError>> failure) {
        success();
    }
    
    public override TOut Match<TOut>(Func<TOut> success, Func<IEnumerable<IError>, TOut> failure) {
        return success();
    }
    
    public override void IfSuccess(Action action) {
        action();
    }
    
    public override void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<IEnumerable<IError>> failure) {
        success(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
    }
    
    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut> success, Func<IEnumerable<IError>, TOut> failure) {
        return success(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
    }
    
    public override void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action) {
        action(_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
    }
    
    public override void IfFailure(Action<IEnumerable<IError>> action) {
    }
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6, [NotNullWhen(true)] out TValue7? value7, [NotNullWhen(true)] out TValue8? value8) {
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
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6, [NotNullWhen(true)] out TValue7? value7, [NotNullWhen(true)] out TValue8? value8, [NotNullWhen(false)] out IEnumerable<IError>? errors) {
        value1 = _value1;
        value2 = _value2;
        value3 = _value3;
        value4 = _value4;
        value5 = _value5;
        value6 = _value6;
        value7 = _value7;
        value8 = _value8;
        errors = null;
        return true;
    }
    
    public override bool TryGet([NotNullWhen(false)] out IEnumerable<IError>? errors) {
        errors = Errors;
        return false;
    }
    
    public override void Deconstruct(out bool isSuccess, out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? value, out IEnumerable<IError>? error) {
        isSuccess = true;
        value = (_value1, _value2, _value3, _value4, _value5, _value6, _value7, _value8);
        error = null;
    }
    
}
