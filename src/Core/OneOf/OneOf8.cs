#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
/// Minimal discriminated union base abstraction representing a value that can be one of 8 types.
/// Specific semantic unions (e.g. Either&lt;TLeft,TRight&gt;) can inherit from this to express intent
/// without duplicating core shape.
/// </summary>
/// <typeparam name="TFirst">First possible contained type.</typeparam>
/// <typeparam name="TSecond">Second possible contained type.</typeparam>
/// <typeparam name="TThird">Third possible contained type.</typeparam>
/// <typeparam name="TFourth">Fourth possible contained type.</typeparam>
/// <typeparam name="TFifth">Fifth possible contained type.</typeparam>
/// <typeparam name="TSixth">Sixth possible contained type.</typeparam>
/// <typeparam name="TSeventh">Seventh possible contained type.</typeparam>
/// <typeparam name="TEighth">Eighth possible contained type.</typeparam>
public abstract class OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull
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
    /// True when the instance currently holds a value of the fifth type.
    /// </summary>
    public abstract bool IsFifth { get; }
    /// <summary>
    /// True when the instance currently holds a value of the sixth type.
    /// </summary>
    public abstract bool IsSixth { get; }
    /// <summary>
    /// True when the instance currently holds a value of the seventh type.
    /// </summary>
    public abstract bool IsSeventh { get; }
    /// <summary>
    /// True when the instance currently holds a value of the eighth type.
    /// </summary>
    public abstract bool IsEighth { get; }
    
    /// <summary>
    /// Pattern match returning a value.
    /// </summary>
    /// <typeparam name="TOut">The type of value to return</typeparam>
    /// <param name="firstFunc">Function to invoke when value is of type TFirst</param>
    /// <param name="secondFunc">Function to invoke when value is of type TSecond</param>
    /// <param name="thirdFunc">Function to invoke when value is of type TThird</param>
    /// <param name="fourthFunc">Function to invoke when value is of type TFourth</param>
    /// <param name="fifthFunc">Function to invoke when value is of type TFifth</param>
    /// <param name="sixthFunc">Function to invoke when value is of type TSixth</param>
    /// <param name="seventhFunc">Function to invoke when value is of type TSeventh</param>
    /// <param name="eighthFunc">Function to invoke when value is of type TEighth</param>
    /// <returns>The result of invoking the appropriate function</returns>
    public abstract TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc, Func<TFifth, TOut> fifthFunc, Func<TSixth, TOut> sixthFunc, Func<TSeventh, TOut> seventhFunc, Func<TEighth, TOut> eighthFunc);
    
    /// <summary>
    /// Pattern match executing side-effect actions.
    /// </summary>
    /// <param name="firstAction">Action to invoke when value is of type TFirst</param>
    /// <param name="secondAction">Action to invoke when value is of type TSecond</param>
    /// <param name="thirdAction">Action to invoke when value is of type TThird</param>
    /// <param name="fourthAction">Action to invoke when value is of type TFourth</param>
    /// <param name="fifthAction">Action to invoke when value is of type TFifth</param>
    /// <param name="sixthAction">Action to invoke when value is of type TSixth</param>
    /// <param name="seventhAction">Action to invoke when value is of type TSeventh</param>
    /// <param name="eighthAction">Action to invoke when value is of type TEighth</param>
    public abstract void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction, Action<TFifth> fifthAction, Action<TSixth> sixthAction, Action<TSeventh> seventhAction, Action<TEighth> eighthAction);
    
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
    /// Attempts to extract the fifth value.
    /// </summary>
    /// <param name="fifth">The fifth value if present</param>
    /// <returns>True if the value is of type TFifth, false otherwise</returns>
    public abstract bool Fifth([NotNullWhen(true)] out TFifth? fifth);
    
    /// <summary>
    /// Attempts to extract the sixth value.
    /// </summary>
    /// <param name="sixth">The sixth value if present</param>
    /// <returns>True if the value is of type TSixth, false otherwise</returns>
    public abstract bool Sixth([NotNullWhen(true)] out TSixth? sixth);
    
    /// <summary>
    /// Attempts to extract the seventh value.
    /// </summary>
    /// <param name="seventh">The seventh value if present</param>
    /// <returns>True if the value is of type TSeventh, false otherwise</returns>
    public abstract bool Seventh([NotNullWhen(true)] out TSeventh? seventh);
    
    /// <summary>
    /// Attempts to extract the eighth value.
    /// </summary>
    /// <param name="eighth">The eighth value if present</param>
    /// <returns>True if the value is of type TEighth, false otherwise</returns>
    public abstract bool Eighth([NotNullWhen(true)] out TEighth? eighth);
    
    /// <summary>
    /// Creates a OneOf instance containing a first value.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a second value.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a third value.
    /// </summary>
    /// <param name="value">The third value</param>
    /// <returns>A OneOf instance containing the third value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a fourth value.
    /// </summary>
    /// <param name="value">The fourth value</param>
    /// <returns>A OneOf instance containing the fourth value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFourth(TFourth value) {
        return new FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a fifth value.
    /// </summary>
    /// <param name="value">The fifth value</param>
    /// <returns>A OneOf instance containing the fifth value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFifth(TFifth value) {
        return new FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a sixth value.
    /// </summary>
    /// <param name="value">The sixth value</param>
    /// <returns>A OneOf instance containing the sixth value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSixth(TSixth value) {
        return new SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a seventh value.
    /// </summary>
    /// <param name="value">The seventh value</param>
    /// <returns>A OneOf instance containing the seventh value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSeventh(TSeventh value) {
        return new SeventhOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Creates a OneOf instance containing a eighth value.
    /// </summary>
    /// <param name="value">The eighth value</param>
    /// <returns>A OneOf instance containing the eighth value</returns>
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromEighth(TEighth value) {
        return new EighthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
    
    /// <summary>
    /// Implicit conversion from first type to OneOf.
    /// </summary>
    /// <param name="value">The first value</param>
    /// <returns>A OneOf instance containing the first value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFirst value) {
        return FromFirst(value);
    }
    
    /// <summary>
    /// Implicit conversion from second type to OneOf.
    /// </summary>
    /// <param name="value">The second value</param>
    /// <returns>A OneOf instance containing the second value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSecond value) {
        return FromSecond(value);
    }
    
    /// <summary>
    /// Implicit conversion from third type to OneOf.
    /// </summary>
    /// <param name="value">The third value</param>
    /// <returns>A OneOf instance containing the third value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TThird value) {
        return FromThird(value);
    }
    
    /// <summary>
    /// Implicit conversion from fourth type to OneOf.
    /// </summary>
    /// <param name="value">The fourth value</param>
    /// <returns>A OneOf instance containing the fourth value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFourth value) {
        return FromFourth(value);
    }
    
    /// <summary>
    /// Implicit conversion from fifth type to OneOf.
    /// </summary>
    /// <param name="value">The fifth value</param>
    /// <returns>A OneOf instance containing the fifth value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFifth value) {
        return FromFifth(value);
    }
    
    /// <summary>
    /// Implicit conversion from sixth type to OneOf.
    /// </summary>
    /// <param name="value">The sixth value</param>
    /// <returns>A OneOf instance containing the sixth value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSixth value) {
        return FromSixth(value);
    }
    
    /// <summary>
    /// Implicit conversion from seventh type to OneOf.
    /// </summary>
    /// <param name="value">The seventh value</param>
    /// <returns>A OneOf instance containing the seventh value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSeventh value) {
        return FromSeventh(value);
    }
    
    /// <summary>
    /// Implicit conversion from eighth type to OneOf.
    /// </summary>
    /// <param name="value">The eighth value</param>
    /// <returns>A OneOf instance containing the eighth value</returns>
    public static  implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TEighth value) {
        return FromEighth(value);
    }
    
}
