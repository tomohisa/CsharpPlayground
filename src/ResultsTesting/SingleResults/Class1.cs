using System.Diagnostics;
using System.Text.Json.Serialization;
namespace SingleResults;

public class Class1
{

}

public record TwoValueResult<TValue1, TValue2>(
    TValue1? Value1,
    TValue2? Value2,
    Exception? Exception)
{
    public SingleResult<TValue3> Railway<TValue3>(Func<TValue1, TValue2, SingleResult<TValue3>> handleValueFunc) => this
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value1: { } value1, Value2: { } value2 } => handleValueFunc(value1, value2),
            _ => SingleResult<TValue3>.OutOfRange
        };
};

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
    public SingleResult<TValue2> UseValue<TValue2>(Func<TValue, SingleResult<TValue2>> handleValueFunc) => this
        switch
        {
            { Exception: not null } e => e.GetException(),
            { Value: { } value } => handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };
    
    public static SingleResult<TValue> Try(Func<TValue> func)
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
    public static SingleResult<TValue> Try(Func<SingleResult<TValue>> func)
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

public record Optional<TValue>(TValue? Value)
{
    [JsonIgnore]
    public bool HasValue => Value is not null;
    public TValue GetValue() => Value ?? throw new InvalidOperationException("no value");
}

public static class FunctionDeclarations
{
    public static SingleResult<int> Increment(int target) => target + 1;
    public static SingleResult<int> Double(int target) => target * 2;
    public static SingleResult<int> Triple(int target) => target * 3;
    public static SingleResult<int> Add(int target1, int target2) => target1 + target2;
    public static SingleResult<int> Divide(int numerator, int denominator) =>
        (numerator, denominator) switch
        {
            (_, 0) => new ApplicationException("can not divide by 0"),
            _ => numerator / denominator
        };
    
    
    public static SingleResult<int> CombinedCalc(int target1, int target2, int target3)
        => Increment(target1) switch
            {
                { Exception: not null } exception1 => exception1,
                { Value: { } value1 }  => Add(value1, target2) switch
                {
                    {Exception : not null } exception2 => exception2,
                    { Value: { } value2 }  => Divide(value2, target3)
                }
            };
    public static SingleResult<int> Combined2Calc(int target1, int target2, int target3)
        => Increment(target1) switch
        {
            { Exception: not null } exception1 => exception1,
            { Value: { } value1 }  => Add(target2, target3) switch
            {
                {Exception : not null } exception2 => exception2,
                { Value: { } value2 }  => Divide(value1, value2)
            }
        };
    public static SingleResult<int> RailwayCalc(int target1, int target2, int target3)
        => SingleResult<int>.Railway(
            () => Increment(target1),
            value1 => SingleResult<int>.Railway(
                () => Add(value1, target2),
                value2 => Divide(value2, target3)));
    public static SingleResult<int> RailwayCalc2(int target1, int target2, int target3)
        => SingleResult<int>.Railway(
            Increment(target1),
            value1 => SingleResult<int>.Railway(
                () => Add(value1, target2),
                value2 => Divide(value2, target3)));
    
    
    public static SingleResult<int> Railway2Calc(int target1, int target2, int target3)
        => SingleResult<int>.Railway2Combine(
            () => Increment(target1),
            () => Add(target2, target3),
            Divide);
    public static SingleResult<int> Railway2Calc2(int target1, int target2, int target3)
        => SingleResult<int>.Railway2Combine(
            Increment(target1),
            Add(target2, target3),
            Divide);
    
    
    public static SingleResult<int> RailwayCalc3(int target1, int target2, int target3)
        => Increment(target1)
            .UseValue((value1) => Add(value1, target2))
            .UseValue(value2 => Divide(value2, target3));
    public static SingleResult<int> Railway2Calc3(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValue(Add(target2, target3))
            .Railway(Divide);
    
    public static SingleResult<int> RailwayInstance(int target1)
     => Increment(target1)
         .Railway(Double)
         .Railway(Triple);

}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     