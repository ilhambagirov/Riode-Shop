using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Riode.WebUI.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly RiodeDBContext db;
        readonly IConfiguration configuration;
        public HomeController(RiodeDBContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            var faqs = db.FAQs
                .Where(f => f.DeleteByUserId == null).ToList();
            return View(faqs);
        }

        public IActionResult notFoundPage()
        {
            return View();
        }

        public IActionResult Elements()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe([Bind("Email")] Subscribe model)
        {

            if (ModelState.IsValid)
            {
                var currentmodel = db.Subscribes.FirstOrDefault(m => m.Email.Equals(model.Email));
                if (currentmodel != null && currentmodel.EmailConfirmed == true)
                {
                    return Json(new
                    {
                        error = true,
                        message = "This Email is already in Our Database"
                    });
                }
                else if (currentmodel != null && (currentmodel.EmailConfirmed ?? false == false))
                {
                    return Json(new
                    {
                        error = true,
                        message = "Please Confirm Your Subscription"
                    });
                }


                db.Subscribes.Add(model);
                db.SaveChanges();

                string token = $"subscribetoken-{model.Id}-{DateTime.Now:yyyyMMddHHmmss}";

                string chiperToken = token.Encrypt(configuration["Encryption-Key:Riode"]);

                string path = $"{Request.Scheme}://{Request.Host}/subscribe-confirm?token={chiperToken}";

                var mailSent = configuration.SendEmail(model.Email, "Riode Newsletter Subscription", $"Please confirm your Email through this <a href={path}>link</a>");

                if (mailSent == false)
                {
                    db.Database.RollbackTransaction();
                    return Json(new
                    {
                        error = false,
                        message = "Try Again"
                    });
                }
                return Json(new
                {
                    error = false,
                    message = "Check Your Email and Confirm Subscription"
                });
            }
            return Json(new
            {
                error = true,
                message = "Error! Try Again Later"
            });
        }

        [HttpGet]
        [Route("subscribe-confirm")]
        public IActionResult SubscribeConfirm(string token)
        {
            string plainTokken = token.Decrypt(configuration["Encryption-Key:Riode"]);
            Match match = Regex.Match(plainTokken, @"subscribetoken-(?<id>\d+)-(?<timeStampt>\d{14})");

            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups["id"].Value);
                string ts = match.Groups["timeStampt"].Value;
                var subscribe = db.Subscribes.FirstOrDefault(s => s.Id == id && s.DeleteByUserId == null);

                if (subscribe == null)
                {
                    ViewBag.message = Tuple.Create(true, "Token Error");
                    goto end;
                }

                if ((subscribe.EmailConfirmed ?? false) == true)
                {
                    ViewBag.message = Tuple.Create(true, "Already Confirmed");
                    goto end;
                }

                subscribe.EmailConfirmed = true;
                subscribe.EmailConfirmedDate = DateTime.Now;
                db.SaveChanges();

                ViewBag.message = Tuple.Create(false, "Succesfully Confirmed");
            }
        end:
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.ContactPosts.Add(contact);
                db.SaveChanges();

                ViewBag.Message = "Successfully";
                ModelState.Clear();
                return Json(new
                {
                    error = false,
                    message = "Successfully"
                });
            }
            return Json(new
            {
                error = true,
                message = "Try Again"
            });
        }
    }
}
