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
    [Authorize(Roles = StaticData.DepartmentHeadRole)]

    public class SalariesController : Controller
    {
        private readonly AppDbContext _context;

        public SalariesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Salaries.Include(s => s.Company).Include(s => s.Position);
            return View(await appDbContext.ToListAsync());
        }

     
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Payment,PositionId,CompanyId")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", salary.CompanyId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", salary.PositionId);
            return View(salary);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", salary.CompanyId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", salary.PositionId);
            return View(salary);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Payment,PositionId,CompanyId")] Salary salary)
        {
            if (id != salary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", salary.CompanyId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", salary.PositionId);
            return View(salary);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Company)
                .Include(s => s.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.Id == id);
        }
    }
}
