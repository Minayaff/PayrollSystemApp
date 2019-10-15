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

    public class CompanyToDepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public CompanyToDepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GetCompanyToDepartments.Include(c => c.Company).Include(c => c.Department);
            return View(await appDbContext.ToListAsync());
        }
      
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,DepartmentId")] CompanyToDepartment companyToDepartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyToDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", companyToDepartment.CompanyId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", companyToDepartment.DepartmentId);
            return View(companyToDepartment);
        }


        public IActionResult Delete(int? companyId, int? departId)
        {
            if (companyId == null && departId ==null)
            {
                return NotFound();
            }

            return View();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CompanyToDepartment cmb)
        {
            if (cmb == null)
            {
                return NotFound();
            }

            var b = await _context.GetCompanyToDepartments.Where(x => x.CompanyId == cmb.CompanyId && x.DepartmentId == cmb.DepartmentId).FirstOrDefaultAsync();
            _context.GetCompanyToDepartments.Remove(b);
            await _context.SaveChangesAsync();
           
            return RedirectToAction();
        }

      
       

        private bool CompanyToDepartmentExists(int id)
        {
            return _context.GetCompanyToDepartments.Any(e => e.CompanyId == id);
        }
    }
}
