using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Authorization;
using HrPayroll.Utilities;

namespace HrPayroll.Controllers
{
    [Authorize(Roles = StaticData.AdminRole)]
    public class HoldingsController : Controller
    {
        private readonly AppDbContext _context;

        public HoldingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Holdings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holding = await _context.Holdings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holding == null)
            {
                return NotFound();
            }

            return View(holding);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holding);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holding = await _context.Holdings.FindAsync(id);
            if (holding == null)
            {
                return NotFound();
            }
            return View(holding);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Holding holding)
        {
            if (id != holding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoldingExists(holding.Id))
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
            return View(holding);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holding = await _context.Holdings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holding == null)
            {
                return NotFound();
            }

            return View(holding);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holding = await _context.Holdings.FindAsync(id);
            _context.Holdings.Remove(holding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoldingExists(int id)
        {
            return _context.Holdings.Any(e => e.Id == id);
        }
    }
}
