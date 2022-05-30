MERGE INTO Location AS Target 
USING (VALUES 
        (1, 'Polska', 'Kraków', 'Jana Pawła II'), 
        (2, 'Polska', 'Warszawa', 'Jutrzenki'), 
        (3, 'Polska', 'Rzeszów', 'Litewska'),
		(4, 'Polska', 'Bielsko-Biała', 'Legionów'),
		(5, 'Polska', 'Wrocław', 'Szybowcowa'),
        (6, 'USA', 'Dallas', 'Oak Lawn Avenue')
) 
AS Source (LocationId, Country, City, Street) 
ON Target.LocationId = Source.LocationId 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (LocationId, Country, City, Street) 
VALUES (LocationId, Country, City, Street);
