using System.Globalization;

namespace Shared.Entities.ValueObjects;


public class IntValueObject : ValueObject
{
  public int Value { get; }

  public IntValueObject(int value)
  {
    this.Value = value;
  }

  public override string ToString()
  {
    return this.Value.ToString(NumberFormatInfo.InvariantInfo);
  }

  protected override IEnumerable<object> GetAtomicValues()
  {
    yield return this.Value;
  }

  public override bool Equals(object? obj)
  {
    if (this == obj)
    {
      return true;
    }

    if (obj is not IntValueObject item)
    {
      return false;
    }

    return this.Value == item.Value;
  }

  public static implicit operator int(IntValueObject value) => value.Value;
  public static implicit operator IntValueObject(int value) => new IntValueObject(value);

  public override int GetHashCode()
  {
    return HashCode.Combine(this.Value);
  }
}
