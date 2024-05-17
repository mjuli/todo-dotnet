using TodoApp.Models;

namespace TodoApp.Dtos
{
    public record TodoCreateInputDto(string Title, Guid CategoryId);
}