---
layout: default
title: Streaming requests
parent: Basics
nav_order: 3
---

# Streaming requests

Some operations naturally return a sequence of items (paging, chunks from an API, file lines, etc.). UnambitiousFx.Mediator supports streaming requests so you can consume items as they are produced without buffering the entire result in memory.

This page shows how to:
- Model a streaming request with `IStreamRequest<TItem>`
- Implement a handler with `IStreamRequestHandler<TRequest, TItem>`
- Consume results as an `IAsyncEnumerable<Result<TItem>>` using `await foreach`
- Wire up cancellation and understand error propagation
- Register the handler in DI

All examples are AOT‑friendly and avoid runtime reflection.

## When to use streaming
- You expect many items and want to start processing before the full set is available.
- You want backpressure and fewer allocations vs materializing `List<TItem>`.
- The data source is naturally asynchronous or paged.

## Define a streaming request
```csharp
using UnambitiousFx.Mediator.Abstractions;

public sealed record SearchInvoicesQuery(string CustomerId, int PageSize = 100)
    : IStreamRequest<Invoice>;
```

- `TItem` is the element type produced by the stream, not a page wrapper.

## Implement the streaming handler
A handler yields `Result<TItem>` values. Successful items use `Result.Success(item)`; failures use `Result.Failure(...)`. Consumers can decide how to handle failed items.

```csharp
using System.Runtime.CompilerServices;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

public sealed class SearchInvoicesQueryHandler : IStreamRequestHandler<SearchInvoicesQuery, Invoice>
{
    private readonly IInvoiceRepository _repo;
    public SearchInvoicesQueryHandler(IInvoiceRepository repo) => _repo = repo;

    // Note the EnumeratorCancellation attribute for proper cancellation flow
    public async IAsyncEnumerable<Result<Invoice>> HandleAsync(
        SearchInvoicesQuery request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var page = 1;
        while (true)
        {
            // Fetch the next page; returns IEnumerable<Invoice> or empty when done
            var batch = await _repo.GetPageAsync(
                customerId: request.CustomerId,
                page: page,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            if (batch is null || !batch.Any())
                yield break; // end of stream

            foreach (var invoice in batch)
            {
                // Per-item validation/transform can happen here
                if (invoice.IsArchived)
                {
                    yield return Result.Failure<Invoice>(
                        $"Invoice {invoice.Id} is archived");
                    continue;
                }

                yield return Result.Success(invoice);
            }

            page++;

            // Optional: respect cancellation between pages
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
```

Notes:
- Prefer `async IAsyncEnumerable<Result<TItem>>` to surface per-item errors without throwing.
- Cancellation should be checked during iteration; `EnumeratorCancellation` ensures `await foreach` cancels the producer.

## Consuming a stream
Use `await foreach` to consume items and handle successes/failures individually.

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

public sealed class InvoiceExportService(ISender sender)
{
    public async Task<Result> ExportAsync(string customerId, CancellationToken ct)
    {
        var hasFailures = false;

        await foreach (var item in sender.StreamAsync<SearchInvoicesQuery, Invoice>(
            new SearchInvoicesQuery(customerId), ct))
        {
            item.Match(
                onSuccess: invoice =>
                {
                    // Write invoice to CSV
                    // writer.Write(invoice);
                },
                onFailure: error =>
                {
                    hasFailures = true;
                    // Log/collect errors per item
                    // _logger.LogWarning("Skipping invoice: {Message}", error.Message);
                });
        }

        return hasFailures ? Result.Failure("Some items failed during export") : Result.Success();
    }
}
```

- `ISender.StreamAsync<TRequest, TItem>(TRequest, CancellationToken)` returns `IAsyncEnumerable<Result<TItem>>`.
- You decide whether to stop on first failure or continue.

## DI registration
Register the streaming handler using the mediator configuration.

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

services.AddMediator(cfg =>
{
    cfg.RegisterStreamRequestHandler<SearchInvoicesQueryHandler, SearchInvoicesQuery, Invoice>();
});
```

The registration is AOT‑friendly (no runtime scanning). Handlers and behaviors use scoped lifetime by default.

## Cancellation
- Always accept a `CancellationToken` in the handler and propagate it to I/O.
- Consumers pass the token to `StreamAsync(..., cancellationToken)`.
- Add `[EnumeratorCancellation]` so cancellation interrupts the async iterator promptly.

## Error propagation patterns
- Per‑item errors: yield `Result.Failure<TItem>(...)` for items that couldn’t be produced/validated.
- Fatal errors: if the entire stream cannot proceed, you may `yield break` and optionally log, or throw only for exceptional conditions. Prefer modeling expected failures as `Result`.
- Aggregation: callers can count failures, collect errors, or stop early.

## AOT and performance notes
- Avoid reflection in handlers; keep constructors public and simple.
- Prefer streaming from data sources that support async paging to reduce allocations.
- If applying cross‑cutting concerns to streams, use stream pipeline behaviors (see the Behaviors section once available).

## See also
- Send a request: ./send-request.html
- Publish an event: ./publish-event.html
- Register mediator (DI): ./register-mediator.html
- Behaviors (overview): ../behaviors/index.html
- Glossary: ../glossary.html
