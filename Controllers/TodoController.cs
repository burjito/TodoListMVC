using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListMVC.Data;
using TodoListMVC.Models;
using System.Linq;

namespace TodoListMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;
        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Todo
        public IActionResult Index(string category, string priority, string status, string sortOrder)
        {
            var tasks = _context.TodoItems
                .Include(t => t.Category)
                .Where(t => t.Status != Status.Deleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category) && int.TryParse(category, out int catId))
                tasks = tasks.Where(t => t.CategoryId == catId);

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse(priority, out Priority p))
                tasks = tasks.Where(t => t.Priority == p);

            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, out Status s))
                tasks = tasks.Where(t => t.Status == s);

            tasks = sortOrder switch
            {
                "title_desc"     => tasks.OrderByDescending(t => t.Title),
                "priority_asc"   => tasks.OrderBy(t => t.Priority),
                "priority_desc"  => tasks.OrderByDescending(t => t.Priority),
                "title_asc"      => tasks.OrderBy(t => t.Title),
                _                => tasks.OrderBy(t => t.Id)
            };

            ViewData["CurrentSort"] = sortOrder ?? "";
            ViewData["Categories"]  = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(tasks.ToList());
        }

        // GET: /Todo/Details/5
        public IActionResult Details(int id)
        {
            var item = _context.TodoItems
                .Include(t => t.Category)
                .FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Todo/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = _context.Categories.OrderBy(c => c.Name).ToList();
            return View();
        }

        // POST: /Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = _context.Categories.OrderBy(c => c.Name).ToList();
                return View(item);
            }

            item.Status = Status.Pending;
            _context.TodoItems.Add(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Todo/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null) return NotFound();

            ViewData["Categories"] = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(item);
        }

        // POST: /Todo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = _context.Categories.OrderBy(c => c.Name).ToList();
                return View(item);
            }

            var existing = _context.TodoItems.Find(item.Id);
            if (existing == null) return NotFound();

            existing.Title       = item.Title;
            existing.Description = item.Description;
            existing.CategoryId  = item.CategoryId;
            existing.Priority    = item.Priority;
            existing.Status      = item.Status;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Todo/Delete/5  (soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null) return NotFound();

            item.Status = Status.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: /Todo/MarkAsDone/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsDone(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null) return NotFound();

            item.Status = Status.Done;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: /Todo/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null) return NotFound();

            item.Status = Status.Pending;
            _context.SaveChanges();
            return RedirectToAction(nameof(DeletedTasks));
        }

        // POST: /Todo/DeleteForever/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteForever(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null) return NotFound();

            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(DeletedTasks));
        }

        // GET: /Todo/CompletedTasks
        public IActionResult CompletedTasks()
        {
            var tasks = _context.TodoItems
                .Include(t => t.Category)
                .Where(t => t.Status == Status.Done)
                .OrderByDescending(t => t.Id)
                .ToList();
            return View(tasks);
        }

        // GET: /Todo/DeletedTasks
        public IActionResult DeletedTasks()
        {
            var tasks = _context.TodoItems
                .Include(t => t.Category)
                .Where(t => t.Status == Status.Deleted)
                .OrderByDescending(t => t.Id)
                .ToList();
            return View(tasks);
        }

        // ===== Category Management =====

        // GET: /Todo/Categories
        public IActionResult Categories()
        {
            var cats = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(cats);
        }

        // GET: /Todo/AddCategory
        public IActionResult AddCategory() => View();

        // POST: /Todo/AddCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Categories));
        }

        // GET: /Todo/EditCategory/5
        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Todo/EditCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            var existing = _context.Categories.Find(category.Id);
            if (existing == null) return NotFound();

            existing.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Categories));
        }

        // POST: /Todo/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            if (_context.TodoItems.Any(t => t.CategoryId == id))
            {
                TempData["Error"] = "Cannot delete category in use by tasks.";
                return RedirectToAction(nameof(Categories));
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Categories));
        }
    }
}
