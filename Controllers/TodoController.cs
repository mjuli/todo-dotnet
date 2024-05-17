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
            var todos = context.Todos
                .Where(todo => !todo.IsDeleted)
                .Select(t => TodoUpdate(t, context))
                .ToList();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id, AppDbContext context)
        {
            var todo = context.Todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
                return NotFound();

            return Ok(TodoUpdate(todo, context));
        }

        [HttpPost]
        public IActionResult Create(TodoCreateInputDto todo, AppDbContext context)
        {
            if (string.IsNullOrWhiteSpace(todo.Title) || todo.CategoryId == null)
                return BadRequest();

            var category = context.Categories.SingleOrDefault(c => !c.IsDeleted && c.Id == todo.CategoryId);
            if (category == null)
            {
                return NotFound();
            }

            Todo newTodo = new Todo(todo.Title, todo.CategoryId);

            context.Todos.Add(newTodo);
            context.SaveChanges();

            var todoOutput = new TodoOutputDto(newTodo.Title, newTodo.IsDone, category.Title, newTodo.CreatedAt);

            return Created(nameof(GetById), todoOutput);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, TodoUpdateInputDto todoDto, AppDbContext context)
        {
            var todo = context.Todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(todoDto.Title) || todoDto.CategoryId == null)
                return BadRequest();

            todo.Update(todoDto.Title, todoDto.IsDone, todoDto.CategoryId);
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

        [HttpPost("/category")]
        public IActionResult CreateCategory(CategoryInputDto category, AppDbContext context)
        {
            if (string.IsNullOrWhiteSpace(category.Title))
                return BadRequest();

            Category newCategory = new Category(category.Title);

            context.Categories.Add(newCategory);
            context.SaveChanges();

            var categoryOutput = new CategoryOutputDto(newCategory.Title, newCategory.CreatedAt);

            return Created(nameof(GetById), categoryOutput);
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

            var todos = context.Todos
                .Where(t => !t.IsDeleted && t.CategoryId == category.Id && t.IsDone == request.IsDone)
                .Select(t => new TodoOutputDto(t.Title, t.IsDone, category.Title, t.CreatedAt))
                .ToList();

            return Ok(todos);
        }

        [HttpGet("/category/all")]
        public IActionResult GetAllCategory(AppDbContext context)
        {
            var categories = context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new CategoryOutputDto(c.Title, c.CreatedAt))
                .ToList();

            return Ok(categories);
        }

        private static TodoOutputDto? TodoUpdate(Todo todo, AppDbContext context)
        {
            if (todo.IsDeleted)
                return null;

            Category? category = context.Categories.SingleOrDefault(c => c.Id == todo.CategoryId);
            var title = "Sem categoria";
            if (category != null)
                title = category.Title;

            return new TodoOutputDto(todo.Title, todo.IsDone, title, todo.CreatedAt);
        }

        // [HttpPost("send-file")]
        // public IActionResult SendFile([FromForm] SendFileInputDto request)
        // {
        //     var stream = request.file.OpenReadStream();
        //     return Ok();
        // }
    }
}