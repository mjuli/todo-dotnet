using TodoApp.Models;

namespace TodoApp.Dtos
{
    public record CategoryInputDto(string Title, bool IsDone);
}