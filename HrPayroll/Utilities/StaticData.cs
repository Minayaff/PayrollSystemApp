using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Utilities
{
    public static class StaticData
    {
        public enum Roles
        {
            Admin,
            HR,
            PayrollSpecialist,
            DepartmentHead

        }
        public const string AdminRole = "Admin";
        public const string HRRole = "HR";
        public const string PayrollSpecialistRole = "PayrollSpecialist";
        public const string DepartmentHeadRole = "DepartmentHead";


    }


}

