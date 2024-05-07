using DotNext;
namespace DotnextResults;

public class Class1
{

}
public static class FunctionnDeclarations
{
    public static Result<int> Increment(int target) => target + 1;
    public static Result<int> Add(int target1, int target2) => target1 + target2;
    public static Result<int> Divide(int numerator, int denominator) =>
        (numerator, denominator) switch
        {
            (_, 0) => Result.FromException<int>(new ApplicationException("can not divide by 0")),
            _ => numerator / denominator
        };
    public static Result<int> CombinedCalc(this int target1, int target2, int target3)
        => Increment(target1) switch
        {
            { Error: not null } failure1 => failure1,
            { IsSuccessful: true } success => Add(success.Value, target2) switch
            {
                { Error: not null } failure2 => failure2,
                { IsSuccessful: true } success2 => Divide(success2.Value, target3),
                _ => Result.FromException<int>(new ArgumentOutOfRangeException())
            },
            _ => Result.FromException<int>(new ArgumentOutOfRangeException())
        };
}