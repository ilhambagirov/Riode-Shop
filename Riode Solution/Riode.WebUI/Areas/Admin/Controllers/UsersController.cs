using Microsoft.AspNetCore.Mvc;
using Riode.Application.Core.Infrastructure;
using Riode.Domain.Models.DataContext;
using System.Linq;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        readonly RiodeDBContext db;
        public UsersController(RiodeDBContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var users = db.Users
                .ToList();
            return View(users);
        }
        public IActionResult Details(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        public ActionResult<CommandJsonResponse> Ban(int id)
        {
            var response = new CommandJsonResponse();
            if (id <= 0)
            {
                return BadRequest();
            }
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                response.Error = true;
                response.Message = "User is not found";
                goto end;
            }
            if (user.Banned==false)
            {
                user.Banned = true;
                response.Error = false;
                response.Message = "User is unblocked";
            }
            else
            {
                user.Banned = false;
                response.Error = false;
                response.Message = "User is Banned";
            }
            db.SaveChangesAsync();
           

        end:
            return response;
        }
    }
}
