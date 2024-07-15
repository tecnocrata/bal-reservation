using System;

namespace Shared.Core.Bus.Query;
public class QueryNotRegisteredException : Exception
{
  public QueryNotRegisteredException(IQuery query) : base(
      $"The query {query} has not a query handler associated")
  {
  }
}
