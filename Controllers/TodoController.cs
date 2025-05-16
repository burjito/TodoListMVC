using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListMVC.Models;
using System.Linq;
using TodoListMVC.Data;

namespace TodoListMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string category, string priority, string status, string sortOrder)
        {
            var tasks = _context.TodoItems.Where(t => !t.IsHidden).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                tasks = tasks.Where(t => t.Category.ToString() == category);

            if (!string.IsNullOrEmpty(priority))
                tasks = tasks.Where(t => t.Priority.ToString() == priority);

            if (!string.IsNullOrEmpty(status))
                tasks = tasks.Where(t => t.Status.ToString() == status);

            // Sorting logic
            tasks = sortOrder switch
            {
                "title_desc" => tasks.OrderByDescending(t => t.Title),
                "priority_asc" => tasks.OrderBy(t => t.Priority),
                "priority_desc" => tasks.OrderByDescending(t => t.Priority),
                "title_asc" => tasks.OrderBy(t => t.Title),
                _ => tasks.OrderBy(t => t.Id) // Default sorting (by Id)
            };

            // Pass current sortOrder to view via ViewData for UI state
            ViewData["CurrentSort"] = sortOrder ?? "";

            return View(tasks.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TodoItem item)
        {
            if (!ModelState.IsValid)
                return View(item);

            var existingItem = _context.TodoItems.Find(item.Id);
            if (existingItem == null)
                return NotFound();

            // Update fields explicitly to avoid overposting
            existingItem.Title = item.Title;
            existingItem.Description = item.Description;
            existingItem.Category = item.Category;
            existingItem.Priority = item.Priority;
            existingItem.Status = item.Status;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // GET: Delete confirmation page
        public IActionResult Delete(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: Perform the actual deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            _context.TodoItems.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Hide(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            item.IsHidden = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult MarkAsDone(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            item.Status = Status.Done;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
