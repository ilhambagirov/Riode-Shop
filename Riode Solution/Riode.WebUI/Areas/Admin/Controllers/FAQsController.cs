using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.Application.FaqModule;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FAQsController : Controller
    {
        private readonly RiodeDBContext _context;
        readonly IMediator mediator;

        public FAQsController(RiodeDBContext context, IMediator mediator)
        {
            _context = context;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.faq.index")]
        public async Task<IActionResult> Index(FaqPagedQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }

        [Authorize(Policy = "admin.faq.details")]
        public async Task<IActionResult> Details(FaqSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.faq.create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "admin.faq.create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FaqCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [Authorize(Policy = "admin.faq.edit")]
        public async Task<IActionResult> Edit(FaqSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            var vm =new FaqViewModel();
            vm.Question = response.Question;
            vm.Answer = response.Answer;

            return View(vm);
        }

        [Authorize(Policy = "admin.faq.edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, FaqEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        [HttpPost]
        [Authorize(Policy = "admin.faq.delete")]
        public async Task<IActionResult> Delete(FaqDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }


    }
}
