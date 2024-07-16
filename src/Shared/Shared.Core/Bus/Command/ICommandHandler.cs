using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
  Task<TResponse> Handle(TCommand command);
}
