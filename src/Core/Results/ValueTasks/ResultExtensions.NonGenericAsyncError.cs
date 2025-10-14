namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result> MapErrorAsync(this Result                           result,
                                                  Func<Exception, ValueTask<Exception>> map) {
        if (result.Ok(out _)) {
            return new ValueTask<Result>(result);
        }

        result.Ok(out var primary);
        return Await();

        async ValueTask<Result> Await() {
            var mapped  = await map(primary!);
            var failure = Result.Failure(mapped);
            CopyReasonsAndMetadata(result, failure);
            return failure;
        }
    }

    public static ValueTask<Result> TapErrorAsync(this Result                result,
                                                  Func<Exception, ValueTask> tap) {
        if (result.Ok(out _)) {
            return new ValueTask<Result>(result);
        }

        result.Ok(out var primary);
        return Await();

        async ValueTask<Result> Await() {
            await tap(primary!);
            return result;
        }
    }

    private static void CopyReasonsAndMetadata(BaseResult from,
                                               BaseResult to) {
        foreach (var r in from.Reasons) {
            to.AddReason(r);
        }

        foreach (var kv in from.Metadata) {
            to.AddMetadata(kv.Key, kv.Value);
        }
    }
}
