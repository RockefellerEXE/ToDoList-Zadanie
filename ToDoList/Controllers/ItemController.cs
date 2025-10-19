using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.DAL;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ItemsContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ItemController(ItemsContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetCurrentUserId()
        {
            return _userManager.GetUserId(User);
        }
        public async Task<IActionResult> Index( DateTime? date)
        {
            var userId = GetCurrentUserId();
            var selectedDate = date?.Date ?? DateTime.Today;
            var items = await _context.Items
                .Where(i=>i.UserId == userId && i.DueAt.Date == selectedDate)
                .OrderBy(i=>i.DueAt)
                .ToListAsync();

            // Zadania na dziś
            ViewBag.SelectedDate = selectedDate;

            var nextDay = selectedDate.AddDays(1);
            var futureTasks = await _context.Items
                .Where(i => i.UserId == userId && i.DueAt.Date >= nextDay)
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
                .Where(i => i.UserId == userId && !i.Finished && i.DueAt > now && i.DueAt <= now.AddHours(1))
                .OrderBy(i => i.DueAt)
                .ToListAsync();
            // Zadania w ciągu godziny
            ViewBag.SoonTasks = soonTasks;

            var forgottenTasks = await _context.Items
                .Where(i => i.UserId == userId && !i.Finished && i.DueAt < now)
                .OrderBy(i => i.DueAt)
                .ToListAsync();

            // Zadania po terminie
            ViewBag.ForgottenTasks = forgottenTasks;

            return View(items);
        }
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id && i.UserId == GetCurrentUserId());
            if (item != null)
            {
                item.Finished = !item.Finished;
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
            }
            return RedirectToAction("Index", new { date = DateTime.Now.ToString("yyyy-MM-dd") });

        }
        public IActionResult Create(DateTime? date) {
            var item = new Item();
            item.DueAt = date ?? DateTime.Today;
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, Finished, DueAt")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.UserId = GetCurrentUserId();
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
                //return RedirectToAction("Index");
            }
            //else
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors)
            //                                  .Select(e => e.ErrorMessage)
            //                                  .ToList();

            //}
            return View(item);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == GetCurrentUserId());
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, Finished, DueAt")] Item item)
        {

            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == GetCurrentUserId());

            if (ModelState.IsValid && existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.Finished = item.Finished;
                existingItem.DueAt = item.DueAt;
                _context.Update(existingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
                //return RedirectToAction("Index");
            }
            return View(item);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == GetCurrentUserId());
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == GetCurrentUserId());
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { date = item.DueAt.Date.ToString("yyyy-MM-dd") });
            }
            //return RedirectToAction("Index");
            return RedirectToAction("Index", new { date = DateTime.Now.ToString("yyyy-MM-dd") });
        }
    }
}
