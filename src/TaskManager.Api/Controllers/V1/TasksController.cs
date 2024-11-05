using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Contracts.Entities;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Models.TaskAggregate.Dtos;
using TaskManager.Domain.Notifier;
using TaskManager.Domain.Validator;
using TaskManager.S3.Model.File;

namespace TaskManager.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tasks")]
public class TasksController(
    ITaskService taskService,
    IValidatorGeneric validatorFactory,
    INotifierMessage notifierMessage,
    IFileService fileService) :
    MainController(validatorFactory, notifierMessage)
{
    /// <summary>
    ///     Pesquisa tarefas por paginação
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 500)
    {
        var validFilter = new PaginationFilter(pageNumber, pageSize);
        var requestFilter = new RequestFilter(Request.Scheme, Request.Host.ToUriComponent(), Request.Path.Value);

        var response = taskService.Get(validFilter, requestFilter);

        return CustomResponse(response);
    }

    /// <summary>
    ///     Pesquisa uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var task = taskService.Get(id);

        return task is null
            ? CustomResponse(httpStatusCode: HttpStatusCode.NotFound)
            : CustomResponse(task);
    }

    /// <summary>
    ///     Cadastra uma tarefa
    /// </summary>
    /// <param name="registerTaskDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterTaskDto registerTaskDto)
    {
        if (!await Validate(registerTaskDto)) return CustomResponse();

        var id = await taskService.RegisterAsync(registerTaskDto);

        return CustomResponse(id, HttpStatusCode.Created);
    }

    /// <summary>
    ///     Atualiza uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <param name="registerTaskDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RegisterTaskDto registerTaskDto)
    {
        if (!await Validate(registerTaskDto)) return CustomResponse();

        await taskService.UpdateAsync(id, registerTaskDto);
        return CustomResponse(id);
    }

    /// <summary>
    ///     Exclui uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = taskService.Get(id);

        if (task == null)
            return CustomResponse(httpStatusCode: HttpStatusCode.NotFound);

        if (!string.IsNullOrEmpty(task.AttachmentKey))
            await fileService.DeleteFile(task.AttachmentKey);

        await taskService.RemoveAsync(id);
        return CustomResponse();
    }
}