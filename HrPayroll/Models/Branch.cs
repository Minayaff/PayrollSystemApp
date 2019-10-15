using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class Branch
    {
        public Branch()
        {
            WorkPlaces = new HashSet<WorkPlace>();
            Campaigns = new HashSet<Campaign>();

        }
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public virtual Company  Company{ get; set; }
        public bool  IsMain { get; set; }
        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }
        public virtual ICollection<Campaign>  Campaigns { get; set; }

    }

}
