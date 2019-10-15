using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.DAL;
using HrPayroll.Models;
using HrPayroll.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrPayroll.Controllers
{
    [Authorize(Roles = StaticData.PayrollSpecialistRole)]

    public class PayrollController : Controller
    {
        private readonly AppDbContext _context;
        public PayrollController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(e => e.AppUser).Include(e => e.Attendances).Include(e => e.Bonus)
           .Include(e => e.Penals).Include(e => e.WorkPlaces).Include("WorkPlaces.Position").Include("WorkPlaces.Position.Salaries").ToListAsync();

            foreach (var item in employees)
            {
                decimal baseamount = item.WorkPlaces.First().Position.Salaries.First().Payment;
                decimal salary = baseamount;
                decimal dailyamount = baseamount / 30;
                decimal bonus = 0;
                decimal penal = 0;
                foreach (var item1 in item.Bonus.Where(x => x.Date > DateTime.Now.AddDays(-30)))
                {
                    baseamount += item1.Amount;
                    bonus += item1.Amount;
                }
                foreach (var item1 in item.Penals.Where(x => x.Date > DateTime.Now.AddDays(-30)))
                {
                    baseamount -= item1.Amount;
                    penal += item1.Amount;
                }
                foreach (var item1 in item.Attendances.Where(x => x.Date > DateTime.Now.AddDays(-30) && x.Permission == Permission.Uzursuz))
                {
                    baseamount -= dailyamount;
                }

                WorkPlace workPlace = await _context.WorkPlaces.Where(w => w.EmployeeId == item.Id).FirstOrDefaultAsync();
                CompanyMonthGain gain = await _context.Gains.Where(g => g.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && g.BranchId == workPlace.BranchId).FirstOrDefaultAsync();

                Campaign campaign = await _context.Campaigns.Where(g => g.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && g.BranchId == workPlace.BranchId).FirstOrDefaultAsync();

                if(gain != null && campaign != null)
                {
                    if (campaign.FromAmount <= gain.Amount)
                    {
                        baseamount += campaign.Bonus;
                    }
                }

                item.finalSalary = (int)baseamount;
                Payroll payroll = new Payroll()
                {
                    Employee = item,
                    EmployeeId = item.Id,
                    Date = DateTime.Now,
                    Bonus = bonus,
                    Penal = penal,
                    Salary = salary,
                    TotalSalary = item.finalSalary
                };


                _context.Payrolls.Add(payroll);
                await _context.SaveChangesAsync();
            }

            ViewBag.Success = "true";
            return RedirectToAction("Index","Employees");
        }
    }
}