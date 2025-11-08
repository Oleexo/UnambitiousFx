using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using UnambitiousFx.Core.Results;

namespace CoreBenchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ResultTupleVsMultiBenchmark
{
    private const int A = 1;
    private const int B = 2;
    private const int C = 3;

    private readonly Result<(int, int, int)> _tupleSuccess = Result.Success<(int, int, int)>((A, B, C));
    private readonly Result<int, int, int> _multiSuccess = Result.Success(A, B, C);

    private readonly Exception _error = new("boom");
    private readonly Result<(int, int, int)> _tupleFailure;
    private readonly Result<int, int, int> _multiFailure;

    public ResultTupleVsMultiBenchmark()
    {
        _tupleFailure = Result.Failure<(int, int, int)>(_error);
        _multiFailure = Result.Failure<int, int, int>(_error);
    }

    // Creation (Success)
    [Benchmark(Description = "Create Success (Tuple)")]
    public Result<(int, int, int)> Create_Success_Tuple() => Result.Success<(int, int, int)>((A, B, C));

    [Benchmark(Description = "Create Success (Multi)")]
    public Result<int, int, int> Create_Success_Multi() => Result.Success(A, B, C);

    // Creation (Failure)
    [Benchmark(Description = "Create Failure (Tuple)")]
    public Result<(int, int, int)> Create_Failure_Tuple() => Result.Failure<(int, int, int)>(_error);

    [Benchmark(Description = "Create Failure (Multi)")]
    public Result<int, int, int> Create_Failure_Multi() => Result.Failure<int, int, int>(_error);

    // Match on Success
    [Benchmark(Description = "Match Success (Tuple)")]
    public int Match_Success_Tuple() => _tupleSuccess.Match(v => v.Item1 + v.Item2 + v.Item3, _ => 0);

    [Benchmark(Description = "Match Success (Multi)")]
    public int Match_Success_Multi() => _multiSuccess.Match((x, y, z) => x + y + z, _ => 0);

    // Match on Failure
    [Benchmark(Description = "Match Failure (Tuple)")]
    public int Match_Failure_Tuple() => _tupleFailure.Match(v => v.Item1 + v.Item2 + v.Item3, _ => -1);

    [Benchmark(Description = "Match Failure (Multi)")]
    public int Match_Failure_Multi() => _multiFailure.Match((x, y, z) => x + y + z, _ => -1);

    // Ok on Success
    [Benchmark(Description = "Ok Success (Tuple)")]
    public int Ok_Success_Tuple()
    {
        _tupleSuccess.TryGet(out var value);
        return value.Item1 + value.Item2 + value.Item3;
    }

    [Benchmark(Description = "Ok Success (Multi)")]
    public int Ok_Success_Multi()
    {
        _multiSuccess.TryGet(out var x, out var y, out var z);
        return x + y + z;
    }

    // Ok on Failure
    [Benchmark(Description = "Ok Failure (Tuple)")]
    public bool Ok_Failure_Tuple()
    {
        return _tupleFailure.TryGet(out _);
    }

    [Benchmark(Description = "Ok Failure (Multi)")]
    public bool Ok_Failure_Multi()
    {
        return _multiFailure.TryGet(out _, out _, out _);
    }
}
