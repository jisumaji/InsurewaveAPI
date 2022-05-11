using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class UserDetail
    {
        public UserDetail()
        {
            BuyerAssets = new HashSet<BuyerAsset>();
        }

        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public int? LicenseId { get; set; }

        public virtual BrokerDetail BrokerDetail { get; set; } = null!;
        public virtual InsurerDetail InsurerDetail { get; set; } = null!;
        public virtual ICollection<BuyerAsset> BuyerAssets { get; set; }
    }
}
