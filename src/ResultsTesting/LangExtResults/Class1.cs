using LanguageExt.Common;
namespace ClassLibrary1;

public class Class1
{

}

public static class FunctionDeclarations
{
    public static Result<int> Increment(int target) => target + 1;
    public static Result<int> Add(int target1, int target2) => target1 + target2;
    public static Result<int> Divide(int numerator, int denominator) =>
        (numerator, denominator) switch
        {
            (_, 0) => new Result<int>(new ApplicationException("Can not divide by 0")),
            _ => numerator / denominator
        };
    public static Result<int> CombinedCalc(int target1, int target2, int target3)
        => Increment(target1)
            .Match(
                (value) => Add(value, target2)
                    .Match(
                        (value2) => Divide(value2, target1),
                        (exception) => new Result<int>(exception)
                    ),
                (exception) => new Result<int>(exception)
            );
}