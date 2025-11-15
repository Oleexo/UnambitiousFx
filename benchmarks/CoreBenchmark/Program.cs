using BenchmarkDotNet.Running;
using CoreBenchmark;

BenchmarkSwitcher.FromTypes(new[]
{
    typeof(ResultTupleVsMultiBenchmark),
    typeof(ResultVsFluentResultsBenchmark)
}).Run(args);
