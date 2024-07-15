using System.Threading.Tasks;

namespace Shared.Core.Bus.Command;
public interface ICommandBus
{
  Task Dispatch(ICommand command);
}
