using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Infrastructure.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "identity-user")]
    public class UserRolesTagHelper : TagHelper
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppDbContext _dbContext;
        public UserRolesTagHelper(UserManager<AppUser> usermgr, RoleManager<IdentityRole> rolemgr, AppDbContext dbContext)
        {
            _userManager = usermgr;
            _roleManager = rolemgr;
            _dbContext = dbContext;
        }
        [HtmlAttributeName("identity-user")]
        public string User { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            AppUser user = await _userManager.FindByIdAsync(User);
            var ur = await _dbContext.UserRoles.Where(x => x.UserId == User).ToListAsync();
            if (user != null)
            {

                foreach (var userRole in ur)
                {
                    foreach (var role in _roleManager.Roles.OrderByDescending(r => r.Name))
                    {
                        if (role != null && role.Id == userRole.RoleId)
                        {
                            names.Add(role.Name);
                        }
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Role" : string.Join(", ", names));
        }
    }
}
