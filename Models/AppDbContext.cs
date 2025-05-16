using Microsoft.EntityFrameworkCore;
using TodoListMVC.Models; 

namespace TodoListMVC.Data 
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
