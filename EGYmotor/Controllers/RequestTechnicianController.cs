using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using EGYmotor.Data;
using EGYmotor.Models;
using System.Linq;

namespace EGYmotor.Controllers
{
    public class RequestTechnicianController : Controller
    {
        private readonly EGYmotorContext db;

        public RequestTechnicianController(EGYmotorContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Request model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

                model.UserId = userId.Value;
                db.Requests.Add(model);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Successful Request";
                return RedirectToAction(nameof(MyRequest)); 
        }

        public IActionResult MyRequest()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var request = db.Requests
                            .Include(r => r.RegisterUser) 
                            .FirstOrDefault(r => r.UserId == userId.Value);

            if (request == null)
            {
                ViewBag.Message = "You have not made a request yet.";
                return View();
            }

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMyRequest(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var request = db.Requests.FirstOrDefault(r => r.RequestId == id && r.UserId == userId.Value);
            if (request == null)
            {
                return NotFound();
            }

            db.Requests.Remove(request);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Request deleted successfully.";
            return RedirectToAction(nameof(Create));
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
