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
            .Then(s => Result<int>.Success(s.Length)).Then(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Then(_ => Result.Success())
            .Then(_ => Result<string>.Success("x"));
        
        Assert.Equal("x", x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void PassesFailure()
    {
        var x = Result<string>.Fail("some")
            .Then(s => Result<int?>.Success(s.Length))
            .Then(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Then(_ => Result.Success())
            .Then(_ => Result<string>.Success("x"));
            
        x.AssertFailure();
        Assert.Contains("some", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void CanResultTypes()
    {
        var x = Result<int>.Success(1)
            .Then(i => Result<int>.Success(i + 1))
            .Then(i => Result<int>.Success(i + 1))
            .Then(i => Result<int>.Success(i + 1))
            .Then(i => Result<int>.Success(i + 1))
            ;
                
        Assert.Equal(5, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void CanDoManyResultsPermutation()
    {
        var x = Result<int>.Success(1)
            .Then(i => Result<int>.Success(i + 1))
            .Then(i => Result<int>.Success(i + 2));
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }
 
    [Fact]
    public void FailsThroughManyResults()
    {
        var x = Result<int>.Fail("bad")
            .Then(i => Result<int>.Success(i + 2));
                    
        x.AssertFailure();
        Assert.Contains("bad", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void BindResultToResult()
    {
        var x = Result.Success().Then(_ => Result.Success());
        x.AssertSuccessful();
    }
    
    [Fact]
    public void CanFailForManyReasons()
    {
    
        var x = Result.Success()
            .Then(_ => Result.Fail(new[] {"no", "not good"}));
        
        x.AssertFailure();
        
        Assert.Equal(new[] {"no", "not good"}, x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void CapturesExceptionsInFailureBinding()
    {
        
        var x = Result.Success().Then(_ =>
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
            .Then(i => Result<int>.Success(int.Parse(i)));
        
        x.AssertSuccessful();
        Assert.Equal(1, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void ImplicitlyCastsToATask()
    {
        // This shouldn't compile if broken so just assert success
        Task<Result<string>> task = Result<string>.Success("1")
            .Resolve(
                forSuccess: Result<string>.Success,
                forFailure: Result<string>.Fail);
            
        Assert.True(true);
    }
    
    [Fact]
    public void CastsBetweenUnitAndTypesOfT()
    {
        // This shouldn't compile if broken so just assert success
        var x = Result<string>.Success("1")
            .Then(_ => Result.Success())
            .Then(_ => Result.Success())
            .Then(_ => Result<string>.Success("value"))
            .Then(s => Result<string>.Success(s))
            ;
                
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMatch()
    {
        var x = await Result<string>.Success("1")
                .Resolve(
                    forSuccess: async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))),
                    forFailure: async f => await Task.Run(() => Task.FromResult(Result<string>.Fail("bad bad not good"))))
            ;

        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMap()
    {
        var x = await Result<string>.Success("1")
                .Then(s => Task.FromResult(Result<string>.Success(s)))
                .Then(async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))))
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMapFromResultTResult()
    {
        var x = await Result<string>.Success("1")
                .Then(s => Task.FromResult(Result<string>.Success(s)))
                .Then(async s => await Task.Run(() => Task.FromResult(Result<string>.Success(s))))
                .Then(s => Task.FromResult(Result<string>.Success("yay")))
            ;
        
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task FailureWorksWithAsyncStuff()
    {
        var x = await Result.Fail("nope")
                .Then(_ => Task.FromResult(Result<int>.Success(2)))
                .Then(async i => await Task.Run(() => Task.FromResult(Result<string>.Success($"{i}"))))
                .Then(s => Task.FromResult(Result<string>.Success("yay")))
            ;
            
        Assert.Contains("nope", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public async Task CanDoMatchTask()
    {
        await Result.Success()
                .Resolve(
                onSuccess: async _ => await Task.Run(() => Assert.True(true)),
                onFailure: async _ => await Task.Run(() => Assert.Fail("bad bad not good")))
            ;
    }
    
    [Fact]
    public async Task CanDoAsyncMapFromResultToResult()
    {
        var x = await Result<string>.Success("1")
                .Then(_ => Task.FromResult(Result.Success()))
            ;

        var y = await x.Then(_ => Task.FromResult(Result.Success()));
        
            
        x.AssertSuccessful();
        y.AssertSuccessful();
    }
     
    [Fact]
    public async Task CanMapTaskResultToResult()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async s => await DoOtherThing())
                .Then(_ => Result.Success())
            ;

        x.AssertSuccessful();
    }

    [Fact]
    public async Task CanMapTaskResultToResultT()
    {
        // This should compile in this structure
        Result<string> x = await (await DoThing())
                .Then(async s => await DoOtherThing())
                .Then(a => Result<string>.Success("k"))
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanMapTaskResultToTaskResult()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async s => await DoOtherThing())
                .Then(async _ => await DoOtherThing())
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoTaskThenTaskOfT()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async s => await DoOtherThing())
                .Then(async _ => await DoOtherThing())
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoTaskOfTAndThenATaskAndUseMethodBodies()
    {
        // This should compile in this structure
        var x = await (await DoThing())
                .Then(async s => await DoThing())
                .Then(async s => await DoThing())
                .Then(async s => await DoOtherThing())
                .Then(async u => await DoThing())
                .Then(DoStringThing)
                .Then(async s => await DoOtherThing())
                .Then(DoThing)
            ;
    
        x.AssertSuccessful();
    }

    [Fact]
    public void CanDoImplicitFlatMapping()
    {
        // This should compile in this structure
        var x = Result.Success()
                .Then(_ => "x")
                .Then(s => 4)
            ;
            
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public void CanDoImplicitFlatMappingWithActions()
    {
        // This should compile in this structure
        var x = Result.Success()
                .Then(_ => "x")
                .Then(_ => new List<string>().Add("as"))
                .Then(_ => 4)
            ;
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public void ClosuresPersistValues()
    {
        var list = new List<string>();
        // This should compile in this structure
        var x = Result.Success()
                .Then(_ => "x")
                .Then(s => list.Add(s))
                .Then(_ => 4)
            ;
                
        Assert.Contains("x", list);
    }
    
    [Fact]
    public async Task CanDoImplicitFlatMappingAsync()
    {
        // This should compile in this structure
        var x = await (await DoThing())
                .Then(async s => await DoThing())
                .Then(s => "4")
                .Then(int.Parse)
            ;
        
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public async Task CanDoImplicitFlatMappingAsyncInClojureWithActions()
    {
        var list = new List<string>();
        // This should compile in this structure
        var x = await (await DoThing())
                .Then(async s => await DoThing())
                .Then(s => $"Good {s}")
                .Then(s => list.Add(s))
            ;
            
        Assert.Contains("Good k", list);
    }

    private Task<Result<string>> DoThing()
    {
        return Task.FromResult(Result<string>.Success("k"));
    }
    
    private Task<Result<string>> DoStringThing(string s)
    {
        return Task.FromResult(Result<string>.Success(s));
    }
     
    private Task<Result> DoOtherThing()
    {
        return Task.FromResult(Result.Success());
    }
}