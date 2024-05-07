using SingleResults;
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
    public void RailwaySpec()
    {
        var sut =  FunctionDeclarations.RailwayInstance(1);

        Assert.True(sut.IsSuccess);
        Assert.Equal(12, sut.Value);
    }
    
}
