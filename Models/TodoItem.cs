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
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        // Make this nullable so it isn't implicitly required
        public Category? Category { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; } = Status.Pending;
    }
}
