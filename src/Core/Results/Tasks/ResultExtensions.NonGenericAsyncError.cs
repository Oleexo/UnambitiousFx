namespace UnambitiousFx.Core.Results.Tasks;

public static partial class ResultExtensions {
    public static Task<Result> MapErrorAsync(this Result                      result,
                                             Func<Exception, Task<Exception>> map) {
        if (result.Ok(out _)) {
            return Task.FromResult(result);
        }

        result.Ok(out var primary);
        return Await();

        async Task<Result> Await() {
            var mapped  = await map(primary!);
            var failure = Result.Failure(mapped);
            CopyReasonsAndMetadata(result, failure);
            return failure;
        }
    }

    public static Task<Result> TapErrorAsync(this Result           result,
                                             Func<Exception, Task> tap) {
        if (result.Ok(out _)) {
            return Task.FromResult(result);
        }

        result.Ok(out var primary);
        return Await();

        async Task<Result> Await() {
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
