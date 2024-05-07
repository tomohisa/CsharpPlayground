namespace FunctionalResults;

public record Success<TSuccessClass>(TSuccessClass Value) : IFunctionalResult<TSuccessClass>
{
    public static implicit operator Success<TSuccessClass>(TSuccessClass value) => new (value);

}