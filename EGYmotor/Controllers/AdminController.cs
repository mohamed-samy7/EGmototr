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
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (admin != null)
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
            int? userId = HttpContext.Session.GetInt32("ID");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Admins.FirstOrDefault(u => u.ID == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }
        public IActionResult ViewAllUsers()
        {
            var users = _context.RegisterUser.ToList();
            return View(users);
        }
        public async Task<IActionResult> ViewAllRequest()
        {
            var feedbacks = await _context.Requests
                .Include(f => f.RegisterUser)
                .ToListAsync();

            return View(feedbacks);
        }
        public IActionResult ManageFeedbacks()
        {
            var feedbacks = _context.Feedback
                .Include(f => f.RegisterUser)
                .ToList();

            return View(feedbacks);
        }

        public async Task<IActionResult> AllPayments()
        {
            var payments = await _context.Payments
                .Include(p => p.RegisterUser)
                .ToListAsync();

            return View(payments);
        }



    }

}
