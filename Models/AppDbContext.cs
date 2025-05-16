using Microsoft.EntityFrameworkCore;
using TodoListMVC.Models; // Adjust based on your model namespace

namespace TodoListMVC.Data // This should match what you're using in `Program.cs`
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
