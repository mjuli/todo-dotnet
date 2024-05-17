using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TodoApp.Models
{
    public class Todo
    {
        public Todo(string title, Category category)
        {
            Title = title;
            CategoryId = category.Id;
            Category = category;
            Id = Guid.NewGuid();
            IsDone = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
        }
        public Guid Id { get; init; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; private set; }

        public void Update(string title, bool isDone, Category category)
        {
            Title = title;
            IsDone = isDone;
            UpdatedAt = DateTime.Now;
            Category = category;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}