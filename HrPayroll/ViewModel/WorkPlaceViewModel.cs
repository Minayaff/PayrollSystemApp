using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.ViewModel
{
    public class WorkPlaceViewModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EntryDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        public CompanyToDepartment CompanyToDepartment { get; set; }

    }
}
