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
        public async Task<IActionResult> Create(Request model, IFormFile? ImageFile)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                model.ImagePath = "/uploads/" + fileName;
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

            var requests = db.Requests
                            .Include(r => r.RegisterUser)
                            .Where(r => r.UserId == userId.Value)
                            .ToList();

            if (!requests.Any())
            {
                ViewBag.Message = "You have not made a request yet.";
                return View();
            }

            return View(requests);
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