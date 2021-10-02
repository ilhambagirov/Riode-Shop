using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.AppCode.Application.MailBoxModule;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly RiodeDBContext _context;
        private readonly IConfiguration configuration;
        readonly IMediator mediator;
        public ContactsController(RiodeDBContext context, IConfiguration configuration,IMediator mediator)
        {
            _context = context;
            this.configuration = configuration;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.contacts.index")]
        public async Task<IActionResult> Index(MailBoxPagedQuery query)
        {

            var response = await mediator.Send(query);
            return View(response);
        }

        [Authorize(Policy = "admin.contacts.details")]
        public async Task<IActionResult> Details(MailBoxSingleQuery query)
        {
            var contact = await mediator.Send(query);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [Authorize(Policy = "admin.contacts.answer")]
        public async Task<IActionResult> Answer([FromRoute] int id, [Bind("Id,Answer")] Contact model)
        {
            if (model == null)
            {
                return NotFound();
            }

            if (id != model.Id)
            {
                return NotFound();
            }


            var contact = await _context.ContactPosts
                .FirstOrDefaultAsync(m => m.Id == id && m.DeleteByUserId == null && m.AnswerDate == null);
            if (contact == null)
            {
                return NotFound();
            }

            contact.Answer = model.Answer;
            contact.AnswerBy = 1;
            contact.AnswerDate = DateTime.Now;
            await _context.SaveChangesAsync();

            var content = "ContactAnswerTemplate.html".GetStaticFileContent();

            var mailSent = configuration.SendEmail(contact.Email, "Riode Answer", content.Replace("##answer##", contact.Answer));

            return RedirectToAction("index");
        }

        [Authorize(Policy = "admin.contacts.mark")]
        public async Task<IActionResult> Mark([FromRoute] int id, [Bind("Id,Marked")] Contact model)
        {
            if (model == null)
            {
                return NotFound();
            }

            if (id != model.Id)
            {
                return NotFound();
            }
            var contact = await _context.ContactPosts
                .FirstOrDefaultAsync(m => m.Id == id && m.DeleteByUserId == null);
            if (contact == null)
            {
                return NotFound();
            }
            if (contact.Marked == true)
            {
                contact.Marked = false;
            }
            else
            {
                contact.Marked = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
