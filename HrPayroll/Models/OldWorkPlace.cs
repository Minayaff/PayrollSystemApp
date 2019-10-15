using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class OldWorkPlace
    {
        public int Id { get; set; }
        [Required, StringLength(60)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FireDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime HireDate { get; set; }
        [Required, StringLength(80)]
        public string FireReason { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }



    }
}
