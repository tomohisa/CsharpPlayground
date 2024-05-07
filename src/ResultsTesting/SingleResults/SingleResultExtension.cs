namespace SingleResults;

public static class SingleResultExtension
{
    public static async Task<SingleResult<TValue2>> RailwayAsync<TValue1, TValue2>(this Task<SingleResult<TValue1>> firstValue, Func<TValue1, Task<SingleResult<TValue2>>> handleValueFunc) => (await firstValue)
        switch
        {
            { Exception: not null } e => e.Exception,
            { Value: { } value } => await handleValueFunc(value),
            _ => SingleResult<TValue2>.OutOfRange
        };
}