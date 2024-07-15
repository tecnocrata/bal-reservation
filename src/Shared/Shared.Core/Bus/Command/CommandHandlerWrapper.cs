using System;
using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public abstract class CommandHandlerWrapper
{
  public abstract Task Handle(ICommand command, IServiceProvider provider);
}

public class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper
    where TCommand : ICommand
{
  public override async Task Handle(ICommand command, IServiceProvider provider)
  {
    if (command == null)
    {
      throw new ArgumentNullException(nameof(command));
    }

    if (provider == null)
    {
      throw new ArgumentNullException(nameof(provider));
    }

    var handler = provider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
    if (handler == null)
    {
      throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found");
    }

    await handler.Handle((TCommand)command);
  }
}
