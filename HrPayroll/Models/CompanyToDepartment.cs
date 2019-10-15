using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class CompanyToDepartment
    {
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }



    }
}
