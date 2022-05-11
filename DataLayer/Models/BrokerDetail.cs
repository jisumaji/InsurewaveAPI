using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class BrokerDetail
    {
        public BrokerDetail()
        {
            BrokerRequests = new HashSet<BrokerRequest>();
            PolicyDetails = new HashSet<PolicyDetail>();
        }

        public string BrokerId { get; set; } = null!;
        public int? CustomerCount { get; set; }
        public double? Commission { get; set; }

        public virtual UserDetail Broker { get; set; } = null!;
        public virtual ICollection<BrokerRequest> BrokerRequests { get; set; }
        public virtual ICollection<PolicyDetail> PolicyDetails { get; set; }
    }
}
