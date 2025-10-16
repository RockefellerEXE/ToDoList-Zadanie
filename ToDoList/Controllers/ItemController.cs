using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.DAL;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemContext _context;
        public ItemController(ItemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var item = _context.Items.ToListAsync();
            return View(item);
        }
        public IActionResult Create() { 
            return View();
        }
    }
}
