using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDoList.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Finished { get; set; }

        public DateTime DueAt { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

    }
}
