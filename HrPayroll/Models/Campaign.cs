using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch  Branch { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime  Date { get; set; }
        [Required]
        public decimal FromAmount { get; set; }
        [Required]
        public decimal ToAmount { get; set; }
        [Required]
        public decimal Bonus { get; set; }
    }
}
