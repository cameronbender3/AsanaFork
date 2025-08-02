ALTER PROCEDURE ProjectInsert 
    @projectName NVARCHAR(255), 
    @description NVARCHAR(MAX),
    @complete_percent INT,
    @id INT OUT
AS

    INSERT INTO Project (NAME, DESCRIPTION, COMPLETE_PERCENT)
    VALUES (@projectName, @description, @complete_percent);

    SELECT @id = SCOPE_IDENTITY();