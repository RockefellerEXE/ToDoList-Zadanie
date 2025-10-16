using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.DAL
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}
