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
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Surname { get; set; }
        [Required, StringLength(50)]
        public string FatherName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }
        [Required, StringLength(60)]
        public string Adress { get; set; }
        [Required, StringLength(60)]
        public string RegisterAdress { get; set; }
        [Required]
        public string PassportId { get; set; }
        [Required]
        public string PassportExpDate { get; set; }
        [Required, StringLength(60)]
        public string Education { get; set; }
        public MartialStatus MartialStatus { get; set; }
        public Gender Gender { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

    

    }

}