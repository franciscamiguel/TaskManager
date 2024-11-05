CREATE PROCEDURE usp_AddTask
    @Title NVARCHAR(100),
    @Description NVARCHAR(255),
    @IsCompleted BIT,
    @Id INT OUTPUT
AS
BEGIN
    INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt)
    VALUES (@Title, @Description, @IsCompleted, GETDATE());

    -- Captura o Id gerado
    SET @Id = SCOPE_IDENTITY();
END;
GO
