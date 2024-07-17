using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Shared.Core.Bus.Command;

namespace Shared.Infrastructure.Bus.Command;
public class InMemoryCommandBus : ICommandBus
{
  private static readonly ConcurrentDictionary<Type, object> _commandHandlers =
      new ConcurrentDictionary<Type, object>();

  private readonly IServiceProvider _provider;

  public InMemoryCommandBus(IServiceProvider provider)
  {
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
  }


  public async Task<TResponse> Dispatch<TResponse>(ICommand command) where TResponse : class
  {
    if (command == null)
    {
      throw new ArgumentNullException(nameof(command));
    }

    var handler = GetWrappedHandlers<TResponse>(command);

    if (handler == null)
    {
      throw new CommandNotRegisteredException(command);
    }

    return await handler.Handle(command, _provider);
  }

  private CommandHandlerWrapper<TResponse> GetWrappedHandlers<TResponse>(ICommand command)
  {
    Type[] typeArgs = { command.GetType(), typeof(TResponse) };

    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(typeArgs);
    var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(typeArgs);

    var handlers = _provider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType)) as IEnumerable;
    if (handlers == null)
    {
      throw new InvalidOperationException($"The command {command.GetType().Name} has not a command handlers associated");
    }

    var handler = handlers.Cast<object>()
    .Select(h =>
    {
      var instance = Activator.CreateInstance(wrapperType);
      if (instance == null)
      {
        throw new InvalidOperationException($"Unable to create an instance of type {wrapperType.FullName}.");
      }

      return (CommandHandlerWrapper<TResponse>)instance;
    })
    .FirstOrDefault();

    if (handler == null)
    {
      throw new InvalidOperationException($"The command {command.GetType().Name} has not a command handler associated");
    }

    var wrappedHandlers = (CommandHandlerWrapper<TResponse>)_commandHandlers.GetOrAdd(command.GetType(), handler);

    return wrappedHandlers;
  }
}
