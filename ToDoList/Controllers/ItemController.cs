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
        public async Task<IActionResult> Index( DateTime? date)
        {
            var selectedDate = date?.Date ?? DateTime.Today;
            var item = await _context.Items
                .Where(i=>i.DueAt.Date == selectedDate)
                .OrderBy(i=>i.DueAt)
                .ToListAsync();

            ViewBag.SelectedDate = selectedDate;

            return View(item);
        }
        public IActionResult Create(DateTime? date) { 
            var item = new Item();
            if (date.HasValue)
            {
                item.DueAt = date.Value;
            }
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, Finished, DueAt")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, Finished, DueAt")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
