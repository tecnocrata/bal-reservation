
namespace Shared.Entities.ValueObjects;

public class DateTimeValueObject : ValueObject
{
  public DateTime Value { get; }

  public DateTimeValueObject(DateTime value)
  {
    Value = value;
  }

  protected override IEnumerable<object> GetAtomicValues()
  {
    yield return Value;
  }
}
