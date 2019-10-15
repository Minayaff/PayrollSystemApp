using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Company
    {
        public Company()
        {
            Branches = new HashSet<Branch>();
            Salaries = new HashSet<Salary>();
            CompanyToDepartments = new HashSet<CompanyToDepartment>();
        }
        public int Id { get; set; }
        [Required,StringLength(50)]
        public string Name { get; set; }
        public int HoldingId { get; set; }
        public virtual Holding Holding { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<CompanyToDepartment> CompanyToDepartments { get; set; }

    }
}
