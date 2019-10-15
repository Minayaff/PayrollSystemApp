using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class HoldingToDepartment
    {
        public int HoldingId{ get; set; }
        public virtual Holding Holding { get; set; }
        public int DepartmentId{ get; set; }
        public virtual Department  Department{ get; set; }
    }
}
