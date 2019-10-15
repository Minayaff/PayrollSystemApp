using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class CompanyMonthGain
    {
        public int Id { get; set; }
        public Branch Branch { get; set; }
        public int BranchId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
