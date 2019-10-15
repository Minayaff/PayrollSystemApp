using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Holding
    {
        public Holding()
        {
            Companies = new HashSet<Company>();
            HoldingToDepartments = new HashSet<HoldingToDepartment>();
        }
        public int Id { get; set; }
        [Required, StringLength(60)]
        public string Name{ get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<HoldingToDepartment>  HoldingToDepartments { get; set; }
    }
}
