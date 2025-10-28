using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
/// Minimal discriminated union base abstraction representing a value that can be one of 2 types.
/// Specific semantic unions (e.g. Either&lt;TLeft,TRight&gt;) can inherit from this to express intent
/// without duplicating core shape.
/// </summary>
/// <typeparam name="TFirst">First possible contained type.</typeparam>
/// <typeparam name="TSecond">Second possible contained type.</typeparam>
public abstract class OneOf<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull
{
    /// <summary>
    /// True when the instance currently holds a value of the first type.
    /// </summary>
    public abstract bool IsFirst { get; }
    /// <summary>
    /// True when the instance currently holds a value of the second type.
    /// </summary>
    public abstract bool IsSecond { get; }
    
    /// <summary>
    /// Pattern match returning a value.
    /// </summary>
    /// <typeparam name="TOut">The type of value to return</typeparam>
    /// <param name="firstFunc">Function to invoke when value is of type TFirst</param>
    /// <param name="secondFunc">Function to invoke when value is of type TSecond</param>
    /// <returns>The result of invoking the appropriate function</returns>
    public abstract TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc);
    
    /// <summary>
    /// Pattern match executing side-effect actions.
    /// </summary>
    /// <param name="firstAction">Action to invoke when value is of type TFirst</param>
    /// <param name="secondAction">Action to invoke when value is of type TSecond</param>
    public abstract void Match(Action<TFirst> firstAction, Action<TSecond> secondAction);
    
    /// <summary>
    /// Attempts to extract the first value.
    /// </summary>
    /// <param name="first">The first value if present</param>
    /// <returns>True if the value is of type TFirst, false otherwise</returns>
    public abstract bool First([NotNullWhen(true)] out TFirst? first);
    
    /// <summary>
    /// Attempts to extract the second value.
    /// </summary>
    /// <param name="second">The second value if present</param>
    /// <returns>True if the value is of type TSecond, false otherwise</returns>
    public abstract bool Second([NotNullWhen(true)] out TSecond? second);
    
    /// <summary>
    /// Creates a OneOf instance containing a first value.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static OneOf<TFirst, TSecond> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a second value.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static OneOf<TFirst, TSecond> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond>(value);
    }
    
    /// <summary>
    /// Implicit conversion from first type to OneOf.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static  implicit operator OneOf<TFirst, TSecond>(TFirst value) {
        return FromFirst(value);
    }
    
    /// <summary>
    /// Implicit conversion from second type to OneOf.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static  implicit operator OneOf<TFirst, TSecond>(TSecond value) {
        return FromSecond(value);
    }
    
}
