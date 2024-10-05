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

            // spline chart ===================================


            // income
            var incomeSummary = selectedTransactions
                .Where(t => t.Category.Type == "Income")
                .GroupBy(t => t.Date)
                .Select(s => new SplineChartData
                {
                    Day = s.First().Date.ToString("dd-MMM"),
                    Income = s.Sum(i => i.Amount),


                })
                .ToList();

            // expense
            var expenseSummary = selectedTransactions
                .Where(t => t.Category.Type == "Expense")
                .GroupBy(t => t.Date)
                .Select(s => new SplineChartData
                {
                    Day = s.First().Date.ToString("dd-MMM"),
                    Expense = s.Sum(i => i.Amount),


                })
                .ToList();

            // Combine Income & Expense
            //skipping day without transactions
            var past7Days = Enumerable.Range(0, 7)
                .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData =
                from day in past7Days
                join income in incomeSummary on day equals income.Day into dayIncomeJoined
                from income in dayIncomeJoined.DefaultIfEmpty()
                join expense in expenseSummary on day equals expense.Day into dayExpenseJoined
                from expense in dayExpenseJoined.DefaultIfEmpty()
                select new
                {
                    Day = day,
                    Income = income?.Income ?? 0,
                    Expense = expense?.Expense ?? 0,

                };



            // recent transactions

            ViewBag.RecentTransactions = await context.Transactions
                .Include(t => t.Category)
                .OrderByDescending(d => d.Date)
                .Take(5)
                .ToListAsync();


            return View();
        }
    }

    public class SplineChartData
    {
        public string Day;
        public int Income;
        public int Expense;
    }
}
