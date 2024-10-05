using ExpenceTracker.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenceTracker.Controllers
{
    public class DashboardController(TrackerContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            // past 7 days
            var startDate = DateTime.Today.AddDays(-6);
            var endDate = DateTime.Today;

            var selectedTransactions = await context.Transactions
                .Include(t => t.Category)
                .Where(c => startDate <= c.Date && c.Date <= endDate)
                .ToListAsync();

            var totalIncome = selectedTransactions
                .Where(c => c.Category.Type == "Income")
                .Sum(t => t.Amount);
            ViewBag.TotalIncome = totalIncome;

            var totalExpense= selectedTransactions
                .Where(c => c.Category.Type == "Expense")
                .Sum(t => t.Amount);
            ViewBag.TotalIncome = totalExpense;

            int balance = totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0");


            return View();
        }
    }
}
