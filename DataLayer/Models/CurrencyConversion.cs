using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class CurrencyConversion
    {
        public CurrencyConversion()
        {
            BuyerAssets = new HashSet<BuyerAsset>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public double Rate { get; set; }

        public virtual ICollection<BuyerAsset> BuyerAssets { get; set; }
    }
}
