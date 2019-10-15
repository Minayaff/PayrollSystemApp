using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Hosting;
using static HrPayroll.Extensions.IFormFileExtension;
using HrPayroll.ViewModel;
using Microsoft.AspNetCore.Authorization;
using HrPayroll.Utilities;
using Microsoft.AspNetCore.Identity;

namespace HrPayroll.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IHostingEnvironment _myroot;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EmployeesController(AppDbContext context, IHostingEnvironment myroot, UserManager<AppUser> userManager)
        {
            _context = context;
            _myroot = myroot;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            var employees = await _context.Employees.Include(e => e.AppUser).Include(e => e.Attendances).Include(e => e.Bonus)
           .Include(e => e.Penals).Include(e => e.WorkPlaces).Include("WorkPlaces.Position").Include("WorkPlaces.Position.Salaries").ToListAsync();

            foreach (var item in employees)
            {
                decimal baseamount = item.WorkPlaces.First().Position.Salaries.First().Payment;
                decimal dailyamount = baseamount/30;
                foreach (var item1 in item.Bonus.Where(x=>x.Date>DateTime.Now.AddDays(-30)))
                {
                    baseamount += item1.Amount;
                }
                foreach (var item1 in item.Penals.Where(x => x.Date > DateTime.Now.AddDays(-30)))
                {
                    baseamount -= item1.Amount;
                }
                foreach (var item1 in item.Attendances.Where(x => x.Date > DateTime.Now.AddDays(-30)&& x.Permission == Permission.Uzursuz))
                {
                    baseamount -= dailyamount;
                }
               
                item.finalSalary = (int)baseamount;
            }
            int take = 5;

            PaginationEmployeeVM data = new PaginationEmployeeVM
            {


                Employees = await _context.Employees.OrderBy(e => e.Id).Skip((page - 1) * take).Take(take).ToListAsync(),

                //Employee = await _context.Employees.OrderBy(e => e.Name).Skip((page - 1) * take).Take(take).FirstOrDefault(),

                pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Employees.Count()
                },

                EmployeesSalaryMonth = employees
            };

            return View(data);
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            Employee dbemployee = new Employee();

            if (employee.Photo != null && employee.Photo.IsImage())
            {
                dbemployee.PhotoPath = await employee.Photo.SaveImage(_myroot.WebRootPath.ToString());
                dbemployee.Name = employee.Name;
                dbemployee.Surname = employee.Surname;
                dbemployee.FatherName = employee.FatherName;
                dbemployee.Birthdate = employee.Birthdate;
                dbemployee.Adress = employee.Adress;
                dbemployee.RegisterAdress = employee.RegisterAdress;
                dbemployee.PassportId = employee.PassportId;
                dbemployee.PassportExpDate = employee.PassportExpDate;
                dbemployee.Education = employee.Education;
                dbemployee.MartialStatus = employee.MartialStatus;
                dbemployee.Gender = employee.Gender;

            }
            else
            {
                dbemployee.PhotoPath = "no-image.jpg";
                dbemployee.Name = employee.Name;
                dbemployee.Surname = employee.Surname;
                dbemployee.FatherName = employee.FatherName;
                dbemployee.Birthdate = employee.Birthdate;
                dbemployee.Adress = employee.Adress;
                dbemployee.RegisterAdress = employee.RegisterAdress;
                dbemployee.PassportId = employee.PassportId;
                dbemployee.PassportExpDate = employee.PassportExpDate;
                dbemployee.Education = employee.Education;
                dbemployee.MartialStatus = employee.MartialStatus;
                dbemployee.Gender = employee.Gender;

            }

            await _context.AddAsync(dbemployee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
        [Bind("Id,Name,Surname,FatherName,Birthdate,Adress,RegisterAdress,PassportId," +
            "PassportExpDate,Education,MartialStatus,Gender,PhotoPath")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
