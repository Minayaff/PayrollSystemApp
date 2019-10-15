using HrPayroll.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.ViewModel
{
    public class RegisterViewModelRole
    {
        [Required]
        public string Username { get; set; }
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required, Compare(nameof(Password)), DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        public Employee Employee { get; set; }

    }
}
