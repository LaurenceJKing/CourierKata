using System.Linq;

namespace Shipping
{
    public class ShippingCostBreakdown
    {
        public ShippingCostItem[] Parcels { get; internal init; } = Array.Empty<ShippingCostItem>();
        
        public decimal SpeedyShippingCostInDollars { get; internal set; }

        public decimal TotalCostInDollars =>
            Parcels.Sum(p => p.ShippingCostInDollars) +
            SpeedyShippingCostInDollars;
    }

    public class ShippingCostItem
    {
        public string Description { get; init; } = string.Empty;
        public decimal ShippingCostInDollars { get; internal init; }
    }
}