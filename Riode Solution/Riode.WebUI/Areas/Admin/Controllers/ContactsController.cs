using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Riode.Application.MailBoxModule;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
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
        public ContactsController(RiodeDBContext context, IConfiguration configuration, IMediator mediator)
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
        [HttpPost]
        public async Task<IActionResult> Answer([FromRoute] int id, MailBoxAnswerCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response != null)
            {
                return RedirectToAction("index");
            }

            return View(command);
        }

        [Authorize(Policy = "admin.contacts.mark")]
        public async Task<IActionResult> Mark([FromRoute] int id, MailBoxMarkCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            await mediator.Send(command);

            return RedirectToAction("index");








        }
    }
}
