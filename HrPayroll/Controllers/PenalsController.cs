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

    public class PenalsController : Controller
    {
        private readonly AppDbContext _context;
        private static int? ID;

        public PenalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Penals
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penal = await _context.Employees
                .Include(x => x.Penals)
                .FirstAsync(v => v.Id == id);

            ID = id;

            return View(penal);
        }



        public async Task<IActionResult> PartiallPenal(int? months)
        {
            var p = await _context.Employees
              .Include(x => x.Penals)
              .FirstAsync(v => v.Id == ID);
            var Penal = p.Penals.Where(v => v.Date.Month == months).ToList();
            return PartialView("Penal_PartialView", Penal);

        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            ViewBag.EmployeeId = id;
            ViewBag.EmployeeName = employee.Name + " " + employee.Surname;
            if (employee == null)
            {
                return NotFound();
            }
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,Date,Reason,IsPayed,EmployeeId")] Penal penal)
        {
            if (penal.Amount != 0 && penal.EmployeeId != 0 && penal.Date != null && penal.Reason != null)
            {
                _context.Add(penal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            return View(penal);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penal = await _context.Penals.FindAsync(id);
            ViewBag.EmployeeId = penal.EmployeeId;

            if (penal == null)
            {
                return NotFound();
            }
            return View(penal);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Date,Reason,IsPayed,EmployeeId")] Penal penal)
        {
            if (id != penal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenalExists(penal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Employees");
            }
            return View(penal);
        }

        // GET: Penals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penal = await _context.Penals
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penal == null)
            {
                return NotFound();
            }

            return View(penal);
        }

        // POST: Penals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var penal = await _context.Penals.FindAsync(id);
            _context.Penals.Remove(penal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Employees");
        }

        private bool PenalExists(int id)
        {
            return _context.Penals.Any(e => e.Id == id);
        }
    }
}
