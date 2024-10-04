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
        public IActionResult AddOrEdit()
        {
            ViewData["CategoryId"] = new SelectList(context.Categories, "CategoryId", "CategoryId");
            return View();
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
                context.Add(transaction);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(context.Categories, "CategoryId", "CategoryId", transaction.CategoryId);
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
    }
}
