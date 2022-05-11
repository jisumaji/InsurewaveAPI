using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class BuyerAsset
    {
        public BuyerAsset()
        {
            BrokerRequests = new HashSet<BrokerRequest>();
            PolicyDetails = new HashSet<PolicyDetail>();
        }

        public int AssetId { get; set; }
        public string UserId { get; set; } = null!;
        public int? CountryId { get; set; }
        public string AssetName { get; set; } = null!;
        public decimal PriceUsd { get; set; }
        public string Type { get; set; } = null!;
        public string? Request { get; set; }

        public virtual CurrencyConversion? Country { get; set; }
        public virtual UserDetail User { get; set; } = null!;
        public virtual ICollection<BrokerRequest> BrokerRequests { get; set; }
        public virtual ICollection<PolicyDetail> PolicyDetails { get; set; }
    }
}
