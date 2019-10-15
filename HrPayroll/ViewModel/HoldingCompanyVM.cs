using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.ViewModel
{
    public class HoldingCompanyVM
    {
        public Holding Holding  { get; set; }
        public Company Company { get; set; }
    }
}
