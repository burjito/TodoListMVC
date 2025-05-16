using System.ComponentModel.DataAnnotations;

namespace TodoListMVC.Models
{
    public enum Category { Home, School, Personal }
    public enum Priority { Low, Medium, High }
    public enum Status { Pending, Done }

    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public required string Description { get; set; }

        public Category Category { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; } = Status.Pending;

        public bool IsHidden { get; set; } = false;
    }
}
