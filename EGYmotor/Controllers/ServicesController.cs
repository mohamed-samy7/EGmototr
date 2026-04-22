using Microsoft.AspNetCore.Mvc;

namespace EGYmotor.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult services()
        {
            return View("services");
        }
    }
}
