namespace TaskManager.S3.Configs;

public class AwsS3Configuration
{
    public string BucketName { get; set; }
    public string FolderPathName { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}