using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.ViewModel
{
    public class PaginationModel
    {
        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}
