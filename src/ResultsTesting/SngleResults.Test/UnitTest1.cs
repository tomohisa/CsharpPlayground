using SingleResults.Usage;
namespace SngleResults.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }

    [Fact]
    public async Task RailwayAsyncSpec()
    {
        var sut = await FunctionDeclarations.RailwayAsync(1);

        Assert.True(sut.IsSuccess);
        Assert.Equal(12, sut.Value);
    }
    [Fact]
    public async Task Railway2AsyncSpec()
    {
        var sut = await FunctionDeclarations.Railway2Async(1);

        Assert.True(sut.IsSuccess);
        Assert.Equal(12, sut.Value);
    }
    [Fact]
    public void RailwaySpec()
    {
        var sut = FunctionDeclarations.RailwayInstance(1);

        Assert.True(sut.IsSuccess);
        Assert.Equal(12, sut.Value);
    }

    [Fact]
    public void Railway2Calc3Spec()
    {
        var sut = FunctionDeclarations.Railway2Calc3(9, 2, 3);
        Assert.True(sut.IsSuccess);
        Assert.Equal(2, sut.Value);
    }
    [Fact]
    public async Task Railway2Calc3AsyncSpec()
    {
        var sut = await FunctionDeclarations.RailwayCalc3Async(9, 2, 3);
        Assert.True(sut.IsSuccess);
        Assert.Equal(2, sut.Value);
    }
    [Fact]
    public async Task RailwayCalc3Async2Spec()
    {
        var sut = await FunctionDeclarations.RailwayCalc3Async2(9, 2, 3);
        Assert.True(sut.IsSuccess);
        Assert.Equal(2, sut.Value);
    }
}
