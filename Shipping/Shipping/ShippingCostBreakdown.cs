using System.Linq;

namespace Shipping
{
    public class ShippingCostBreakdown
    {
        public ParcelSize[] Parcels { get; internal init; }
        
        public decimal SpeedyShippingCostInDollars { get; internal set; }

        public decimal TotalCostInDollars =>
            Parcels.Sum(p => p.ShippingCostInDollars) +
            SpeedyShippingCostInDollars;
    }
}