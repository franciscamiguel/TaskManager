CREATE PROCEDURE usp_GetTaskById
    @Id INT
AS
BEGIN
SELECT * FROM Tasks WHERE Id = @Id;
END;