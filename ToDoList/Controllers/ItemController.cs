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
            var items = await _context.Items
                .Where(i=>i.DueAt.Date == selectedDate)
                .OrderBy(i=>i.DueAt)
                .ToListAsync();

            // Zadania na dziś
            ViewBag.SelectedDate = selectedDate;

            var nextDay = selectedDate.AddDays(1);
            var futureTasks = await _context.Items
                .Where(i => i.DueAt.Date >= nextDay)
                .OrderBy(i => i.DueAt)
                .ToListAsync();

            // Zadania w przyszłości
            ViewBag.FutureTasksCount = futureTasks.Count;

            var nextTask = futureTasks.FirstOrDefault();

            // Najbliższe zadanie w przyszłości
            ViewBag.NextTaskName = nextTask?.Name ?? "-";
            ViewBag.NextTaskDue = nextTask?.DueAt.ToString("dd MMM yyyy") ?? "-";

            var now = DateTime.Now;
            var soonTasks = await _context.Items
                .Where(i => !i.Finished && i.DueAt > now && i.DueAt <= now.AddHours(1))
                .OrderBy(i => i.DueAt)
                .ToListAsync();
            // Zadania w ciągu godziny
            ViewBag.SoonTasks = soonTasks;

            var forgottenTasks = await _context.Items
                .Where(i => !i.Finished && i.DueAt < now)
                .OrderBy(i => i.DueAt)
                .ToListAsync();

            // Zadania po terminie
            ViewBag.ForgottenTasks = forgottenTasks;

            return View(items);
        }
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            item.Finished = !item.Finished;
            _context.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
            //return RedirectToAction("Index");
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
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
                //return RedirectToAction("Index");
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
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
                //return RedirectToAction("Index");
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
            return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
            //return RedirectToAction("Index");
        }
    }
}
