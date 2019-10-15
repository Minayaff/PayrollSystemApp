using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Salary
    {

        public int Id { get; set; }
        [Required]
        public int Payment { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
