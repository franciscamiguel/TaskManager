CREATE PROCEDURE usp_UpdateTask
    @Id INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(255),
    @IsCompleted BIT
AS
BEGIN
UPDATE Tasks
SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted
WHERE Id = @Id;
END;