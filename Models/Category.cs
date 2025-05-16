using System.ComponentModel.DataAnnotations;

namespace TodoListMVC.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
