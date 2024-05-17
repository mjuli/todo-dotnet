using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll(AppDbContext context)
        {
            var todos = context.Todos.Where(todo => !todo.IsDeleted).ToList();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id, AppDbContext context)
        {
            var todo = context.Todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public IActionResult Create(TodoCreateInputDto todo, AppDbContext context)
        {
            if (string.IsNullOrWhiteSpace(todo.Title) || todo.Category == null)
                return BadRequest();

            Todo newTodo = new Todo(todo.Title, todo.Category);

            context.Todos.Add(newTodo);
            context.SaveChanges();

            var todoOutput = new TodoOutputDto(newTodo.Title, newTodo.IsDone, newTodo.Category, newTodo.CreatedAt);

            return Created(newTodo.Id.ToString(), todoOutput);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, TodoUpdateInputDto todoDto, AppDbContext context)
        {
            var todo = context.Todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(todoDto.Title) || todoDto.Category == null)
                return BadRequest();

            todo.Update(todoDto.Title, todoDto.IsDone, todoDto.Category);
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id, AppDbContext context)
        {
            var todo = context.Todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
                return NotFound();

            todo.Delete();
            context.SaveChanges();

            return NoContent();
        }

        [HttpGet("/category")]
        public IActionResult GetCategory([FromQuery] CategoryInputDto request, AppDbContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest();
            }

            var category = context.Categories.SingleOrDefault(c => !c.IsDeleted && c.Title == request.Title);
            if (category == null)
            {
                return NotFound();
            }

            var todos = context.Todos.Where(t => !t.IsDeleted && t.CategoryId == category.Id).ToList();

            return Ok(todos);
        }

        // [HttpPost("send-file")]
        // public IActionResult SendFile([FromForm] SendFileInputDto request)
        // {
        //     var stream = request.file.OpenReadStream();
        //     return Ok();
        // }
    }
}