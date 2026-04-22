using Microsoft.AspNetCore.Mvc;
using EGYmotor.Models;
using EGYmotor.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EGYmotor.Controllers
{
    public class PaymentController : Controller
    {
        private readonly EGYmotorContext _context;

        public PaymentController(EGYmotorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Payment model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Login");
                model.UserId = userId.Value;
                _context.Payments.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Payment Successful!";
                return RedirectToAction("Create");
          
        }

        public IActionResult MyPayments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Login");

            var payments = _context.Payments
                .Where(p => p.UserId == userId)
                .ToList();

            return View(payments);
        }
    }
}
