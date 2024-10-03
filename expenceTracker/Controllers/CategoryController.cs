using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenceTracker.Context;
using expenceTracker.Models;

namespace ExpenceTracker.Controllers
{
    public class CategoryController(TrackerContext context) : Controller
    {
        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id=0)
            => View(id == 0 ? new Category() : context.Categories.Find(id));
        

        // POST: Category/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (category.CategoryId == 0)
            {
                context.Categories.Add(category);
            }
            else 
            {
                context.Categories.Update(category);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Category/Delete/5
        // This action might not be needed, as you are using POST directly
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
