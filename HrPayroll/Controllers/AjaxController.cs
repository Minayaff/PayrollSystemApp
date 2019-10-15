using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrPayroll.Controllers
{
    public class AjaxController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AjaxController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpPost]
        public async Task<JsonResult> LoadBranchToCompanyId(int id)
        {
            //List<CompanyToDepartment> Departments = await _appDbContext.GetCompanyToDepartments.Include(c=>c.Department).Where(c => c.CompanyId == id).ToListAsync();
            var Departments = await _appDbContext.GetCompanyToDepartments.Where(c => c.CompanyId == id).Select(c => new { c.DepartmentId, c.Department.Name }).ToListAsync();

            List<Branch> Branches =await  _appDbContext.Branches.Where(b => b.CompanyId == id).ToListAsync();
            List<Position> Positions = await _appDbContext.Positions.Where(b => b.DepartmentId == id).ToListAsync();


            return Json(new { departments = Departments, branches = Branches, position =Positions });
        }
        public async Task<JsonResult> LoadBranchToDepartmentId(int id)
        {
          
            List<Position> Positions = await _appDbContext.Positions.Where(b => b.DepartmentId == id).ToListAsync();


            return Json(new { position = Positions });
        }
    }
}
