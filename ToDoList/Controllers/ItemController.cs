using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            List<Item> lista = new List<Item>();
            var item = new Item() { Name = "Testowa pozycja", Description = "Opis testowej pozycji", Finished = false };
            var item2 = new Item() { Name = "Testowa pozycja2", Description = "Opis testowej pozycji2", Finished = true };
            lista.Add(item);
            lista.Add(item2);
            return View(lista);
        }
    }
}
