namespace UnambitiousFx.Core.Results;

public partial class ResultExtensions {
    internal static void CopyReasonsAndMetadata(this BaseResult source,
                                                BaseResult      target) {
        target.AddReasons(source.Reasons);
        target.AddMetadata(source.Metadata);
    }
}
