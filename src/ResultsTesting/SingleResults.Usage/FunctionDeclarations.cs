namespace SingleResults.Usage;

public static class FunctionDeclarations
{
    public static SingleResult<int> Increment(int target) => target + 1;
    public static int IncrementWithThrowing(int target) => target switch
    {
        3 => throw new ApplicationException("3 is not allowed"),
        _ => target + 1
    };
    public static SingleResult<int> Double(int target) => target * 2;
    public static SingleResult<int> Triple(int target) => target * 3;
    public static int TripleWithThrowing(int target) => target switch
    {
        10 => throw new ApplicationException("10 is not allowed"),
        _ => target * 3
    };
    public static SingleResult<int> Add(int target1, int target2) => target1 + target2;
    public static int AddWithThrowing(int target1, int target3) => target1 switch
    {
        > 100 => throw new ApplicationException("over 100 is not allowed"),
        _ => target1 + target3
    };
    public static SingleResult<int> Divide(int numerator, int denominator) =>
        (numerator, denominator) switch
        {
            (_, 0) => new ApplicationException("can not divide by 0"),
            _ => numerator / denominator
        };
    public static int DivideWithThrowing(int numerator, int denominator) =>
        denominator == 0
            ? throw new ApplicationException("can not divide by 0")
            : numerator / denominator;

    public static Task<SingleResult<int>> IncrementAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target + 1));
    public static async Task<int> IncrementAsyncWithThrowing(int target) =>
        await Task.FromResult(
            target switch
            {
                3 => throw new ApplicationException("3 is not allowed"),
                _ => target + 1
            });
    public static Task<SingleResult<int>> DoubleAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target * 2));
    public static Task<int> DoubleAsyncWithThrowing(int target) =>
        Task.FromResult(
            target switch
            {
                5 => throw new ApplicationException("5 is not allowed"),
                _ => target * 2
            });
    public static Task<SingleResult<int>> TripleAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target * 3));
    public static Task<SingleResult<int>> AddAsync(int target1, int target2) =>
        Task.FromResult(SingleResult<int>.FromValue(target1 + target2));
    public static Task<int> AddAsyncWithThrowing(int target1, int target2) =>
        Task.FromResult(
            target1 switch
            {
                > 100 => throw new ApplicationException("over 100 is not allowed"),
                _ => target1 + target2
            });
    public static Task<SingleResult<int>> DivideAsync(int numerator, int denominator) =>
        Task.FromResult(
            (numerator, denominator) switch
            {
                (_, 0) => SingleResult<int>.FromException(
                    new ApplicationException("can not divide by 0")),
                _ => SingleResult<int>.FromValue(numerator / denominator)
            });
    public static Task<int> DivideAsyncWithThrowing(int numerator, int denominator) =>
        Task.FromResult(
            denominator == 0
                ? throw new ApplicationException("can not divide by 0")
                : numerator / denominator);

    public static SingleResult<int> CombinedCalc(int target1, int target2, int target3)
        => Increment(target1) switch
        {
            { Exception: not null } exception1 => exception1,
            { Value: { } value1 } => Add(value1, target2) switch
            {
                { Exception : not null } exception2 => exception2,
                { Value: { } value2 } => Divide(value2, target3)
            }
        };
    public static SingleResult<int> Combined2Calc(int target1, int target2, int target3)
        => Increment(target1) switch
        {
            { Exception: not null } exception1 => exception1,
            { Value: { } value1 } => Add(target2, target3) switch
            {
                { Exception : not null } exception2 => exception2,
                { Value: { } value2 } => Divide(value1, value2)
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
            .Railway(value1 => Add(value1, target2))
            .Railway(value2 => Divide(value2, target3));
    public static SingleResult<int> Railway2Calc3(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValue(Add(target2, target3))
            .Railway(Divide);
    public static SingleResult<int> Railway2Calc4(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValueWrapTry(() => AddWithThrowing(target2, target3))
            .RailwayWrapTry(DivideWithThrowing);

    public static Task<SingleResult<int>> RailwayCalc3Async(int target1, int target2, int target3)
        => IncrementAsync(target1)
            .CombineValueAsync(() => AddAsync(target2, target3))
            .RailwayAsync(DivideAsync);
    public static Task<SingleResult<int>> RailwayCalc3Async2(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValueAsync(() => AddAsync(target2, target3))
            .Railway(Divide);
    public static Task<SingleResult<int>> RailwayCalc3Async4(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValueAsyncWrapTry(() => AddAsyncWithThrowing(target2, target3))
            .RailwayAsyncWrapTry(DivideAsyncWithThrowing);

    public static SingleResult<int> RailwayCalc3Async3(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValue(Add(target2, target3))
            .RailwayWrapTry(DivideWithThrowing);
    public static Task<SingleResult<int>> RailwayCalc3Async5(int target1, int target2, int target3)
        => IncrementAsync(target1)
            .CombineValueAsyncWrapTry(() => AddAsyncWithThrowing(target2, target3))
            .RailwayAsync(DivideAsync);

    public static Task<SingleResult<int>> Railway2Calc3Async6(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValue(Add(target2, target3))
            .RailwayAsync(DivideAsync);
    public static Task<SingleResult<int>> Railway2Calc3Async7(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValue(Add(target2, target3))
            .RailwayAsyncWrapTry(DivideAsyncWithThrowing);

    public static SingleResult<int> RailwayInstance(int target1)
        => Increment(target1)
            .Railway(Double)
            .Railway(Triple);
    public static SingleResult<int> RailwayInstance2(int target1)
        => SingleResult<int>.WrapTry(() => IncrementWithThrowing(target1))
            .Railway(Double)
            .RailwayWrapTry(TripleWithThrowing);

    public static Task<SingleResult<int>> RailwayAsync(int target1)
        => IncrementAsync(target1).RailwayAsync(DoubleAsync).RailwayAsync(TripleAsync);
    public static Task<SingleResult<int>> Railway2Async(int target1)
        => Increment(target1).RailwayAsync(DoubleAsync).RailwayAsync(TripleAsync);
    public static Task<SingleResult<int>> Railway3Async(int target1)
        => Increment(target1).RailwayAsync(DoubleAsync).Railway(Triple);
    public static Task<SingleResult<int>> Railway4Async(int target1)
        => SingleResult<int>.WrapTryAsync(() => IncrementAsyncWithThrowing(target1))
            .RailwayAsyncWrapTry(DoubleAsyncWithThrowing)
            .RailwayAsync(TripleAsync);
}
