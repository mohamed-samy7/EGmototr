using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EGYmotor.Data;
using EGYmotor.Models;
using System.Threading.Tasks;

namespace EGYmotor.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly EGYmotorContext _context;

        public FeedbackController(EGYmotorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllFeedbacks()
        {
            var feedbacks = await _context.Feedback
                .Include(f => f.RegisterUser)
                .ToListAsync();

            return View(feedbacks);
        }

        public IActionResult Create()
        {
            return View(new Feedback());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]



        public async Task<IActionResult> Create(Feedback feedback)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            feedback.UserId = userId.Value;

            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AllFeedbacks));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMyFeedback(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var feedback = await _context.Feedback
                .FirstOrDefaultAsync(f => f.FeedbackID == id && f.UserId == userId.Value);

            if (feedback == null)
            {
                return NotFound(); 
            }

            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AllFeedbacks));
        }
        [HttpPost]
        public IActionResult DeleteByAdmin(int id)
        {
            var feedback = _context.Feedback.FirstOrDefault(f => f.FeedbackID == id);
            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
                _context.SaveChanges();
            }

            return RedirectToAction("ManageFeedbacks", "Admin");
        }


    }
}
