using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

internal sealed class FailureResult<TValue1, TValue2, TValue3> : Result<TValue1, TValue2, TValue3>, IFailureResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
{
    public FailureResult(Exception error, bool attachPrimaryExceptionalReason) {
        if (attachPrimaryExceptionalReason) 
        {    
          AddReason(new ExceptionalError(error));
        }
    }
    
    public FailureResult(IEnumerable<IError> errors) {
        AddReasons(errors);
    }
    
    public FailureResult(Exception error) : this(error, true) {
    }
    
    public override bool IsFaulted => true;
    public override bool IsSuccess => false;
    
    public override void Match(Action success, Action<IEnumerable<IError>> failure) {
        failure(Errors);
    }
    
    public override TOut Match<TOut>(Func<TOut> success, Func<IEnumerable<IError>, TOut> failure) {
        return failure(Errors);
    }
    
    public override void IfSuccess(Action action) {
    }
    
    public override void Match(Action<TValue1, TValue2, TValue3> success, Action<IEnumerable<IError>> failure) {
        failure(Errors);
    }
    
    public override TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TOut> success, Func<IEnumerable<IError>, TOut> failure) {
        return failure(Errors);
    }
    
    public override void IfSuccess(Action<TValue1, TValue2, TValue3> action) {
    }
    
    public override void IfFailure(Action<IEnumerable<IError>> action) {
        action(Errors);
    }
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(false)] out IEnumerable<IError>? error) {
        value1 = default;
        value2 = default;
        value3 = default;
        error = Errors;
        return false;
    }
    
    public override bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3) {
        value1 = default;
        value2 = default;
        value3 = default;
        return false;
    }
    
    public override bool TryGet([NotNullWhen(false)] out IEnumerable<IError>? errors) {
        errors = Errors;
        return false;
    }
    
    public override void Deconstruct(out bool isSuccess, out (TValue1, TValue2, TValue3)? value, out IEnumerable<IError>? error) {
        isSuccess = false;
        value = default;
        error = Errors;
    }
    
}
