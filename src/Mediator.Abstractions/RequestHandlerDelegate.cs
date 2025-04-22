using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a delegate that handles a request and returns a response
/// </summary>
/// <typeparam name="TResponse">The type of response</typeparam>
/// <returns>A task containing the response</returns>
public delegate ValueTask<IResult<TResponse>> RequestHandlerDelegate<TResponse>()
    where TResponse : notnull;

public delegate ValueTask<IResult> RequestHandlerDelegate();
