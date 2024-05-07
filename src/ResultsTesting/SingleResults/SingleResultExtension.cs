namespace SingleResults;

public static class SingleResultExtension
{
    public static async Task<SingleResult<TValue2>> RailwayAsync<TValue1, TValue2>(
        this Task<SingleResult<TValue1>> firstValue,
        Func<TValue1, Task<SingleResult<TValue2>>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => await handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };
    public static async Task<SingleResult<TValue2>> RailwayAsyncWrapTry<TValue1, TValue2>(
        this Task<SingleResult<TValue1>> firstValue,
        Func<TValue1, Task<TValue2>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => await SingleResult<TValue2>.WrapTryAsync(
                () => handleValueFunc(value)),
            _ => SingleResult<TValue2>.OutOfRange
        };
    public static async Task<SingleResult<TValue3>> RailwayAsync<TValue1, TValue2, TValue3>(
        this Task<TwoValueResult<TValue1, TValue2>> firstValue,
        Func<TValue1, TValue2, Task<SingleResult<TValue3>>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value1: { } value1, Value2: { } value2 } => await handleValueFunc(value1, value2),
            _ => SingleResult<TValue3>.OutOfRange
        };
    public static async Task<SingleResult<TValue3>> RailwayAsyncWrapTry<TValue1, TValue2, TValue3>(
        this Task<TwoValueResult<TValue1, TValue2>> firstValue,
        Func<TValue1, TValue2, Task<TValue3>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value1: { } value1, Value2: { } value2 } => await SingleResult<TValue3>.WrapTryAsync(
                () => handleValueFunc(value1, value2)),
            _ => SingleResult<TValue3>.OutOfRange
        };
    public static async Task<SingleResult<TValue2>> Railway<TValue1, TValue2>(
        this Task<SingleResult<TValue1>> firstValue,
        Func<TValue1, SingleResult<TValue2>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };
    public static async Task<SingleResult<TValue3>> Railway<TValue1, TValue2, TValue3>(
        this Task<TwoValueResult<TValue1, TValue2>> firstValue,
        Func<TValue1, TValue2, SingleResult<TValue3>> handleValueFunc) => await firstValue
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value1: { } value1, Value2: { } value2 } => handleValueFunc(value1, value2),
            _ => SingleResult<TValue3>.OutOfRange
        };

    public static async Task<TwoValueResult<TValue1, TValue2>> CombineValueAsync<TValue1, TValue2>(
        this Task<SingleResult<TValue1>> firstValueTask,
        Func<Task<SingleResult<TValue2>>> secondValueFunc) => await firstValueTask switch
    {
        { Exception: not null } e => new TwoValueResult<TValue1, TValue2>(
            e.Value,
            default,
            e.Exception),
        { Value: { } firstValue } => await secondValueFunc() switch
        {
            { Exception: not null } e => new TwoValueResult<TValue1, TValue2>(
                firstValue,
                default,
                e.Exception),
            { Value: { } secondValue } => new TwoValueResult<TValue1, TValue2>(
                firstValue,
                secondValue,
                null),
            _ => new TwoValueResult<TValue1, TValue2>(firstValue, default, null)
        },
        _ => new TwoValueResult<TValue1, TValue2>(
            default,
            default,
            new ResultValueNullException("out of range"))
    };
    public static async Task<TwoValueResult<TValue1, TValue2>> CombineValueAsyncWrapTry<TValue1,
        TValue2>(
        this Task<SingleResult<TValue1>> firstValueTask,
        Func<Task<TValue2>> secondValueFunc) => await firstValueTask switch
    {
        { Exception: not null } e => new TwoValueResult<TValue1, TValue2>(
            e.Value,
            default,
            e.Exception),
        { Value: { } firstValue } => await SingleResult<TValue2>.WrapTryAsync(secondValueFunc)
            switch
            {
                { Exception: not null } e => new TwoValueResult<TValue1, TValue2>(
                    firstValue,
                    default,
                    e.Exception),
                { Value: { } secondValue } => new TwoValueResult<TValue1, TValue2>(
                    firstValue,
                    secondValue,
                    null),
                _ => new TwoValueResult<TValue1, TValue2>(firstValue, default, null)
            },
        _ => new TwoValueResult<TValue1, TValue2>(
            default,
            default,
            new ResultValueNullException("out of range"))
    };
}
