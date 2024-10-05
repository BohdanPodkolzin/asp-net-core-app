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
                .Where(c => c.Category?.Type == "Income")
                .Sum(t => t.Amount);
            ViewBag.TotalIncome = totalIncome;

            var totalExpense= selectedTransactions
                .Where(c => c.Category?.Type == "Expense")
                .Sum(t => t.Amount);
            ViewBag.TotalExpense = totalExpense;

            var balance = totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0");

            // donut
            ViewBag.DoughnutChartData = selectedTransactions
                .Where(c => c.Category?.Type == "Expense")
                .GroupBy(c => c.Category.CategoryId)
                .Select(n => new
                {
                    amount = n.Sum(a => a.Amount),
                    formattedAmount = n.Sum(a => a.Amount).ToString("C0"),
                    categoryIconWithTitle = n.First().Category.Icon + " " + n.First().Category.Title

                })
                .OrderBy(x => x.amount)
                .ToList();


            return View();
        }
    }
}
