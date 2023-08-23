using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Commands;
using Shared.Application.Tests.Commands.TestApplication;
using Shared.Application.Extensions;

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

    private async Task<CommandResult> SendSuccessfulCommandReturningBaseResult()
    {
        return await _commandBus.SendCommand(new SubtractValueCommand(2));
    }
    
    private async Task<CommandResult> SendFailedCommandReturningBaseResult()
    {
        return await _commandBus.SendCommand(new SubtractValueCommand(1));
    }

    private async Task<AddValueCommandResult> SendSuccessfulCommand()
    {
        return await _commandBus.SendCommand<AddValueCommand, AddValueCommandResult>(new AddValueCommand(2));
    }

    private async Task<AddValueCommandResult> SendFailedCommand()
    {
        return await _commandBus.SendCommand<AddValueCommand, AddValueCommandResult>(new AddValueCommand(1));
    }
        
    [Fact]
    public async Task a_successful_command_has_success_status()
    {
        var result = await SendSuccessfulCommand();
        Assert.Equal(ResultStatus.Success, result.Status);
    }
    
    [Fact]
    public async Task a_successful_command_with_a_value_has_a_value()
    {
        var result = await SendSuccessfulCommand();
        Assert.NotEqual(0, result.ResultValue);
    }
     
    [Fact]
    public async Task a_failed_command_shows_failed()
    {
        var result = await SendFailedCommand();
        Assert.Equal(ResultStatus.Failed, result.Status);
    }
    
    [Fact]
    public async Task a_failed_command_has_errors()
    {
        var result = await SendFailedCommand();
        Assert.NotEmpty(result.Errors);
    }
    
    [Fact]
    public async Task base_result_has_success()
    {
        var result = await SendSuccessfulCommandReturningBaseResult();
        Assert.Equal(ResultStatus.Success, result.Status);
    }
    
    [Fact]
    public async Task base_result_has_failure()
    {
        var result = await SendFailedCommandReturningBaseResult();
        Assert.Equal(ResultStatus.Failed, result.Status);
    }
    
    [Fact]
    public async Task base_result_has_error_message()
    {
        var result = await SendFailedCommandReturningBaseResult();
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void a_success_result_has_success_status()
    {
        var result = CommandResult.Success();
        
        Assert.Equal(ResultStatus.Success, result.Status);
    }
    
    [Fact]
    public void a_success_result_has_no_error_messages()
    {
        var result = CommandResult.Success();
            
        Assert.Null(result.Errors);
    }
     
    [Fact]
    public void a_failed_result_has_failed_status()
    {
        var result = CommandResult.Failure("ruh roh");
            
        Assert.Equal(ResultStatus.Failed, result.Status);
    }
    
    [Fact]
    public void a_failed_results_messages_can_be_read()
    {
        var result = CommandResult.Failure("ruh roh");
                
        Assert.Contains("ruh roh", result.Errors!);
    }
    
    [Fact]
    public void a_failed_result_can_have_many_messages()
    {
        var result = CommandResult.Failure(new [] {"ruh roh", "this not good"});
                    
        Assert.Contains("ruh roh", result.Errors!);
        Assert.Contains("this not good", result.Errors!);
    }
}