using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Domain.Notifier;
using TaskManager.Domain.Validator;

namespace TaskManager.Api.Controllers;

[ApiController]
public class MainController(
    IValidatorGeneric validadorFactory,
    INotifierMessage notifierMessage) : ControllerBase
{
    /// <summary>
    ///     Retorna resposta personalizada
    /// </summary>
    /// <param name="data"></param>
    /// <param name="httpStatusCode"></param>
    /// <returns></returns>
    protected ActionResult CustomResponse(object data = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        if (!notifierMessage.IsValid())
        {
            var messages = notifierMessage.GetMessages().ToArray();
            var resultFail = new CustomResponse
            {
                Success = false,
                Status = (int)HttpStatusCode.BadRequest,
                Data = null,
                Messages = messages
            };
            return BadRequest(resultFail);
        }

        var resultSuccess = new CustomResponse
        {
            Success = true,
            Status = (int)httpStatusCode,
            Data = data,
            Messages = null
        };

        return httpStatusCode switch
        {
            HttpStatusCode.Created => Created(string.Empty, resultSuccess),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.NotFound => NotFound(),
            HttpStatusCode.OK => Ok(resultSuccess),
            _ => Ok()
        };
    }

    /// <summary>
    ///     Valida todas as classes de Entities e Dtos
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected async Task<bool> Validate<T>(T data)
    {
        var validatorResult = await validadorFactory.GetValidator<T>()
            .ValidateAsync(data);

        if (validatorResult.IsValid) return true;

        notifierMessage.AddRange(validatorResult.Errors?.Select(e => e.ErrorMessage)
            .ToList());

        return false;
    }
}