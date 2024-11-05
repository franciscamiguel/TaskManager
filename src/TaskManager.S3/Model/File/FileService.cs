using TaskManager.Domain.Dtos;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Notifier;
using TaskManager.S3.Model.Storage;
using TaskManager.S3.Model.Storage.Entities;

namespace TaskManager.S3.Model.File;

public class FileService(
    INotifierMessage notifierMessage,
    IStorageService storageService) :
    IFileService
{
    public async Task<string> UploadFile(AttachmentDto attachment, int entityId, string folderPathName)
    {
        if (attachment == null || attachment.File.Length == 0)
            return null;

        var awsKey = attachment.File.FileName.SetKey(folderPathName, entityId);
        var awsObject = new AwsObject
        (
            attachment.File.OpenReadStream(),
            awsKey,
            attachment.File.ContentType
        );
        var result = await storageService.UploadFile(awsObject);

        return result;
    }

    public async Task<AwsObjectResponse> DownloadFile(string key)
    {
        var fileResponse = await storageService.GetFile(key);

        if (fileResponse == null)
            notifierMessage.Add("Não existe arquivo para download");

        return fileResponse;
    }

    public async Task DeleteFile(string key)
    {
        await storageService.DeleteFile(key);
    }

    public async Task<List<string>> DeleteMultipleFiles(List<string> keys)
    {
        return await storageService.DeleteFiles(keys);
    }
}