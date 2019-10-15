using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee  Employee { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Required]
        public decimal Bonus { get; set; }
        [Required]
        public decimal Penal { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public decimal TotalSalary { get; set; }
    }
}
