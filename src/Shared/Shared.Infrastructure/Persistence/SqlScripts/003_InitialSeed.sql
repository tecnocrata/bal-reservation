USE Restaurant;

-- Insert data into the Users table
INSERT INTO Users (Id, UserName, Password)
VALUES
    (NEWID(), 'eortuno', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92'), -- '123456' (hashed with SHA-256)
    (NEWID(), 'enrique.ortuno', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92'), -- '123456' (hashed with SHA-256)
    (NEWID(), 'enrique', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92'); -- '123456' (hashed with SHA-256)

-- Insert data into the Reservations table
DECLARE @i INT = 0;
WHILE @i < 10
BEGIN
    INSERT INTO Reservations (Id, Status, Date, Name, NumberOfGuests)
    VALUES (
        NEWID(),
        'Open',
        DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 15), GETDATE()) + '19:00:00', -- Random date between today and 15 days in the future, starting from 7 PM
        CONCAT('Guest', @i + 1),
        ABS(CHECKSUM(NEWID()) % 10) + 1 -- Random number of guests between 1 and 10
    );
    SET @i = @i + 1;
END;