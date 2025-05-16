using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListMVC.Models
{
    public enum Priority { Low, Medium, High }
    public enum Status { Pending, Done, Deleted }

    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public required string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; } = Status.Pending;
    }
}
