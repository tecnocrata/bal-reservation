using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core.Bus.Query;

namespace Shared.Infrastructure.Bus.Query;
public class InMemoryQueryBus : IQueryBus
{
  private static readonly ConcurrentDictionary<Type, object> _queryHandlers =
      new ConcurrentDictionary<Type, object>();

  private readonly IServiceProvider _provider;

  public InMemoryQueryBus(IServiceProvider provider)
  {
    _provider = provider;
  }

  public async Task<TResponse> Ask<TResponse>(IQuery request)
          where TResponse : class
  {
    var handler = GetWrappedHandlers<TResponse>(request);

    if (handler == null)
      throw new QueryNotRegisteredException(request);

    return await handler.Handle(request, _provider);
  }

  private QueryHandlerWrapper<TResponse> GetWrappedHandlers<TResponse>(IQuery query)
  {
    Type[] typeArgs = { query.GetType(), typeof(TResponse) };

    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeArgs);
    var wrapperType = typeof(QueryHandlerWrapper<,>).MakeGenericType(typeArgs);

    var handlers = _provider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType)) as IEnumerable;
    if (handlers == null)
    {
      throw new InvalidOperationException($"The query {query.GetType().Name} has not a query handlers associated");
    }

    var handler = handlers.Cast<object>()
    .Select(h =>
    {
      var instance = Activator.CreateInstance(wrapperType);
      if (instance == null)
      {
        throw new InvalidOperationException($"Unable to create an instance of type {wrapperType.FullName}.");
      }

      return (QueryHandlerWrapper<TResponse>)instance;
    })
    .FirstOrDefault();

    if (handler == null)
    {
      throw new InvalidOperationException($"The query {query.GetType().Name} has not a query handler associated");
    }

    var wrappedHandlers = (QueryHandlerWrapper<TResponse>)_queryHandlers.GetOrAdd(query.GetType(), handler);

    return wrappedHandlers;
  }
}
