CREATE PROCEDURE usp_UpdateAwsKey
    @Id INT,
    @AttachmentKey NVARCHAR(2000)
AS
BEGIN
    UPDATE Tasks
    SET AttachmentKey = @AttachmentKey
    WHERE Id = @Id;
END;
