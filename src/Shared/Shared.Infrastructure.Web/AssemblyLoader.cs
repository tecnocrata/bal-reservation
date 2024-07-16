using System.Collections.Concurrent;
using System.Reflection;

namespace Shared.Infrastructure.Web;
public static class AssemblyLoader
{
  private static readonly ConcurrentDictionary<string, Assembly> _assemblies =
      new ConcurrentDictionary<string, Assembly>();

  public static Assembly GetInstance(string key)
  {
    return _assemblies.GetOrAdd(key, Assembly.Load(key));
  }
}