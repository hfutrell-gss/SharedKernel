using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Commands;
using Shared.Abstractions.Results;
using Shared.Application.Tests.Commands.TestApplication;
using Shared.Application.Extensions;
using Shared.Kernel.TestHelpers;

namespace Shared.Application.Tests.Commands;

public class CommandTests
{
    private readonly ICommandBus _commandBus;

    public CommandTests()
    {
        var serviceProvider = new ServiceCollection()
                .AddMediatR(c => c.RegisterServicesFromAssembly(typeof(AddValueCommand).Assembly))
                .AddCqrs()
                .BuildServiceProvider()
            ;

        _commandBus = serviceProvider.GetService<ICommandBus>()!;
    }

    private async Task<Result> SendSuccessfulCommandReturningBaseResult()
    {
        return await _commandBus.SendCommand(new SubtractValueCommand(2));
    }
    
    private async Task<Result> SendFailedCommandReturningBaseResult()
    {
        return await _commandBus.SendCommand(new SubtractValueCommand(1));
    }

    private async Task<Result<AddValueResult>> SendSuccessfulCommand()
    {
        return await _commandBus.SendCommand<AddValueCommand, AddValueResult>(new AddValueCommand(2));
    }

    private async Task<Result<AddValueResult>> SendFailedCommand()
    {
        return await _commandBus.SendCommand<AddValueCommand, AddValueResult>(new AddValueCommand(1));
    }
        
    [Fact]
    public async Task a_successful_command_has_success_status()
    {
        var result = await SendSuccessfulCommand();

        result.AssertSuccessful();
    }
    
    [Fact]
    public async Task a_successful_command_with_a_value_has_a_value()
    {
        var result = await SendSuccessfulCommand();
        
        result.AssertSuccessful();
    }

    
    [Fact]
    public async Task a_failed_command_shows_failed()
    {
        var result = await SendFailedCommand();
        
        result.AssertFailure();
    }
    
    [Fact]
    public async Task a_failed_command_has_errors()
    {
        var result = await SendFailedCommand();

        Assert.NotEmpty(result.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public async Task base_result_has_success()
    {
        var result = await SendSuccessfulCommandReturningBaseResult();
        
        result.AssertSuccessful();
    }
    
    [Fact]
    public async Task base_result_has_failure()
    {
        var result = await SendFailedCommandReturningBaseResult();
        
        result.AssertFailure();
    }
    
    [Fact]
    public async Task base_result_has_error_message()
    {
        var result = await SendFailedCommandReturningBaseResult();
        Assert.NotEmpty(result.ExpectFailureAndGet().FailureReasons);
    }

    [Fact]
    public void a_success_result_has_success_status()
    {
        var result = Result.Success();
        
        result.AssertSuccessful();
    }
    
    [Fact]
    public void a_failed_result_has_failed_status()
    {
        var result = Result.Fail(FailureDetails.From("ruh roh"));
            
        result.AssertFailure();
    }
    
    [Fact]
    public void a_failed_results_messages_can_be_read()
    {
        var result = Result.Fail(FailureDetails.From("ruh roh"));
                
        Assert.Contains("ruh roh", result.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void a_failed_result_can_have_many_messages()
    {
        var result = Result.Fail(FailureDetails.From("ruh roh", "this not good"));
                    
        Assert.Contains("ruh roh", result.ExpectFailureAndGet().FailureReasons);
        
        Assert.Contains("this not good", result.ExpectFailureAndGet().FailureReasons);
    }
}