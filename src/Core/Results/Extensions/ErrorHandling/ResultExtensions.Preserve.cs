namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    private static T Preserve<T>(BaseResult original,
                                 T          mapped)
        where T : BaseResult {
        if (!ReferenceEquals(original, mapped)) {
            // If the mapped result already has a primary ExceptionalError (from MapError constructing a new FailureResult)
            // we should NOT duplicate the original primary ExceptionalError; only copy the non-primary / domain reasons.
            // This keeps the reason count stable (e.g., ExceptionalError + ConflictError stays 2 instead of becoming 3).
            var originalIsFailure = !original.Ok(out _);
            var mappedIsFailure   = !mapped.Ok(out _);
            if (originalIsFailure && mappedIsFailure) {
                original.Ok(out var originalPrimaryEx);
                // Detect if mapped already has an ExceptionalError referencing its own primary exception.
                var mappedPrimaryExceptional =
                    mapped.Reasons.FirstOrDefault(r => r is Reasons.ExceptionalError) as Reasons.ExceptionalError;
                if (mappedPrimaryExceptional is not null) {
                    var skipped = false;
                    foreach (var r in original.Reasons) {
                        if (!skipped                         &&
                            r is Reasons.ExceptionalError ex &&
                            originalPrimaryEx != null        &&
                            ReferenceEquals(ex.Exception, originalPrimaryEx)) {
                            skipped = true; // skip copying original primary exceptional reason
                            continue;
                        }

                        mapped.AddReason(r);
                    }
                }
                else {
                    // No primary exceptional in mapped; copy all reasons.
                    foreach (var r in original.Reasons) {
                        mapped.AddReason(r);
                    }
                }
            }
            else {
                // Success shaping (shouldn't generally happen) or mixed states: just copy reasons.
                foreach (var r in original.Reasons) {
                    mapped.AddReason(r);
                }
            }

            foreach (var kv in original.Metadata) {
                mapped.AddMetadata(kv.Key, kv.Value);
            }
        }

        return mapped;
    }
}
