using System.Linq;

namespace Shipping
{
    public class ShippingCostBreakdown
    {
        public ShippingCostItem[] Parcels { get; internal init; }
        
        public decimal SpeedyShippingCostInDollars { get; internal set; }

        public decimal TotalCostInDollars =>
            Parcels.Sum(p => p.ShippingCostInDollars) +
            SpeedyShippingCostInDollars;
    }

    public class ShippingCostItem
    {
        internal ParcelSize ParcelSize { get; init; }
        public string Description => ParcelSize.Description;
        public decimal ShippingCostInDollars { get; internal init; }
    }
}