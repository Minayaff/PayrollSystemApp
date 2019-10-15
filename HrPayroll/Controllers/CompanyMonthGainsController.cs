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
    [Authorize(Roles = StaticData.PayrollSpecialistRole)]

    public class CompanyMonthGainsController : Controller
    {
        private readonly AppDbContext _context;

        public CompanyMonthGainsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Gains.Include(c => c.Branch);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMonthGain = await _context.Gains
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyMonthGain == null)
            {
                return NotFound();
            }

            return View(companyMonthGain);
        }

        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BranchId,Date,Amount")] CompanyMonthGain companyMonthGain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyMonthGain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", companyMonthGain.BranchId);
            return View(companyMonthGain);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMonthGain = await _context.Gains.FindAsync(id);
            if (companyMonthGain == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", companyMonthGain.BranchId);
            return View(companyMonthGain);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BranchId,Date,Amount")] CompanyMonthGain companyMonthGain)
        {
            if (id != companyMonthGain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyMonthGain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyMonthGainExists(companyMonthGain.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", companyMonthGain.BranchId);
            return View(companyMonthGain);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMonthGain = await _context.Gains
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyMonthGain == null)
            {
                return NotFound();
            }

            return View(companyMonthGain);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyMonthGain = await _context.Gains.FindAsync(id);
            _context.Gains.Remove(companyMonthGain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyMonthGainExists(int id)
        {
            return _context.Gains.Any(e => e.Id == id);
        }
    }
}
