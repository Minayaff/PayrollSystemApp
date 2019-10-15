using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Department
    {
        public Department()
        {
            Positions = new HashSet<Position>();
            HoldingToDepartments = new HashSet<HoldingToDepartment>();
            CompanyToDepartments = new HashSet<CompanyToDepartment>();

        }
        public int Id { get; set; }
        [Required,StringLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<HoldingToDepartment>  HoldingToDepartments { get; set; }
       public virtual ICollection<CompanyToDepartment> CompanyToDepartments { get; set; }
    }
}
