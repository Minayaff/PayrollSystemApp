using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.DAL;
using HrPayroll.Extensions;
using HrPayroll.Models;
using HrPayroll.Utilities;
using HrPayroll.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static HrPayroll.Utilities.StaticData;

namespace HrPayroll.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _myroot;

        public AccountController(AppDbContext dbContext,
                                 UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IHostingEnvironment myroot)
        {
            _db = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _myroot = myroot;

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            AppUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email or Password is invalid");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult =
                  await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, true);
            if (!signInResult.Succeeded)
            {

                ModelState.AddModelError("Email", "Email or password is invalid");
                return View(loginViewModel);
            }
            return RedirectToAction("Index", "Home");
        }


        //Create Roles
        public async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Roles.PayrollSpecialist.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.PayrollSpecialist.ToString()));
            }

            if (!await _roleManager.RoleExistsAsync(Roles.DepartmentHead.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.DepartmentHead.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Roles.HR.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.HR.ToString()));
            }

            //if (!await _roleManager.RoleExistsAsync(Roles.Employee.ToString()))
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
            //}

        }

        //Seed Default Admin
        public async Task SeedAdmin()
        {
            if (_userManager.FindByEmailAsync("admin@code.edu.az").Result == null)
            {
                AppUser admin = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@code.edu.az",
                    EmployeeId = 1,
                };
                IdentityResult result = await _userManager.CreateAsync(admin, "admin123A@");
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(admin, StaticData.AdminRole).Wait();
                    await _db.SaveChangesAsync();

                    await _signInManager.SignInAsync(admin, true);

                }


            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}