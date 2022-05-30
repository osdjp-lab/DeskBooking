MERGE INTO SystemUser AS Target 
USING (VALUES 
        (1, 'Admin', 'Admin', 'admin@mail.com', 'admin', 'True'),
		(2, 'Tom', 'Harding', 'tomharding@mail.com', 'tom', 'False'),
		(3, 'Mathew', 'Oconel', 'mathewoconel@mail.com', 'mathew', 'False'),
		(4, 'John', 'Prescot', 'johnprescot@mail.com', 'john', 'False'),
		(5, 'David', 'Langley', 'davidlangley@mail.com', 'david', 'False'),
		(6, 'Harold', 'Thomson', 'haroldthomson@mail.com', 'harold', 'False')
) 
AS Source (SystemUserId, Name, Surname, Email, Password, isAdmin) 
ON Target.SystemUserId = Source.SystemUserId
WHEN NOT MATCHED BY TARGET THEN 
INSERT (SystemUserId, Name, Surname, Email, Password, isAdmin) 
VALUES (SystemUserId, Name, Surname, Email, Password, isAdmin);
