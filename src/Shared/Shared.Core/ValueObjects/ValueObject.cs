﻿namespace Shared.Entities.ValueObjects;

public abstract class ValueObject
{
  protected static bool EqualOperator(ValueObject left, ValueObject right)
  {
    if (left is null ^ right is null)
    {
      return false;
    }

    return left is null || left.Equals(right);
  }

  protected static bool NotEqualOperator(ValueObject left, ValueObject right)
  {
    return !EqualOperator(left, right);
  }

  protected abstract IEnumerable<object> GetAtomicValues();

  public override bool Equals(object? obj)
  {
    if (obj == null || obj.GetType() != this.GetType())
    {
      return false;
    }

    var other = (ValueObject)obj;
    var thisValues = this.GetAtomicValues().GetEnumerator();
    var otherValues = other.GetAtomicValues().GetEnumerator();
    while (thisValues.MoveNext() && otherValues.MoveNext())
    {
      if (thisValues.Current is null ^
          otherValues.Current is null)
      {
        return false;
      }

      if (thisValues.Current != null &&
          !thisValues.Current.Equals(otherValues.Current))
      {
        return false;
      }
    }

    return !thisValues.MoveNext() && !otherValues.MoveNext();
  }

  public override int GetHashCode()
  {
    return this.GetAtomicValues()
        .Select(x => x != null ? x.GetHashCode() : 0)
        .Aggregate((x, y) => x ^ y);
  }
}
