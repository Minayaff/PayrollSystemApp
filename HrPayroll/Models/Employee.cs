using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Employee
    {
        public Employee()
        {
            WorkPlaces = new HashSet<WorkPlace>();
            OldWorkPlaces = new HashSet<OldWorkPlace>();
            Vocations = new HashSet<Vocation>();
            Attendances = new HashSet<Attendance>();
            Bonus = new HashSet<Bonus>();
            Penals = new HashSet<Penal>();
            Payrolls = new HashSet<Payroll>();
        }
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Surname { get; set; }
        [Required, StringLength(50)]
        public string FatherName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
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
        [StringLength(255)]
        [DataType(DataType.Upload)]
        public string PhotoPath { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }
        public virtual ICollection<OldWorkPlace> OldWorkPlaces { get; set; }
        public virtual ICollection<Vocation> Vocations { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Bonus> Bonus { get; set; }
        public virtual ICollection<Penal> Penals { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }

        [NotMapped]
        public decimal finalSalary { get; set; }
    }
}


