using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public interface ICommandBus
{
  Task<TResponse> Dispatch<TResponse>(ICommand command) where TResponse : class;
}
