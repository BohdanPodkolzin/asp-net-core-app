using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenceTracker.Context;
using expenceTracker.Models;

namespace ExpenceTracker.Controllers
{
    public class TransactionController(TrackerContext context) : Controller
    {
        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var trackerContext = context.Transactions.Include(t => t.Category);
            return View(await trackerContext.ToListAsync());
        }


        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id)
        {
            await GetCategories();
            return View(id == 0 ? new Transaction() : await context.Transactions.FindAsync(id));
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,Amount,Description,Date,CategoryId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                {
                    context.Add(transaction);
                }
                else
                {
                    context.Update(transaction);
                }
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await GetCategories();
            return View(transaction);
        }


        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                context.Transactions.Remove(transaction);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public async Task GetCategories()
        {
            var categories = await context.Categories.ToListAsync();
            var defaultCategory = new Category() { CategoryId = 0, Title = "Specify Category Name" };
            categories.Insert(0, defaultCategory);
            ViewBag.Categories = categories; 
        }
    }
}
