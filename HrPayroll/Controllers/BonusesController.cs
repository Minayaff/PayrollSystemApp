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

    public class BonusesController : Controller
    {
        private readonly AppDbContext _context;
        private static int? ID;
        public BonusesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Employees
                .Include(x => x.Bonus)
                .FirstAsync(v => v.Id == id);

            ID = id;

            //var bonus = _context.Bonus.Include(m => m.Employee).Where(m => m.EmployeeId == id && m.Date.Month == DateTime.Now.Month).ToList();
            //@Model.Sum(m => m.Amount)

            return View(bonus);
        }

        public async Task<IActionResult> PatrialIndex(int? months)
        {

            var bonus = await _context.Employees
              .Include(x => x.Bonus)
              .FirstAsync(v => v.Id == ID);
            var Bonus = bonus.Bonus.Where(v => v.Date.Month == months).ToList();
            return PartialView("Partial_Index", Bonus);

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
        public async Task<IActionResult> Create([Bind
            (include: "Amount,Reason,IsPayed,Date,EmployeeId")]Bonus bonus)
        {
            if ((bonus.Amount != 0 && bonus.EmployeeId != 0 && bonus.Date != null && bonus.Reason != null))
            {
                _context.Add(bonus);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }

            return View(bonus);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Bonus.FindAsync(id);
            ViewBag.EmployeeId = bonus.EmployeeId;

            if (bonus == null)
            {
                return NotFound();
            }
            return View(bonus);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Reason,IsPayed,Date,EmployeeId")] Bonus bonus)
        {
            if (id != bonus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bonus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BonusExists(bonus.Id))
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
            return View(bonus);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Bonus
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bonus == null)
            {
                return NotFound();
            }

            return View(bonus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bonus = await _context.Bonus.FindAsync(id);
            _context.Bonus.Remove(bonus);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Employees");
        }

        private bool BonusExists(int id)
        {
            return _context.Bonus.Any(e => e.Id == id);
        }
    }
}
