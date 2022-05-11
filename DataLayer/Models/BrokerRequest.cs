using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class BrokerRequest
    {
        public int RequestId { get; set; }
        public int AssetId { get; set; }
        public string BrokerId { get; set; } = null!;
        public string? ReviewStatus { get; set; }

        public virtual BuyerAsset Asset { get; set; } = null!;
        public virtual BrokerDetail Broker { get; set; } = null!;
    }
}
