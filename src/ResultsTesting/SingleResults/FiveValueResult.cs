using System.Text.Json.Serialization;
namespace SingleResults;

public record FiveValueResult<TValue1, TValue2, TValue3, TValue4, TValue5>(
    TValue1? Value1,
    TValue2? Value2,
    TValue3? Value3,
    TValue4? Value4,
    TValue5? Value5,
    Exception? Exception)
{
    [JsonIgnore]
    public bool IsSuccess => Exception is null;
    public Exception GetException() =>
        Exception ?? throw new ResultsInvalidOperationException("no exception");
    public TValue1 GetValue1() =>
        (IsSuccess ? Value1 : throw new ResultsInvalidOperationException("no value")) ??
        throw new ResultsInvalidOperationException();
    public TValue2 GetValue2() =>
        (IsSuccess ? Value2 : throw new ResultsInvalidOperationException("no value")) ??
        throw new ResultsInvalidOperationException();
    public TValue3 GetValue3() =>
        (IsSuccess ? Value3 : throw new ResultsInvalidOperationException("no value")) ??
        throw new ResultsInvalidOperationException();
    public TValue4 GetValue4() =>
        (IsSuccess ? Value4 : throw new ResultsInvalidOperationException("no value")) ??
        throw new ResultsInvalidOperationException();
    public TValue5 GetValue5() =>
        (IsSuccess ? Value5 : throw new ResultsInvalidOperationException("no value")) ??
        throw new ResultsInvalidOperationException();

    public SingleResult<TValue6> Railway<TValue6>(
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, SingleResult<TValue6>> handleValueFunc) =>
        this
            switch
            {
                { Exception: not null } e => e.Exception,
                {
                    Value1: { } value1, Value2: { } value2, Value3: { } value3, Value4: { } value4,
                    Value5: { } value5
                } => handleValueFunc(value1, value2, value3, value4, value5),
                _ => SingleResult<TValue6>.OutOfRange
            };
}
