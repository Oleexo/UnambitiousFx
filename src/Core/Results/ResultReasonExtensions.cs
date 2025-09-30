using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public static class ResultReasonExtensions {
    public static T WithReason<T>(this T result, IReason reason) where T : BaseResult {
        result.AddReason(reason);
        return result;
    }

    public static T WithReasons<T>(this T result, IEnumerable<IReason> reasons) where T : BaseResult {
        result.AddReasons(reasons);
        return result;
    }

    public static T WithMetadata<T>(this T result, string key, object? value) where T : BaseResult {
        result.AddMetadata(key, value);
        return result;
    }

    public static T WithMetadata<T>(this T result, IReadOnlyDictionary<string, object?> metadata) where T : BaseResult {
        foreach (var kv in metadata) result.AddMetadata(kv.Key, kv.Value);
        return result;
    }

    public static T WithMetadata<T>(this T result, params (string Key, object? Value)[] items) where T : BaseResult {
        foreach (var (k, v) in items) result.AddMetadata(k, v);
        return result;
    }

    public static T WithSuccess<T>(this T result, string message, IReadOnlyDictionary<string, object?>? metadata = null, bool copyMetadata = true) where T : BaseResult {
        var reasonMetadata = metadata ?? new Dictionary<string, object?>();
        var success = new SuccessReason(message, reasonMetadata);
        result.AddReason(success);
        if (copyMetadata) {
            foreach (var kv in reasonMetadata) result.AddMetadata(kv.Key, kv.Value);
        }
        return result;
    }

    public static T WithSuccess<T>(this T result, ISuccess success, bool copyMetadata = true) where T : BaseResult {
        result.AddReason(success);
        if (copyMetadata) {
            foreach (var kv in success.Metadata) result.AddMetadata(kv.Key, kv.Value);
        }
        return result;
    }

    public static T WithError<T>(this T result, IError error, bool copyMetadata = true) where T : BaseResult {
        result.AddReason(error);
        if (copyMetadata) {
            foreach (var kv in error.Metadata) result.AddMetadata(kv.Key, kv.Value);
        }
        return result;
    }

    public static T WithErrors<T>(this T result, IEnumerable<IError> errors, bool copyMetadata = true) where T : BaseResult {
        foreach (var e in errors) result.WithError(e, copyMetadata);
        return result;
    }

    public static T WithError<T>(this T result, string code, string message, System.Exception? exception = null, IReadOnlyDictionary<string, object?>? metadata = null, bool copyMetadata = true) where T : BaseResult {
        var err = new InlineError(code, message, exception, metadata);
        return result.WithError(err, copyMetadata);
    }

    private sealed record InlineError : ErrorBase {
        public InlineError(string code, string message, System.Exception? exception, IReadOnlyDictionary<string, object?>? metadata)
            : base(code, message, exception, metadata) { }
    }
}
