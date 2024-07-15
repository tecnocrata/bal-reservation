using System.Threading.Tasks;

namespace Shared.Core.Bus.Query;
public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery
{
  Task<TResponse> Handle(TQuery query);
}
