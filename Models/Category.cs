using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TodoApp.Models
{
    public class Category
    {
        public Category(string title)
        {
            Title = title;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
            Todos = new List<Todo>();
        }

        [PrimaryKey]
        public Guid Id { get; init; }
        public string Title { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Todo> Todos { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; private set; }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}