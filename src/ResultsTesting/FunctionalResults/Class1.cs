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