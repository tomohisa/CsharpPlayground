using System.Text.Json.Serialization;
namespace SingleResults;

public record SingleResult<TValue>(TValue? Value, Exception? Exception) 
{
    public static SingleResult<TValue> FromValue(TValue value) => new(value, null);
    public static SingleResult<TValue> FromException(Exception exception) => new(default, exception);
    
    [JsonIgnore]
    public bool IsSuccess => Exception is null;
    public Exception GetException() => Exception ?? throw new InvalidOperationException("no exception");
    public TValue GetValue() => (IsSuccess ? Value : throw new InvalidOperationException("no value")) ?? throw new InvalidOperationException();
    public static implicit operator SingleResult<TValue>(TValue value) => new (value, null);
    public static implicit operator SingleResult<TValue>(Exception exception) => new (default, exception);
    public static SingleResult<TValue> OutOfRange => new (new ArgumentOutOfRangeException());

    public TwoValueResult<TValue, TValue2> CombineValue<TValue2>(
        SingleResult<TValue2> secondValue) => this switch
    {
        { Exception: not null } e => new(Value, default, e.Exception),
        { Value: { } } => secondValue switch
        {
            { Exception: not null } e => new(Value, default, e.Exception),
            { Value: { } } => new(Value, secondValue.Value, null),
            _ => new(Value, default, null)
        },
        _ => new(Value, default, new ArgumentOutOfRangeException("out of range"))
    };
    public TwoValueResult<TValue, TValue2> CombineValueWrapTry<TValue2>(
        Func<TValue2> secondValueFunc) => this switch
    {
        { Exception: not null } e => new(Value, default, e.Exception),
        { Value: { } } => SingleResult<TValue2>.WrapTry(secondValueFunc) switch
        {
            { Exception: not null } e => new(Value, default, e.Exception),
            { Value: { } secondValue } => new(Value, secondValue, null),
            _ => new(Value, default, null)
        },
        _ => new(Value, default, new ArgumentOutOfRangeException("out of range"))
    };
    public TwoValueResult<TValue, TValue2> CombineValue<TValue2>(
        Func<TValue, SingleResult<TValue2>> secondValueFunc) => this switch
    {
        { Exception: not null } e => new(Value, default, e.Exception),
        { Value: { } value } => secondValueFunc(value) switch
        {
            { Exception: not null } e => new(Value, default, e.Exception),
            { Value: { } secondValue } => new(Value, secondValue, null),
            _ => new(Value, default, null)
        },
        _ => new(Value, default, new ArgumentOutOfRangeException("out of range"))
    };
    public static SingleResult<TValue> WrapTry(Func<TValue> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return e;
        }
    }
    public static SingleResult<TValue> WrapTry(Func<SingleResult<TValue>> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public SingleResult<TValue2> Railway<TValue2>(Func<TValue, SingleResult<TValue2>> handleValueFunc) => this
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };
    
    public async Task<SingleResult<TValue2>> RailwayAsync<TValue2>(Func<TValue, Task<SingleResult<TValue2>>> handleValueFunc) => this
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => await handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };

    public SingleResult<TValue2> RailwayWrapTry<TValue2>(Func<TValue, TValue2> handleValueFunc) =>
        this switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => SingleResult<TValue2>.WrapTry(() => handleValueFunc(value)),
            _ => SingleResult<TValue2>.OutOfRange
        };

    public static SingleResult<TValue> Railway<TValue2>(
        Func<SingleResult<TValue2>> func,
        Func<TValue2, SingleResult<TValue>> handleValueFunc) => func()
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => handleValueFunc(value),
            _ => OutOfRange
        };
    public static SingleResult<TValue> Railway<TValue2>(
        SingleResult<TValue2> firstResult,
        Func<TValue2, SingleResult<TValue>> handleValueFunc) => firstResult
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => handleValueFunc(value),
            _ => OutOfRange
        };

    public static SingleResult<TValue> Railway2Combine<TValue1, TValue2>(
        Func<SingleResult<TValue1>> func1,
        Func<SingleResult<TValue2>> func2,
        Func<TValue1, TValue2, SingleResult<TValue>> handleValueFunc) => func1()
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value1 } => func2() switch
            {
                { Exception: not null } e => e.Exception,
                { Value: { } value2 } => handleValueFunc(value1, value2),
                _ => OutOfRange
            },
            _ => OutOfRange
        };
    public static SingleResult<TValue> Railway2Combine<TResult1Class, TResult2Class>(
        SingleResult<TResult1Class> firstValue,
        SingleResult<TResult2Class> secondValue,
        Func<TResult1Class, TResult2Class, SingleResult<TValue>> handleValueFunc) => firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value1 } => secondValue switch
            {
                { Exception: not null } e => e.Exception,
                { Value: { } value2 } => handleValueFunc(value1, value2),
                _ => OutOfRange
            },
            _ => OutOfRange
        };

}