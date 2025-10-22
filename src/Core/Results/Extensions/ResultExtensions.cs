namespace UnambitiousFx.Core.Results.Extensions;

public static class ResultExtensions {
    internal static void CopyReasonsAndMetadata(this BaseResult source,
                                                BaseResult      target) {
        target.AddReasons(source.Reasons);
        target.AddMetadata(source.Metadata);
    }
}
