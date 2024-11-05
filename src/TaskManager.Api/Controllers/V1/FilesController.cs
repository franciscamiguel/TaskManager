using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Contracts.Entities;
using TaskManager.Domain.Dtos;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Notifier;
using TaskManager.Domain.Validator;
using TaskManager.S3.Model.Storage;
using TaskManager.S3.Model.Storage.Entities;

namespace TaskManager.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/files")]
public class FilesController(
    IValidatorGeneric validatorFactory,
    INotifierMessage notifierMessage,
    ITaskService taskService,
    IStorageService storageService) :
    MainController(validatorFactory, notifierMessage)
{
    private readonly INotifierMessage _notifierMessage = notifierMessage;

    /// <summary>
    ///     Salva um arquivo no S3
    /// </summary>
    /// <param name="attachment"></param>
    /// <param name="entityId"></param>
    /// <param name="folderPathName"></param>
    /// <returns></returns>
    [HttpPost("upload/{entityId:int}")]
    public async Task<IActionResult> UploadFile([FromForm] AttachmentDto attachment, int entityId,
        string folderPathName)
    {
        if (attachment == null || attachment.File.Length == 0)
            return BadRequest("Arquivo não enviado ou está vazio");

        var awsKey = attachment.File.FileName.SetKey(folderPathName, entityId);
        var awsObject = new AwsObject
        (
            attachment.File.OpenReadStream(),
            awsKey,
            attachment.File.ContentType
        );

        var result = await storageService.UploadFile(awsObject);

        if (!_notifierMessage.IsValid())
            return CustomResponse(_notifierMessage.GetMessages());

        await taskService.UpdateAttachmentKeyAsync(entityId, result);
        return CustomResponse();
    }

    /// <summary>
    ///     Atualiza um arquivo no S3
    /// </summary>
    /// <param name="attachment"></param>
    /// <param name="entityId"></param>
    /// <param name="folderPathName"></param>
    /// <param name="attachmentKeyOld"></param>
    /// <returns></returns>
    [HttpPost("update-attachment/{entityId:int}")]
    public async Task<IActionResult> UpdateAttachment(
        [FromForm] AttachmentDto attachment,
        int entityId,
        string folderPathName,
        string attachmentKeyOld
    )
    {
        if (attachment == null || attachment.File.Length == 0)
            return BadRequest("Arquivo não enviado ou está vazio");

        var attachmentKey = attachment.File.FileName.SetKey(folderPathName, entityId);
        var awsObject = new AwsObject
        (
            attachment.File.OpenReadStream(),
            attachmentKey,
            attachment.File.ContentType
        );

        await storageService.UpdateFile(awsObject, attachmentKeyOld);
        return CustomResponse();
    }

    /// <summary>
    ///     Baixa um arquivo para ser visualizado no navegador
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string key)
    {
        var fileResponse = await storageService.GetFile(key);

        if (fileResponse == null)
            return NotFound(_notifierMessage.GetMessages());

        return File(fileResponse.File, fileResponse.ContentType, key);
    }

    /// <summary>
    ///     Exclui um arquivo
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpDelete("delete/{key}")]
    public async Task<IActionResult> DeleteFile(string key)
    {
        await storageService.DeleteFile(key);
        return CustomResponse();
    }

    /// <summary>
    ///     Exclui arquivos em lote
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    [HttpDelete("delete-multiple")]
    public async Task<IActionResult> DeleteMultipleFiles([FromBody] List<string> keys)
    {
        await storageService.DeleteFiles(keys);
        return CustomResponse();
    }
}