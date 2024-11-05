namespace TaskManager.S3.Model.Storage.Entities;

public class AwsObjectResponse(Stream file, string contentType)
{
    public Stream File { get; private set; } = file;
    public string ContentType { get; private set; } = contentType;
}