using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Results;

internal static class ResultArityHelpers
{
    public static GenericParameter[] MakeGenericParams(int arity)
    {
        return Enumerable.Range(1, arity)
                         .Select(i => new GenericParameter($"TValue{i}", "notnull"))
                         .ToArray();
    }

    public static string JoinValueTypes(int arity)
    {
        return string.Join(", ", Enumerable.Range(1, arity)
                                           .Select(i => $"TValue{i}")
                                           .ToArray());
    }

    public static string GetOrdinalName(int position)
    {
        return position switch
        {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }
}
