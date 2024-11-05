namespace TaskManager.Api.DTOs;

public class CustomResponse
{
    public bool Success { get; set; }

    public int Status { get; set; }

    public DateTimeOffset DateUtc { get; private set; } = DateTimeOffset.UtcNow;

    /// <summary>
    ///     Quando a operação for sucesso, data terá valores
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    ///     Lista de mensagens de erro
    /// </summary>
    public string[] Messages { get; set; }
}