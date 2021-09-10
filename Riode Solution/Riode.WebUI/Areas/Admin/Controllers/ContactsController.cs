using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly RiodeDBContext _context;
        public ContactsController(RiodeDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Contacts
        public async Task<IActionResult> Index(int typeId)
        {

            var query = _context.ContactPosts.Where(cp => cp.DeleteByUserId == null).AsQueryable();
            ViewBag.all = query.Count();
            ViewBag.ha = query.Where(cp => cp.AnswerBy != null).Count();
            ViewBag.na = query.Where(cp => cp.AnswerBy == null).Count();
            ViewBag.Marked = query.Where(cp => cp.Marked == true).Count();
            switch (typeId)
            {
                case 1:
                    query = query.Where(cp => cp.AnswerBy != null);
                    break;
                case 2:
                    query = query.Where(cp => cp.AnswerBy == null);
                    break;
                case 3:
                    query = query.Where(cp => cp.Marked == true);
                    break;
                default:
                    break;
            }
            return View(await query.ToListAsync());
        }

        // GET: Admin/Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.ContactPosts
                .FirstOrDefaultAsync(m => m.Id == id && m.DeleteByUserId == null && m.AnswerDate == null);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

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

            return RedirectToAction("index");
        }

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
