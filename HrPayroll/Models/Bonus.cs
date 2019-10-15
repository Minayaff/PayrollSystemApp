using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Bonus
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, MaxLength(200)]
        public string Reason { get; set; }
        public bool IsPayed { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
