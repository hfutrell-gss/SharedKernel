using System.Reflection;
using Shared.Abstractions.Commands;
using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests;

public class ResultTests
{
    [Fact]
    public void PassesSuccessWhenResultOfTAndResult()
    {
        var x = Result<string>.Success("some")
            .Bind(s => Result<int>.Success(s.Length))
            .Bind(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Bind(Result.Success)
            .Bind(() => Result<string>.Success("x"));
        
        Assert.Equal("x", x.ResultValue);
    }
    
    [Fact]
    public void PassesFailure()
    {
        var x = Result<string>.Fail("some")
            .Bind(s => Result<int>.Success(s.Length))
            .Bind(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Bind(Result.Success)
            .Bind(() => Result<string>.Success("x"));
            
        Assert.Null(x.ResultValue);
        Assert.Contains("some", x.FailureReasons);
    }
    
    [Fact]
    public void CanDoManyResults()
    {
        var x = Result<int>.Success(1)
            .Bind(i => ChangeResult<int>.Success(i + 1))
            .Bind(i => CommandResult<int>.Success(i + 2));
                
        Assert.Equal(4, x.ResultValue);
    }
    
        
    [Fact]
    public void CanDoManyResultsPermutation()
    {
        var x = ChangeResult<int>.Success(1)
            .Bind(i => CommandResult<int>.Success(i + 1))
            .Bind(i => Result<int>.Success(i + 2));
                
        Assert.Equal(4, x.ResultValue);
    }
 
    [Fact]
    public void FailsThroughManyResults()
    {
        var x = Result<int>.Fail("bad")
            .Bind(i => ChangeResult<int>.Success(i + 1))
            .Bind(i => CommandResult<int>.Success(i + 2));
                    
        Assert.False(x.WasSuccessful);
        Assert.Contains("bad", x.FailureReasons);
    }
}