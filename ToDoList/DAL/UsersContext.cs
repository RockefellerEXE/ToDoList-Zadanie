using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.DAL
{
    public class UsersContext : IdentityDbContext<AppUser,AppRole, int>
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }
    }
}
