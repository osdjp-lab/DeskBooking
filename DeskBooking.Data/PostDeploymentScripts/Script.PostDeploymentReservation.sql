MERGE INTO Reservation AS Target
USING (VALUES 
        (1, 1, 1, '2022-05-27 17:01:15', '2022-05-28 17:01:15'),
        (2, 3, 2, '2022-05-28 17:01:15', '2022-05-31 17:01:15'),
        (3, 5, 3, '2022-05-25 17:01:15', '2022-05-29 17:01:15'),
        (4, 7, 4, '2022-05-26 17:01:15', '2022-05-28 17:01:15')
)
AS Source (ReservationId, DeskId, UserId, StartDate, DurationInDays)
ON Target.ReservationId = Source.ReservationId
WHEN NOT MATCHED BY TARGET THEN
INSERT (ReservationId, DeskId, UserId, StartDate, DurationInDays)
VALUES (ReservationId, DeskId, UserId, StartDate, DurationInDays);
