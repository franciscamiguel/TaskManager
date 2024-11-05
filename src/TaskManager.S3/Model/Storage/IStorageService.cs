using TaskManager.S3.Model.Storage.Entities;

namespace TaskManager.S3.Model.Storage;

public interface IStorageService
{
    /// <summary>
    ///     Update a file by key
    /// </summary>
    /// <param name="file"></param>
    /// <param name="oldKey"></param>
    /// <returns></returns>
    Task<string> UpdateFile(AwsObject file, string oldKey);

    /// <summary>
    ///     Update a file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<string> UploadFile(AwsObject file);

    /// <summary>
    ///     Get file by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<AwsObjectResponse> GetFile(string key);

    /// <summary>
    ///     Delete a file by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task DeleteFile(string key);

    /// <summary>
    ///     xx
    ///     Returns list of all keys deleted
    /// </summary>
    Task<List<string>> DeleteFiles(List<string> keys);
}