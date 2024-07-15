using System;

namespace Shared.Core.Bus.Command;
public class CommandNotRegisteredException : Exception
{
  public CommandNotRegisteredException(ICommand command) : base(
      $"The command {command} has not a command handler associated")
  {
  }
}
