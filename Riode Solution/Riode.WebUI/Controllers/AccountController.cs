using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

<<<<<<< Updated upstream
        public IActionResult PopupLogin()
=======
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

                if (!await userManager.IsInRoleAsync(found, "User"))
                {
                    ViewBag.Message = "Username or Password is incorrect";
                    return View(model);
                }
                var isconfirmed = await db.Users.FirstOrDefaultAsync(u => u.UserName.Equals(model.UserName) && u.EmailConfirmed != true);
                if (isconfirmed != null)
                {
                    ViewBag.Message = "Go confirm your email first!";
                    return View(model);
                }

                var signInResult = await signInManager.PasswordSignInAsync(found, model.Password, false, true);
                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "Username or Password is incorrect";
                    return View(model);
                }

                var redirectUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(redirectUrl))
                {
                    return Redirect(redirectUrl);
                }

                return RedirectToAction("index", "Home");
            }
            ViewBag.Message = "Fill the required fields!";
            return View(model);
        }

        public async Task<IActionResult>Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login") ;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new RiodeUser();

                user.UserName = model.UserName;
                user.Email = model.Email;

                if (model.Password != model.PasswordCheck)
                {
                    ViewBag.Message = "Passwords are different!";
                    return View(model);
                }
                string token = $"subscribetoken-{model.UserName}-{DateTime.Now:yyyyMMddHHmmss}";

                string chiperToken = token.Encrypt("Riode");

                string path = $"{Request.Scheme}://{Request.Host}/register-email-confirm?token={chiperToken}";

                var mailSent = configuration.SendEmail(model.Email, "Riode Newsletter Subscription", $"Please confirm your Email through this <a href={path}>link</a>");

                var iUserResult = userManager.CreateAsync(user, model.Password).Result;
                foreach (var item in iUserResult.Errors)
                {
                    if (item.Code.Contains("DuplicateUserName"))
                    {
                        ViewBag.Message = "Username is already taken!";
                        return View(model);
                    }
                    else if (item.Code.Contains("DuplicateEmail"))
                    {
                        ViewBag.Message = "Email is already taken!";
                        return View(model);
                    }
                }

                if (iUserResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }

                return RedirectToAction("EmailConfrimationNotice", "Account");
            }
            ViewBag.Message = "Fill the required fields!";
            return View(model);
        }


        [HttpGet]
        [Route("register-email-confirm")]
        public IActionResult RegisterConfirm(string token)
        {
            string plainTokken = token.Decrypt("Riode");
            Match match = Regex.Match(plainTokken, @"subscribetoken-(?<id>[a-zA-Z0-9]*)(.*)-(?<timeStampt>\d{14})");

            if (match.Success)
            {
                string id = match.Groups["id"].Value;
                string ts = match.Groups["timeStampt"].Value;
                var subscribe = db.Users.FirstOrDefault(s => s.UserName == id);

                if (subscribe == null)
                {
                    ViewBag.message = Tuple.Create(true, "Token Error");
                    goto end;
                }

                if (subscribe.EmailConfirmed == true)
                {
                    ViewBag.message = Tuple.Create(true, "Already Confirmed");
                    goto end;
                }

                subscribe.EmailConfirmed = true;
                db.SaveChanges();

                ViewBag.message = Tuple.Create(false, "Succesfully Confirmed");
            }
        end:
            return View();
        }

        /*public IActionResult PopupLogin()
        {
            return View();
        }*/
        [Authorize(Policy = "account.wishlist")]
        public IActionResult WishList()
        {
            return View();
        }

        public IActionResult EmailConfrimationNotice()
>>>>>>> Stashed changes
        {
            return View();
        }
    }
}
