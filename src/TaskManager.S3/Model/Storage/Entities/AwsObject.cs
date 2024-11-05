namespace TaskManager.S3.Model.Storage.Entities;

public class AwsObject(Stream file, string fileName, string contentType)
{
    public Stream File { get; private set; } = file;
    public string FileName { get; private set; } = fileName;
    public string ContentType { get; private set; } = contentType;
}