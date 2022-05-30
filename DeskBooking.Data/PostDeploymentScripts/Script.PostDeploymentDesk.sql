MERGE INTO Desk AS Target 
USING (VALUES 
        (1, 1, 'By the window'),
        (2, 1, 'Near the toilets'),
		(3, 2, 'By the door'),
		(4, 2, 'Near the stairs'),
		(5, 3, 'By the cafeteria'),
		(6, 3, 'Private room'),
		(7, 4, 'Weak sygnal'),
		(8, 4, 'New setup'),
		(9, 5, 'Weak computer'),
		(10, 5, 'Out-of-date software'),
		(11, 6, 'Comfortable chair'),
		(12, 6, 'Wooden chair')
) 
AS Source (DeskId, LocationId, Note) 
ON Target.DeskId = Source.DeskId
WHEN NOT MATCHED BY TARGET THEN 
INSERT (DeskId, LocationId, Note) 
VALUES (DeskId, LocationId, Note);
