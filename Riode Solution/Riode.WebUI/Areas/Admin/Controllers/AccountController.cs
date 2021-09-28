using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.Entities.Membership;
using Riode.WebUI.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class AccountController : Controller
    {
        readonly UserManager<RiodeUser> userManager;
        readonly SignInManager<RiodeUser> signInManager;
        public AccountController(UserManager<RiodeUser> userManager, SignInManager<RiodeUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (ModelState.IsValid)
            {
                RiodeUser found = null;
                if (model.UserName.IsEmail())
                {
                    found = await userManager.FindByEmailAsync(model.UserName);
                }
                else
                {
                    found = await userManager.FindByNameAsync(model.UserName);
                }

                if (found == null)
                {
                    ViewBag.Message = "Username or Password is incorrect";
                    return View(model);
                }

                var signInResult = await signInManager.PasswordSignInAsync(found, model.Password,false,true);
                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "Username or Password is incorrect";
                    return View(model);
                }

                return RedirectToAction("index", "Dashboard");
            }

            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
