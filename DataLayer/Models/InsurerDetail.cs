using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class InsurerDetail
    {
        public InsurerDetail()
        {
            PolicyDetails = new HashSet<PolicyDetail>();
        }

        public string InsurerId { get; set; } = null!;
        public int? NoOfProducts { get; set; }
        public double? Commission { get; set; }

        public virtual UserDetail Insurer { get; set; } = null!;
        public virtual ICollection<PolicyDetail> PolicyDetails { get; set; }
    }
}
