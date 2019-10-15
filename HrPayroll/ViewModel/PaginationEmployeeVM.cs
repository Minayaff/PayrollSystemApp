using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.ViewModel
{
    public class PaginationEmployeeVM
    {
        public PaginationModel pagination { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Employee> EmployeesSalaryMonth { get; set; }
        public Employee Employee { get; set; }
    }
}
