namespace SingleResults.Usage;

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

    public static Task<SingleResult<int>> IncrementAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target + 1));
    public static Task<SingleResult<int>> DoubleAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target * 2));
    public static Task<SingleResult<int>> TripleAsync(int target) =>
        Task.FromResult(SingleResult<int>.FromValue(target * 3));
    public static Task<SingleResult<int>> AddAsync(int target1, int target2) =>
        Task.FromResult(SingleResult<int>.FromValue(target1 + target2));
    public static Task<SingleResult<int>> DivideAsync(int numerator, int denominator) =>
        Task.FromResult(
            (numerator, denominator) switch
            {
                (_, 0) => SingleResult<int>.FromException(
                    new ApplicationException("can not divide by 0")),
                _ => SingleResult<int>.FromValue(numerator / denominator)
            });

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

    public static Task<SingleResult<int>> RailwayCalc3Async(int target1, int target2, int target3)
        => IncrementAsync(target1)
            .CombineValueAsync(() => AddAsync(target2, target3))
            .RailwayAsync(DivideAsync);
    public static Task<SingleResult<int>> RailwayCalc3Async2(int target1, int target2, int target3)
        => Increment(target1)
            .CombineValueAsync(() => AddAsync(target2, target3))
            .Railway(Divide);

    public static SingleResult<int> RailwayInstance(int target1)
        => Increment(target1)
            .Railway(Double)
            .Railway(Triple);

    public static Task<SingleResult<int>> RailwayAsync(int target1)
        => IncrementAsync(target1).RailwayAsync(DoubleAsync).RailwayAsync(TripleAsync);
    public static Task<SingleResult<int>> Railway2Async(int target1)
        => Increment(target1).RailwayAsync(DoubleAsync).RailwayAsync(TripleAsync);
    public static Task<SingleResult<int>> Railway3Async(int target1)
        => Increment(target1).RailwayAsync(DoubleAsync).Railway(Triple);
}
