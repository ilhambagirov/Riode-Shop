using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult MailBox()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult InVoice()
        {
            return View();
        }

        public IActionResult Table()
        {
            return View();
        }

        public IActionResult Forms()
        {
            return View();
        }
    }
}
        

