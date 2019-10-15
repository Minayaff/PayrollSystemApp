using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.DAL;
using HrPayroll.Extensions;
using HrPayroll.Models;
using HrPayroll.Utilities;
using HrPayroll.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrPayroll.Controllers
{

    [Authorize(Roles = StaticData.AdminRole)]
    public class AdminController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHostingEnvironment _myroot;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(AppDbContext context, UserManager<AppUser> userManager, IHostingEnvironment myroot,
           SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = context;
            _userManager = userManager;
            _myroot = myroot;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
      

        public IActionResult AddRole()
        {
            ViewBag.Role = _appDbContext.Roles.ToList();


            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RegisterViewModelRole registerView, int? id)
        {
            ViewBag.Role = _appDbContext.Roles.ToList();

            if (id == null)
            {
                return NotFound();
            }

            var employee = _appDbContext.Employees.Include(x => x.AppUser).FirstOrDefault(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(registerView);
            }
            if (employee.AppUser == null)
            {

                AppUser newUser = new AppUser
                {
                    UserName = registerView.Username,
                    Email = registerView.Email,
                    PhoneNumber = registerView.PhoneNumber,
                    EmployeeId = id.Value
                };


                IdentityResult identity = await _userManager.CreateAsync(newUser, registerView.Password);
                if (!identity.Succeeded)
                {
                    foreach (var error in identity.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(registerView);
                }

                await _userManager.AddToRoleAsync(newUser, registerView.Role);

                employee.AppUserId = newUser.Id;
                await _appDbContext.SaveChangesAsync();

                await _signInManager.SignInAsync(newUser, true);

                return RedirectToAction("Index", "Employees");
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> EditRole(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var user = _appDbContext.Users.Include(x => x.Employee).FirstOrDefault(m => m.EmployeeId == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Role = _appDbContext.Roles.ToList();

            return View(new RegisterViewModelRole
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                Employee = user.Employee

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, RegisterViewModelRole test)
        {
            ViewBag.Role = _appDbContext.Roles.ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _appDbContext.Employees.Include(e => e.AppUser).FirstOrDefaultAsync(e => e.Id == id);
                    AppUser appUser = employee.AppUser;
                    appUser.Email = test.Email;
                    appUser.UserName = test.Username;
                    appUser.PhoneNumber = test.PhoneNumber;

                    string role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();

                    await _userManager.RemoveFromRoleAsync(appUser, role);
                    await _userManager.AddToRoleAsync(appUser, test.Role);

                    string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                    await _userManager.ResetPasswordAsync(appUser, token, test.Password);

                    await _userManager.UpdateAsync(appUser);
                    _appDbContext.Update(employee);
                    await _appDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction("Index", "Employees");

                }
            }
            return RedirectToAction("Index", "Employees");
        }
    }
}