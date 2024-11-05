using Microsoft.AspNetCore.Http;

namespace TaskManager.Domain.Dtos;

public record AttachmentDto
{
    public IFormFile File { get; set; }
}