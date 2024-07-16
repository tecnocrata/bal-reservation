





/*
if (value < DateTime.UtcNow)
    {
      throw new ArgumentException("Reservation date must be in the future.", nameof(value));
    }
    // We can only accept reservations up to 30 days in advance
    if (value > DateTime.UtcNow.AddDays(30))
    {
      throw new ArgumentException("Reservation date must be within 30 days.", nameof(value));
    }
    // We only accept reservations starting at 7:00 PM and ending at 10:00 PM
    if (value.TimeOfDay < new TimeSpan(19, 0, 0) || value.TimeOfDay > new TimeSpan(22, 0, 0))
    {
      throw new ArgumentException("Reservation time must be between 7:00 PM and 10:00 PM.", nameof(value));
    }
*/