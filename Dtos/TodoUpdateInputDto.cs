using TodoApp.Models;

namespace TodoApp.Dtos
{
    public record TodoUpdateInputDto(string Title, bool IsDone, Guid CategoryId);
}