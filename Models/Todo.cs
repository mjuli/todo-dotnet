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
        public Todo(string title, Guid categoryId)
        {
            Title = title;
            CategoryId = categoryId;
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; private set; }

        public void Update(string title, bool isDone, Guid categoryId)
        {
            Title = title;
            IsDone = isDone;
            UpdatedAt = DateTime.Now;
            CategoryId = categoryId;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}