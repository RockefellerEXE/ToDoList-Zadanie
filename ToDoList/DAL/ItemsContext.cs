
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.DAL
{
    public class ItemsContext : DbContext
    {
        public ItemsContext(DbContextOptions<ItemsContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}
