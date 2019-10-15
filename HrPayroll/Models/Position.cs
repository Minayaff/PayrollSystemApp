using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Position
    {
        public Position()
        {
            Salaries = new HashSet<Salary>();
            WorkPlaces = new HashSet<WorkPlace>();

        }
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name{ get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }

    }
}
