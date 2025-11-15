using BenchmarkDotNet.Running;

// Run benchmarks comparing UnambitiousFx Mediator vs MediatR
BenchmarkRunner.Run<MediatorBenchmark.MediatorVsMediatRBenchmarks>();
