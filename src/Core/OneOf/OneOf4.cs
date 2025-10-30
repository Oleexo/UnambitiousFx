#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
/// Minimal discriminated union base abstraction representing a value that can be one of 4 types.
/// Specific semantic unions (e.g. Either&lt;TLeft,TRight&gt;) can inherit from this to express intent
/// without duplicating core shape.
/// </summary>
/// <typeparam name="TFirst">First possible contained type.</typeparam>
/// <typeparam name="TSecond">Second possible contained type.</typeparam>
/// <typeparam name="TThird">Third possible contained type.</typeparam>
/// <typeparam name="TFourth">Fourth possible contained type.</typeparam>
public abstract class OneOf<TFirst, TSecond, TThird, TFourth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
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
    /// True when the instance currently holds a value of the third type.
    /// </summary>
    public abstract bool IsThird { get; }
    /// <summary>
    /// True when the instance currently holds a value of the fourth type.
    /// </summary>
    public abstract bool IsFourth { get; }
    
    /// <summary>
    /// Pattern match returning a value.
    /// </summary>
    /// <typeparam name="TOut">The type of value to return</typeparam>
    /// <param name="firstFunc">Function to invoke when value is of type TFirst</param>
    /// <param name="secondFunc">Function to invoke when value is of type TSecond</param>
    /// <param name="thirdFunc">Function to invoke when value is of type TThird</param>
    /// <param name="fourthFunc">Function to invoke when value is of type TFourth</param>
    /// <returns>The result of invoking the appropriate function</returns>
    public abstract TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc);
    
    /// <summary>
    /// Pattern match executing side-effect actions.
    /// </summary>
    /// <param name="firstAction">Action to invoke when value is of type TFirst</param>
    /// <param name="secondAction">Action to invoke when value is of type TSecond</param>
    /// <param name="thirdAction">Action to invoke when value is of type TThird</param>
    /// <param name="fourthAction">Action to invoke when value is of type TFourth</param>
    public abstract void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction);
    
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
    /// Attempts to extract the third value.
    /// </summary>
    /// <param name="third">The third value if present</param>
    /// <returns>True if the value is of type TThird, false otherwise</returns>
    public abstract bool Third([NotNullWhen(true)] out TThird? third);
    
    /// <summary>
    /// Attempts to extract the fourth value.
    /// </summary>
    /// <param name="fourth">The fourth value if present</param>
    /// <returns>True if the value is of type TFourth, false otherwise</returns>
    public abstract bool Fourth([NotNullWhen(true)] out TFourth? fourth);
    
    /// <summary>
    /// Creates a OneOf instance containing a first value.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird, TFourth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a second value.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird, TFourth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a third value.
    /// </summary>
    /// <param name="value">The third value</param>
    /// <returns>A OneOf instance containing the third value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird, TFourth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a fourth value.
    /// </summary>
    /// <param name="value">The fourth value</param>
    /// <returns>A OneOf instance containing the fourth value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth> FromFourth(TFourth value) {
        return new FourthOneOf<TFirst, TSecond, TThird, TFourth>(value);
    }
    
    /// <summary>
    /// Implicit conversion from first type to OneOf.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TFirst value) {
        return FromFirst(value);
    }
    
    /// <summary>
    /// Implicit conversion from second type to OneOf.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TSecond value) {
        return FromSecond(value);
    }
    
    /// <summary>
    /// Implicit conversion from third type to OneOf.
    /// </summary>
    /// <param name="value">The third value</param>
    /// <returns>A OneOf instance containing the third value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TThird value) {
        return FromThird(value);
    }
    
    /// <summary>
    /// Implicit conversion from fourth type to OneOf.
    /// </summary>
    /// <param name="value">The fourth value</param>
    /// <returns>A OneOf instance containing the fourth value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TFourth value) {
        return FromFourth(value);
    }
    
}
