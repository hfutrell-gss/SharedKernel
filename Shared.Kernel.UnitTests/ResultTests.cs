using Shared.Abstractions.Commands;
using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;
using Shared.Kernel.TestHelpers;

namespace Shared.Kernel.UnitTests;

public class ResultTests
{
    [Fact]
    public void PassesSuccessWhenResultOfTAndResult()
    {
        var x = Result<string>.Success("some")
            .Map(s => Result<int>.Success(s.Length)).Map(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Map(_ => Result.Success())
            .Map(_ => Result<string>.Success("x"));
        
        Assert.Equal("x", x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void PassesFailure()
    {
        var x = Result<string>.Fail("some")
            .Map(s => Result<int?>.Success(s.Length))
            .Map(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Map(_ => Result.Success())
            .Map(_ => Result<string>.Success("x"));
            
        x.AssertFailure();
        Assert.Contains("some", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void CanChangeResultTypes()
    {
        var x = ChangeResult<int>.Success(1)
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => CommandResult<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => ChangeResult<int>.Success(i + 1))
            ;
                
        Assert.Equal(5, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void CanDoManyResultsPermutation()
    {
        var x = ChangeResult<int>.Success(1)
            .Map(i => CommandResult<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 2));
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }
 
    [Fact]
    public void FailsThroughManyResults()
    {
        var x = Result<int>.Fail("bad")
            .Map(i => CommandResult<int>.Success(i + 2));
                    
        x.AssertFailure();
        Assert.Contains("bad", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void BindResultToResult()
    {
        var x = Result.Success().Map(_ => Result.Success());
        x.AssertSuccessful();
    }
    
    [Fact]
    public void CanFailForManyReasons()
    {
    
        var x = Result.Success()
            .Map(_ => Result.Fail(new[] {"no", "not good"}));
        
        x.AssertFailure();
        
        Assert.Equal(new[] {"no", "not good"}, x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void CapturesExceptionsInFailureBinding()
    {
        
        var x = Result.Success().Map(_ =>
        {
            throw new Exception("bad bad not good");
            return Result.Success();
        });
        
        x.AssertFailure();
        Assert.Contains("bad bad not good", x.ExpectFailureAndGet().FailureReasons);
        Assert.NotNull(x.ExpectFailureAndGet().Exception);
    }
    
    [Fact]
    public void CanMapAResultOut()
    {
       
        var x = Result<string>.Success("1")
            .Map(i => Result<int>.Success(int.Parse(i)));
        
        x.AssertSuccessful();
        Assert.Equal(1, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void ImplicitlyCastsToATask()
    {
        // This shouldn't compile if broken so just assert success
        Task<Result<string>> task = Result<string>.Success("1")
            .Match(
                onSuccess: Result<string>.Success,
                onFailure: Result<string>.Fail);
            
        Assert.True(true);
    }
    
    [Fact]
    public void CastsBetweenUnitAndTypesOfT()
    {
        // This shouldn't compile if broken so just assert success
        var x = CommandResult<string>.Success("1")
            .Map(_ => CommandResult.Success())
            .Map(_ => Result.Success())
            .Map(_ => Result<string>.Success("value"))
            .Map(s => ChangeResult<string>.Success(s))
            ;
                
        x.AssertSuccessful();
    }
}