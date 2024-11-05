namespace TaskManager.Domain.Extensions;

public static class S3Extension
{
    public static string SetKey(this string fileName, string folderPatchName, int id)
    {
        return $"{folderPatchName}{id}_{fileName}";
    }
}