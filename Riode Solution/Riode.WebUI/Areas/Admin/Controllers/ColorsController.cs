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
    public class ColorsController : Controller
    {
        private readonly RiodeDBContext _context;

        public ColorsController(RiodeDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Colors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colors.ToListAsync());
        }

        // GET: Admin/Colors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // GET: Admin/Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,HexCode,Description,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Colors colors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colors);
        }

        // GET: Admin/Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }
            return View(colors);
        }

        // POST: Admin/Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Colors colors)
        {
            if (id != colors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorsExists(colors.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(colors);
        }

        // GET: Admin/Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // POST: Admin/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colors = await _context.Colors.FindAsync(id);
            _context.Colors.Remove(colors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorsExists(int id)
        {
            return _context.Colors.Any(e => e.Id == id);
        }
    }
}
