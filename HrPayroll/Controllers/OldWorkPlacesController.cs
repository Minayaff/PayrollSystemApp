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
    [Authorize(Roles = StaticData.HRRole)]

    public class OldWorkPlacesController : Controller
    {
        private readonly AppDbContext _context;

        public OldWorkPlacesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.
                Include(x => x.OldWorkPlaces)
                .FirstAsync(v => v.Id == id); ;


            return View(employee);
           
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            ViewBag.EmployeeId = id;
            ViewBag.EmployeeName = employee.Name+" "+employee.Surname;
            if (employee == null)
            {
                return NotFound();
            }

            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind
            (include: "Name, FireDate, HireDate, FireReason, EmployeeId")]OldWorkPlace oldWorkPlace)
        {           
            if (ModelState.IsValid)
            {
                _context.Add(oldWorkPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index" , "Employees");
            }
         
            return View(oldWorkPlace);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldWorkPlace = await _context.oldWorkPlaces.FindAsync(id);
            ViewBag.EmployeId = oldWorkPlace.EmployeeId;


            if (oldWorkPlace == null)
            {
                return NotFound();
            }
            return View(oldWorkPlace);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FireDate,HireDate,FireReason,EmployeeId")] OldWorkPlace oldWorkPlace)
        {
            if (id != oldWorkPlace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oldWorkPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OldWorkPlaceExists(oldWorkPlace.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index" , "Employees");
            }
            return View(oldWorkPlace);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldWorkPlace = await _context.oldWorkPlaces
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oldWorkPlace == null)
            {
                return NotFound();
            }

            return View(oldWorkPlace);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oldWorkPlace = await _context.oldWorkPlaces.FindAsync(id);
            _context.oldWorkPlaces.Remove(oldWorkPlace);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Employees");
        }

        private bool OldWorkPlaceExists(int id)
        {
            return _context.oldWorkPlaces.Any(e => e.Id == id);
        }
    }
}
