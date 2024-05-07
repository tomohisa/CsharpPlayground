namespace FunctionalResults;

public record Failure<TSuccessClass>(Exception Exception) : IFunctionalResult<TSuccessClass>
{
    public static implicit operator Failure<TSuccessClass>(Exception exception) => new (exception);
    public static Failure<TSuccessClass> FromErrorMessage(string message) => new (new ApplicationException(message));
    public static Failure<TSuccessClass> OutOfRange => new (new ArgumentOutOfRangeException());
}