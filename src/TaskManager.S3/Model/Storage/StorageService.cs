using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Domain.Notifier;
using TaskManager.S3.Configs;
using TaskManager.S3.Model.Storage.Entities;

namespace TaskManager.S3.Model.Storage;

public class StorageService(
    IAmazonS3 amazonS3,
    IOptions<AwsS3Configuration> settings,
    INotifierMessage notifierService,
    ILogger<StorageService> logger)
    : IStorageService
{
    private readonly AwsS3Configuration _settings = settings.Value;

    public async Task<string> UpdateFile(AwsObject file, string oldKey)
    {
        await DeleteFile(oldKey);
        return await UploadFile(file);
    }

    public async Task<string> UploadFile(AwsObject file)
    {
        logger.LogInformation("Iniciando upload do arquivo {FileName} para o bucket {BucketName}",
            file.FileName, _settings.BucketName);

        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{_settings.FolderPathName}/{file.FileName}",
                InputStream = file.File,
                ContentType = file.ContentType
            };

            await amazonS3.PutObjectAsync(request);

            logger.LogInformation("Upload do arquivo {FileName} concluído com sucesso", file.FileName);

            return file.FileName;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, "Erro desconhecido ao fazer upload do arquivo {FileName}", file.FileName);
            notifierService.Add(BuildMessage(ex));
            return null;
        }
        catch (Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
            return null;
        }
    }

    public async Task<AwsObjectResponse> GetFile(string key)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{_settings.FolderPathName}/{key}"
            };

            var response = await amazonS3.GetObjectAsync(request);

            return response.HttpStatusCode == HttpStatusCode.OK
                ? new AwsObjectResponse(response.ResponseStream, response.Headers["Content-Type"])
                : null;
        }
        catch (AmazonS3Exception ex)
        {
            var exceptionMessage = ex.StatusCode == HttpStatusCode.NotFound
                ? FormatMessageNotFound(key)
                : BuildMessage(ex);

            notifierService.Add(exceptionMessage);
            return null;
        }
        catch (Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
            return null;
        }
    }

    public async Task DeleteFile(string key)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{_settings.FolderPathName}/{key}"
            };

            await amazonS3.DeleteObjectAsync(request);
        }
        catch (AmazonS3Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
        }
        catch (Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
        }
    }

    /// <summary>
    ///     Returns list of all keys deleted
    /// </summary>
    public async Task<List<string>> DeleteFiles(List<string> keys)
    {
        if (keys is null || keys.Count == 0)
        {
            notifierService.Add("Lista de Chaves está vazia");
            return [];
        }

        try
        {
            var response = await amazonS3.DeleteObjectsAsync(BuildKeyVersion(keys));

            if (response.DeletedObjects.Count == keys.Count)
                return await Task.Run(() => response.DeletedObjects?
                    .Select(p => p.Key)
                    .ToList());


            notifierService.Add("Occoreu um erro ao deletar um ou mais arquivos");

            response.DeleteErrors?.ForEach(p =>
            {
                notifierService.Add($"{_settings.FolderPathName}/{p.Key} - {p.Code} - {p.Message}");
            });

            return response.DeletedObjects?
                .Select(p => p.Key)
                .ToList();
        }

        catch (DeleteObjectsException ex)
        {
            notifierService.Add(BuildMessage(ex));
            PrintDeletionErrorStatus(ex);
            return [];
        }
        catch (AmazonS3Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
            return [];
        }
        catch (Exception ex)
        {
            notifierService.Add(BuildMessage(ex));
            return [];
        }
    }

    public List<string> GetMessages()
    {
        return notifierService.GetMessages();
    }

    public bool IsValid()
    {
        return notifierService.IsValid();
    }

    #region Private Methods

    private void PrintDeletionErrorStatus(DeleteObjectsException ex)
    {
        var errorResponse = ex.Response;
        foreach (var deleteError in errorResponse.DeleteErrors)
            notifierService.Add($"{deleteError.Key} - {deleteError.Code} - {deleteError.Message}");
    }

    private DeleteObjectsRequest BuildKeyVersion(List<string> keys)
    {
        var keyVersions = keys.Select(key => new KeyVersion { Key = $"{_settings.FolderPathName}/{key}" }).ToList();

        return new DeleteObjectsRequest
        {
            BucketName = _settings.BucketName,
            Objects = keyVersions,
            Quiet = false
        };
    }

    private static string FormatMessageNotFound(string key)
    {
        return $"Arquivo não encontrado. Chave: {key}";
    }

    private static string BuildMessage(Exception ex)
    {
        return $"{ex.Message}. {ex.InnerException?.Message}";
    }

    #endregion Private Methods
}