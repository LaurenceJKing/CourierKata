namespace Shipping
{
    public class ShippingCostCalculator
    {
        public ShippingCostBreakdown CalculateShippingCosts(Parcel[] parcels)
        {
            return new ShippingCostBreakdown()
            {
                Parcels = parcels.Select(parcel => ParcelSize.CalculateForParcel(parcel)).ToArray()
            };
        }

    }
}