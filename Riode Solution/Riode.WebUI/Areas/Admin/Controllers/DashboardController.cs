using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        [Authorize(Policy = "admin.dashboard.index")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Policy = "admin.dashboard.mailbox")]
        public IActionResult MailBox()
        {
            return View();
        }
        [Authorize(Policy = "admin.dashboard.chat")]
        public IActionResult Chat()
        {
            return View();
        }
        [Authorize(Policy = "admin.dashboard.invoice")]
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
        

