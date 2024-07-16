using System;
using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public abstract class CommandHandlerWrapper<TResponse>
{
  public abstract Task<TResponse> Handle(ICommand command, IServiceProvider provider);
}

public class CommandHandlerWrapper<TCommand, TResponse> : CommandHandlerWrapper<TResponse>
    where TCommand : ICommand
{
  public override async Task<TResponse> Handle(ICommand command, IServiceProvider provider)
  {
    if (command == null)
    {
      throw new ArgumentNullException(nameof(command));
    }

    if (provider == null)
    {
      throw new ArgumentNullException(nameof(provider));
    }

    var handler = provider.GetService(typeof(ICommandHandler<TCommand, TResponse>)) as ICommandHandler<TCommand, TResponse>;
    if (handler == null)
    {
      throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found");
    }

    return await handler.Handle((TCommand)command);
  }
}
