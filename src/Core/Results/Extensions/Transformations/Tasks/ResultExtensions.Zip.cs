namespace UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

public static class ResultZipExtensions {
    public static async Task<Result<T1, T2>> ZipAsync<T1, T2>(this Task<Result<T1>> awaitableR1,
                                                              Task<Result<T2>>      awaitableR2)
        where T1 : notnull
        where T2 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        return r1.Zip(r2);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, TR>(this Task<Result<T1>> awaitableR1,
                                                              Task<Result<T2>>      awaitableR2,
                                                              Func<T1, T2, TR>      projector)
        where T1 : notnull
        where T2 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        return r1.Zip(r2, projector);
    }

    public static async Task<Result<T1, T2, T3>> ZipAsync<T1, T2, T3>(this Task<Result<T1>> awaitableR1,
                                                                      Task<Result<T2>>      awaitableR2,
                                                                      Task<Result<T3>>      awaitableR3)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        return r1.Zip(r2, r3);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, TR>(this Task<Result<T1>> awaitableR1,
                                                                  Task<Result<T2>>      awaitableR2,
                                                                  Task<Result<T3>>      awaitableR3,
                                                                  Func<T1, T2, T3, TR>  projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        return r1.Zip(r2, r3, projector);
    }

    public static async Task<Result<T1, T2, T3, T4>> ZipAsync<T1, T2, T3, T4>(this Task<Result<T1>> awaitableR1,
                                                                              Task<Result<T2>>      awaitableR2,
                                                                              Task<Result<T3>>      awaitableR3,
                                                                              Task<Result<T4>>      awaitableR4)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        return r1.Zip(r2, r3, r4);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, T4, TR>(this Task<Result<T1>>          awaitableR1,
                                                                      Task<Result<T2>>               awaitableR2,
                                                                      Task<Result<T3>>               awaitableR3,
                                                                      Task<Result<T4>>               awaitableR4,
                                                                      Func<T1, T2, T3, T4, TR>       projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        return r1.Zip(r2, r3, r4, projector);
    }

    public static async Task<Result<T1, T2, T3, T4, T5>> ZipAsync<T1, T2, T3, T4, T5>(this Task<Result<T1>> awaitableR1,
                                                                                      Task<Result<T2>>      awaitableR2,
                                                                                      Task<Result<T3>>      awaitableR3,
                                                                                      Task<Result<T4>>      awaitableR4,
                                                                                      Task<Result<T5>>      awaitableR5)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        return r1.Zip(r2, r3, r4, r5);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, T4, T5, TR>(this Task<Result<T1>>              awaitableR1,
                                                                          Task<Result<T2>>                   awaitableR2,
                                                                          Task<Result<T3>>                   awaitableR3,
                                                                          Task<Result<T4>>                   awaitableR4,
                                                                          Task<Result<T5>>                   awaitableR5,
                                                                          Func<T1, T2, T3, T4, T5, TR>       projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        return r1.Zip(r2, r3, r4, r5, projector);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ZipAsync<T1, T2, T3, T4, T5, T6>(this Task<Result<T1>> awaitableR1,
                                                                                              Task<Result<T2>>      awaitableR2,
                                                                                              Task<Result<T3>>      awaitableR3,
                                                                                              Task<Result<T4>>      awaitableR4,
                                                                                              Task<Result<T5>>      awaitableR5,
                                                                                              Task<Result<T6>>      awaitableR6)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        return r1.Zip(r2, r3, r4, r5, r6);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, T4, T5, T6, TR>(this Task<Result<T1>>                  awaitableR1,
                                                                              Task<Result<T2>>                       awaitableR2,
                                                                              Task<Result<T3>>                       awaitableR3,
                                                                              Task<Result<T4>>                       awaitableR4,
                                                                              Task<Result<T5>>                       awaitableR5,
                                                                              Task<Result<T6>>                       awaitableR6,
                                                                              Func<T1, T2, T3, T4, T5, T6, TR>       projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        return r1.Zip(r2, r3, r4, r5, r6, projector);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ZipAsync<T1, T2, T3, T4, T5, T6, T7>(this Task<Result<T1>> awaitableR1,
                                                                                                      Task<Result<T2>>      awaitableR2,
                                                                                                      Task<Result<T3>>      awaitableR3,
                                                                                                      Task<Result<T4>>      awaitableR4,
                                                                                                      Task<Result<T5>>      awaitableR5,
                                                                                                      Task<Result<T6>>      awaitableR6,
                                                                                                      Task<Result<T7>>      awaitableR7)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        var r7 = await awaitableR7;
        return r1.Zip(r2, r3, r4, r5, r6, r7);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, TR>(this Task<Result<T1>>                      awaitableR1,
                                                                                  Task<Result<T2>>                           awaitableR2,
                                                                                  Task<Result<T3>>                           awaitableR3,
                                                                                  Task<Result<T4>>                           awaitableR4,
                                                                                  Task<Result<T5>>                           awaitableR5,
                                                                                  Task<Result<T6>>                           awaitableR6,
                                                                                  Task<Result<T7>>                           awaitableR7,
                                                                                  Func<T1, T2, T3, T4, T5, T6, T7, TR>       projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        var r7 = await awaitableR7;
        return r1.Zip(r2, r3, r4, r5, r6, r7, projector);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<Result<T1>> awaitableR1,
                                                                                                                Task<Result<T2>>      awaitableR2,
                                                                                                                Task<Result<T3>>      awaitableR3,
                                                                                                                Task<Result<T4>>      awaitableR4,
                                                                                                                Task<Result<T5>>      awaitableR5,
                                                                                                                Task<Result<T6>>      awaitableR6,
                                                                                                                Task<Result<T7>>      awaitableR7,
                                                                                                                Task<Result<T8>>      awaitableR8)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        var r7 = await awaitableR7;
        var r8 = await awaitableR8;
        return r1.Zip(r2, r3, r4, r5, r6, r7, r8);
    }

    public static async Task<Result<TR>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, T8, TR>(this Task<Result<T1>>                          awaitableR1,
                                                                                      Task<Result<T2>>                               awaitableR2,
                                                                                      Task<Result<T3>>                               awaitableR3,
                                                                                      Task<Result<T4>>                               awaitableR4,
                                                                                      Task<Result<T5>>                               awaitableR5,
                                                                                      Task<Result<T6>>                               awaitableR6,
                                                                                      Task<Result<T7>>                               awaitableR7,
                                                                                      Task<Result<T8>>                               awaitableR8,
                                                                                      Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>       projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull
        where TR : notnull {
        var r1 = await awaitableR1;
        var r2 = await awaitableR2;
        var r3 = await awaitableR3;
        var r4 = await awaitableR4;
        var r5 = await awaitableR5;
        var r6 = await awaitableR6;
        var r7 = await awaitableR7;
        var r8 = await awaitableR8;
        return r1.Zip(r2, r3, r4, r5, r6, r7, r8, projector);
    }
}
