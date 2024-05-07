using System.Text.Json.Serialization;
namespace FunctionalResults;

public class Class1
{
    public void Test1()
    {
        var result = FunctionDeclarations.Increment(1);
        
        if (result is Success<int> success)
        {
            Console.WriteLine($"Success: {success.Value}");
        }
        else if (result is Failure<int> failure)
        {
            Console.WriteLine($"Failure: {failure.Exception.Message}");
        }
    }

}
public interface IFunctionalResult<TSuccessClass>
{
    
    
}

public record Success<TSuccessClass>(TSuccessClass Value) : IFunctionalResult<TSuccessClass>
{
    public static implicit operator Success<TSuccessClass>(TSuccessClass value) => new (value);

}
public record Failure<TSuccessClass>(Exception Exception) : IFunctionalResult<TSuccessClass>
{
    public static implicit operator Failure<TSuccessClass>(Exception exception) => new (exception);
    public static Failure<TSuccessClass> FromErrorMessage(string message) => new (new ApplicationException(message));
    public static Failure<TSuccessClass> OutOfRange => new (new ArgumentOutOfRangeException());
}

public static class FunctionDeclarations
{
    public static IFunctionalResult<int> Increment(int target) => new Success<int>(target + 1);

    public static IFunctionalResult<int> Add(int target1, int target2) =>
        new Success<int>(target1 + target2);

    public static IFunctionalResult<int> Divide(int numerator, int denominator) =>
        (numerator, denominator) switch
        {
            (_, 0) => new Failure<int>(new ApplicationException("can not divide by 0")),
            _ => new Success<int>( numerator / denominator)
        };
    
    public static IFunctionalResult<int> CombinedCalc(int target1, int target2, int target3)
     => Increment(target1) switch
        {
            Failure<int> failure1 => failure1,
            Success<int> success1 => Add(success1.Value, target2) switch
            {
                Failure<int> failure2 => failure2,
                Success<int> success2 => Divide(success2.Value, target3),
                _ => Failure<int>.OutOfRange
            },
            _ => Failure<int>.OutOfRange
        };
}


