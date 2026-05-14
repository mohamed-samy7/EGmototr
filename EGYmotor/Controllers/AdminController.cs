using EGYmotor.Data;
using EGYmotor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGYmotor.Controllers
{
    public class AdminController : Controller
    {
        private readonly EGYmotorContext _context;

        public AdminController(EGYmotorContext context)
        {
            _context = context;
        }

        private bool IsAdminLoggedIn() =>
            HttpContext.Session.GetInt32("ID") != null;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Admin admin, string secretCode)
        {
            if (secretCode != "@123")
            {
                ModelState.AddModelError("SecretCode", "Invalid registration code.");
                return View(admin);
            }

            if (ModelState.IsValid)
            {
                admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);
                admin.ConfirmPassword = admin.Password;
                _context.Admins.Add(admin);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(admin);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email);
            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                HttpContext.Session.SetInt32("ID", admin.ID);
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Dashboard()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var user = _context.Admins.FirstOrDefault(u => u.ID == HttpContext.Session.GetInt32("ID").Value);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        public IActionResult ViewAllUsers()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var users = _context.RegisterUser.ToList();
            return View(users);
        }

        public async Task<IActionResult> ViewAllRequest()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var requests = await _context.Requests
                .Include(f => f.RegisterUser)
                .ToListAsync();

            return View(requests);
        }

        public IActionResult ManageFeedbacks()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var feedbacks = _context.Feedback
                .Include(f => f.RegisterUser)
                .ToList();

            return View(feedbacks);
        }

        public async Task<IActionResult> AllPayments()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var payments = await _context.Payments
                .Include(p => p.RegisterUser)
                .ToListAsync();

            return View(payments);
        }

        public async Task<IActionResult> Profits()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var payments = await _context.Payments.ToListAsync();

            var totalRevenue = payments.Sum(p => p.Amount);
            var totalTransactions = payments.Count;
            var avgPerTransaction = totalTransactions > 0
                            ? Math.Round(totalRevenue / totalTransactions, 2)
                            : 0;

            var monthlyProfits = payments
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .Select(g => new MonthlyProfit
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(p => p.Amount),
                    Transactions = g.Count()
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalTransactions = totalTransactions;
            ViewBag.AvgPerTransaction = avgPerTransaction;

            return View(monthlyProfits);
        }
    }
}