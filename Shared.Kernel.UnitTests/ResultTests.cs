using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
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
    public void CanResultTypes()
    {
        var x = Result<int>.Success(1)
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 1))
            ;
                
        Assert.Equal(5, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void CanDoManyResultsPermutation()
    {
        var x = Result<int>.Success(1)
            .Map(i => Result<int>.Success(i + 1))
            .Map(i => Result<int>.Success(i + 2));
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }
 
    [Fact]
    public void FailsThroughManyResults()
    {
        var x = Result<int>.Fail("bad")
            .Map(i => Result<int>.Success(i + 2));
                    
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
        var x = Result<string>.Success("1")
            .Map(_ => Result.Success())
            .Map(_ => Result.Success())
            .Map(_ => Result<string>.Success("value"))
            .Map(s => Result<string>.Success(s))
            ;
                
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMatch()
    {
        var x = await Result<string>.Success("1")
                .Match(
                    onSuccess: async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))),
                    onFailure: async f => await Task.Run(() => Task.FromResult(Result<string>.Fail("bad bad not good"))))
            ;

        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMap()
    {
        var x = await Result<string>.Success("1")
                .Map(s => Task.FromResult(Result<string>.Success(s)))
                .Map(async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))))
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMapFromResultTResult()
    {
        var x = await Result<string>.Success("1")
                .Map(s => Task.FromResult(Result<string>.Success(s)))
                .Map(async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))))
                .Map(s => Task.FromResult(Result<string>.Success("yay")))
            ;
        
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task FailureWorksWithAsyncStuff()
    {
        var x = await Result.Fail("nope")
                .Map(_ => Task.FromResult(Result<int>.Success(2)))
                .Map(async i => await Task.Run(() => Task.FromResult(Result<string>.Success($"{i}"))))
                .Map(s => Task.FromResult(Result<string>.Success("yay")))
            ;
            
        Assert.Contains("nope", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public async Task CanDoMatchTask()
    {
        await Result.Success()
                .Match(
                onSuccess: async _ => await Task.Run(() => Assert.True(true)),
                onFailure: async _ => await Task.Run(() => Assert.Fail("bad bad not good")))
            ;
    }
    
    [Fact]
    public async Task CanDoAsyncMapFromResultToResult()
    {
        var x = await Result<string>.Success("1")
                .Map(_ => Task.FromResult(Result.Success()))
            ;

        var y = await x.Map(_ => Task.FromResult(Result.Success()));
        
            
        x.AssertSuccessful();
        y.AssertSuccessful();
    }
     
    [Fact]
    public async Task CanMapResultToResult()
    {
         var x = (await DoThing())
                .Map(async s => await DoOtherThing())
            ;

    }

    private Task<Result<string>> DoThing()
    {
        return Task.FromResult(Result<string>.Success("k"));
    }
    
    private Task<Result> DoOtherThing()
    {
        return Task.FromResult(Result.Success());
    }
}