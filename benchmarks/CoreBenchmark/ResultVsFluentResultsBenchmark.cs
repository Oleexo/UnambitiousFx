using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using UnambitiousFx.Core.Results;
using FluentResult = FluentResults.Result;

namespace CoreBenchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ResultVsFluentResultsBenchmark {
    private const int A = 42;

    private readonly Result<int>               _ourSuccess    = Result.Success(A);
    private readonly FluentResults.Result<int> _fluentSuccess = FluentResult.Ok(A);

    private readonly Result<int>               _ourFailure;
    private readonly FluentResults.Result<int> _fluentFailure;

    private const string ErrorMessage = "boom";

    public ResultVsFluentResultsBenchmark() {
        // Use string-based failure to keep both libraries comparable
        _ourFailure    = Result.Failure<int>(ErrorMessage);
        _fluentFailure = FluentResult.Fail<int>(ErrorMessage);
    }

    // Creation (Success)
    [Benchmark(Description = "Create Success (Our)")]
    public Result<int> Create_Success_Our() => Result.Success(A);

    [Benchmark(Description = "Create Success (FluentResults)")]
    public FluentResults.Result<int> Create_Success_Fluent() => FluentResult.Ok(A);

    // Creation (Failure)
    [Benchmark(Description = "Create Failure (Our)")]
    public Result<int> Create_Failure_Our() => Result.Failure<int>(ErrorMessage);

    [Benchmark(Description = "Create Failure (FluentResults)")]
    public FluentResults.Result<int> Create_Failure_Fluent() => FluentResult.Fail<int>(ErrorMessage);

    // Match/Access on Success
    [Benchmark(Description = "Access Success (Our: Match)")]
    public int Access_Success_Our() => _ourSuccess.Match(v => v + 1, _ => 0);

    [Benchmark(Description = "Access Success (FluentResults: IsSuccess/Value)")]
    public int Access_Success_Fluent() {
        return _fluentSuccess.IsSuccess
                   ? _fluentSuccess.Value + 1
                   : 0;
    }

    // Match/Access on Failure
    [Benchmark(Description = "Access Failure (Our: Match)")]
    public int Access_Failure_Our() => _ourFailure.Match(v => v + 1, _ => -1);

    [Benchmark(Description = "Access Failure (FluentResults: IsSuccess/Value)")]
    public int Access_Failure_Fluent() {
        return _fluentFailure.IsSuccess
                   ? _fluentFailure.Value + 1
                   : -1;
    }

    // Ok/TryGet-like on Success
    [Benchmark(Description = "Ok Success (Our)")]
    public int Ok_Success_Our() {
        _ourSuccess.TryGet(out var value);
        return value + 1;
    }

    [Benchmark(Description = "Ok Success (FluentResults: IsSuccess + Value)")]
    public int Ok_Success_Fluent() {
        if (_fluentSuccess.IsSuccess) {
            var value = _fluentSuccess.Value;
            return value + 1;
        }

        return 0;
    }

    // Ok/TryGet-like on Failure
    [Benchmark(Description = "Ok Failure (Our)")]
    public bool Ok_Failure_Our() {
        return _ourFailure.TryGet(out _);
    }

    [Benchmark(Description = "Ok Failure (FluentResults: IsSuccess)")]
    public bool Ok_Failure_Fluent() {
        return _fluentFailure.IsSuccess;
    }

    public void Test() {
        var (result, errors)   = FluentResult.Ok(1);
        var (result2, errors2) = Result.Success(1);
    }
}
