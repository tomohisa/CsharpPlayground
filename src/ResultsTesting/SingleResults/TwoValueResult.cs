using System.Text.Json.Serialization;
namespace SingleResults;

public record TwoValueResult<TValue1, TValue2>(
    TValue1? Value1,
    TValue2? Value2,
    Exception? Exception)
{
    [JsonIgnore]
    public bool IsSuccess => Exception is null;
    public Exception GetException() => Exception ?? throw new InvalidOperationException("no exception");
    public TValue1 GetValue1() => (IsSuccess ? Value1 : throw new InvalidOperationException("no value")) ?? throw new InvalidOperationException();

    public SingleResult<TValue3> Railway<TValue3>(Func<TValue1, TValue2, SingleResult<TValue3>> handleValueFunc) => this
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value1: { } value1, Value2: { } value2 } => handleValueFunc(value1, value2),
            _ => SingleResult<TValue3>.OutOfRange
        };
    public ThreeValueResult<TValue1, TValue2, TValue3> CombineValue<TValue3>(SingleResult<TValue3> thirdValue) => this switch
    {
        { Exception: not null } e => new(Value1, Value2, default, e.Exception),
        { Value1: { }, Value2: { } } => thirdValue switch
        {
            { Exception: not null } e => new(Value1, Value2, default, e.Exception),
            { Value: { } } => new(Value1, Value2, thirdValue.Value, null),
            _ => new(Value1, Value2, default, null)
        },
        _ => new(Value1, Value2, default, new ArgumentOutOfRangeException("out of range"))
    };
    
    public ThreeValueResult<TValue1, TValue2, TValue3> CombineValue<TValue3>(Func<TValue1,TValue2, SingleResult<TValue3>> thirdValueFunc) => this switch
    {
        { Exception: not null } e => new(Value1, Value2, default, e.Exception),
        { Value1: { } value1, Value2: { } value2 } => thirdValueFunc(value1, value2) switch
        {
            { Exception: not null } e => new(Value1, Value2, default, e.Exception),
            { Value: { } value3 } => new(Value1, Value2, value3, null),
            _ => new(Value1, Value2, default, null)
        },
        _ => new(Value1, Value2, default, new ArgumentOutOfRangeException("out of range"))
    };
    
}