namespace Shared.Entities.ValueObjects;

public class EnumValueObject<TEnum> where TEnum : Enum
{
  public TEnum Value { get; init; }

  public EnumValueObject(TEnum value)
  {
    Value = value;
  }

  public EnumValueObject(string value)
  {
    if (value == null)
    {
      throw new ArgumentNullException(nameof(value));
    }

    if (!Enum.TryParse(typeof(TEnum), value, ignoreCase: true, out var tempValue))
    {
      throw new ArgumentException($"{value} is not a valid value for {typeof(TEnum).Name}");
    }

    Value = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase: true);
  }

  public override bool Equals(object? obj)
  {
    if (obj is not EnumValueObject<TEnum> other)
    {
      return false;
    }

    return Value.Equals(other.Value);
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public override string ToString()
  {
    return Value.ToString();
  }
}