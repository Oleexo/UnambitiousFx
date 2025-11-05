using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using FluentResults;
using FluentResult = FluentResults.Result;

namespace CoreBenchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ResultVsFluentResultsBenchmark {
    private const int A = 42;

    private const    string      ErrorMessage = "boom";
    private readonly Result<int> _fluentFailure;
    private readonly Result<int> _fluentSuccess = FluentResult.Ok(A);

    private readonly UnambitiousFx.Core.Results.Result<int> _ourFailure;

    private readonly UnambitiousFx.Core.Results.Result<int> _ourSuccess = UnambitiousFx.Core.Results.Result.Success(A);

    public ResultVsFluentResultsBenchmark() {
        // Use string-based failure to keep both libraries comparable
        _ourFailure    = UnambitiousFx.Core.Results.Result.Failure<int>(ErrorMessage);
        _fluentFailure = FluentResult.Fail<int>(ErrorMessage);
    }

    // Creation (Success)
    [Benchmark(Description = "Create Success (Our)")]
    public UnambitiousFx.Core.Results.Result<int> Create_Success_Our() {
        return UnambitiousFx.Core.Results.Result.Success(A);
    }

    [Benchmark(Description = "Create Success (FluentResults)")]
    public Result<int> Create_Success_Fluent() {
        return FluentResult.Ok(A);
    }

    // Creation (Failure)
    [Benchmark(Description = "Create Failure (Our)")]
    public UnambitiousFx.Core.Results.Result<int> Create_Failure_Our() {
        return UnambitiousFx.Core.Results.Result.Failure<int>(ErrorMessage);
    }

    [Benchmark(Description = "Create Failure (FluentResults)")]
    public Result<int> Create_Failure_Fluent() {
        return FluentResult.Fail<int>(ErrorMessage);
    }

    // Match/Access on Success
    [Benchmark(Description = "Access Success (Our: Match)")]
    public int Access_Success_Our() {
        return _ourSuccess.Match(v => v + 1, _ => 0);
    }

    [Benchmark(Description = "Access Success (FluentResults: IsSuccess/Value)")]
    public int Access_Success_Fluent() {
        return _fluentSuccess.IsSuccess
                   ? _fluentSuccess.Value + 1
                   : 0;
    }

    // Match/Access on Failure
    [Benchmark(Description = "Access Failure (Our: Match)")]
    public int Access_Failure_Our() {
        return _ourFailure.Match(v => v + 1, _ => -1);
    }

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
}
