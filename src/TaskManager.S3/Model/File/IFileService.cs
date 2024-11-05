using TaskManager.Domain.Dtos;
using TaskManager.S3.Model.Storage.Entities;

namespace TaskManager.S3.Model.File;

public interface IFileService
{
    Task<string> UploadFile(AttachmentDto attachment, int entityId, string folderPathName);
    Task<AwsObjectResponse> DownloadFile(string key);
    Task DeleteFile(string key);
    Task<List<string>> DeleteMultipleFiles(List<string> keys);
}