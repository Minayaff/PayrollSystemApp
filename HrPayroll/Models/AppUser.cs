using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
