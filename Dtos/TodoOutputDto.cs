using TodoApp.Models;

namespace TodoApp.Dtos
{
    public record TodoOutputDto(string Title, bool IsDone, Category Category, DateTime CreatedAt);
}