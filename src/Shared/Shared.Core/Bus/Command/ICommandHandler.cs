using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
  Task Handle(TCommand command);
}
