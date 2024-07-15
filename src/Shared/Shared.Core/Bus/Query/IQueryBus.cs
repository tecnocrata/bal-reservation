using System.Threading.Tasks;

namespace Shared.Core.Bus.Query;
public interface IQueryBus
{
  Task<TResponse> Ask<TResponse>(IQuery request) where TResponse : class;
}
