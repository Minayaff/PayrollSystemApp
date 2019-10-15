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
    public class AttendancesController : Controller
    {
        private readonly AppDbContext _context;

        public AttendancesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atten = await _context.Employees
                .Include(x => x.Attendances)
                .FirstAsync(v => v.Id == id);

            return View(atten);

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
        public async Task<IActionResult> Create([Bind("Date,Permission,Reason,EmployeeId")] Attendance attendance)
        {
            if ((attendance.Date != null && attendance.EmployeeId != 0 && attendance.Reason != null))
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            return View(attendance);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            ViewBag.EmployeeId = attendance.EmployeeId;
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Permission,Reason,EmployeeId")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            return View(attendance);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Employees");

        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
