using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrPayroll.DAL;
using HrPayroll.Models;
using HrPayroll.ViewModel;
using HrPayroll.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace HrPayroll.Controllers
{
    [Authorize(Roles = StaticData.HRRole)]

    public class WorkPlacesController : Controller
    {
        private readonly AppDbContext _context;

        public WorkPlacesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            //ViewBag.Position = _context.Positions.ToList();

            if (id == null)
            {
                return NotFound();
            }


            var work = await _context.Employees
                .Include(x=>x.WorkPlaces)
                .Include("WorkPlaces.Branch")
                .Include("WorkPlaces.Branch.Company")
                .Include("WorkPlaces.Position")
                .Include("WorkPlaces.Position.Department")
                .FirstAsync(v => v.Id == id);

            return View(work);
        }


        public async Task<IActionResult> Create(int? id)
        {
            //ViewBag.Position = _context.Positions.ToList();
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            ViewBag.Company = _context.Companies.ToList();

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
        public async Task<IActionResult> Create(WorkPlaceViewModel vms)
        {
            vms.Branch = await _context.Branches.Where(b => b.Id == vms.BranchId).FirstOrDefaultAsync();
            vms.Position = await _context.Positions.Where(p => p.Id == vms.PositionId).FirstOrDefaultAsync();

            WorkPlace workPlace = new WorkPlace()
            {
                EmployeeId = vms.EmployeeId,
                PositionId = vms.PositionId,
                BranchId = vms.BranchId,
                EntryDate = vms.EntryDate,
                
            };

            if (vms.BranchId != 0 && vms.PositionId != 0 && vms.EntryDate !=null && vms.CompanyToDepartment !=null)
            {
                _context.WorkPlaces.Add(workPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }

            return View(vms);
        }

       

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPlace = await _context.WorkPlaces
                .Include(w => w.Employee)
                .Include(w => w.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workPlace == null)
            {
                return NotFound();
            }


            return View(workPlace);
        }

        // POST: WorkPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workPlace = await _context.WorkPlaces.FindAsync(id);
            _context.WorkPlaces.Remove(workPlace);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index" ,"Employees");
        }

        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}
