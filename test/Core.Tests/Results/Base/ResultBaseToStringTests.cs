using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Base;

public sealed class ResultBaseToStringTests {
    [Fact]
    public void Base_ToString_Success_NoMetadata_NoReasons() {
        var r = new FakeResult(isSuccess: true);
        var s = r.ToString();
        Assert.Equal("Success reasons=0", s);
    }

    [Fact]
    public void Base_ToString_Failure_NoReasons_NoMetadata() {
        var r = new FakeResult(isSuccess: false);
        var s = r.ToString();
        // When failure and no reasons, the base implementation falls back to generic header
        Assert.Equal("Failure(Failure: Error) reasons=0", s);
    }

    private sealed class FakeResult : Result {
        private readonly bool _isSuccess;

        public FakeResult(bool isSuccess) {
            _isSuccess = isSuccess;
        }

        public override bool IsFaulted => !_isSuccess;
        public override bool IsSuccess => _isSuccess;

        public override void Match(Action success, Action<Exception> failure) {
            if (_isSuccess) success(); else failure(new Exception("x"));
        }

        public override TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) {
            return _isSuccess ? success() : failure(new Exception("x"));
        }

        public override void IfSuccess(Action action) { if (_isSuccess) action(); }
        public override void IfFailure(Action<Exception> action) { if (!_isSuccess) action(new Exception("x")); }
        public override bool Ok([System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) { error = _isSuccess ? null : new Exception("x"); return _isSuccess; }

        public override void Deconstruct(out bool isSuccess, out Exception? error) { isSuccess = _isSuccess; error = _isSuccess ? null : new Exception("x"); }
    }
}
