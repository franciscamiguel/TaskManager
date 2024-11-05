namespace TaskManager.Domain.Extensions;

public class Response<T>
{
    protected Response(IEnumerable<T> data)
    {
        Succeeded = data is not null;
        Message = data is not null ? string.Empty : "A consulta não retornou resultados";
        Data = data;
    }

    public bool Succeeded { get; private set; }
    public string Message { get; private set; }
    public IEnumerable<T> Data { get; private set; }
}