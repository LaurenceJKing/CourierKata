namespace Shipping
{
    public class ShippingCostCalculator
    {
        private IExtra SpeedyShipping { get; set; } = new None();

        public ShippingCostBreakdown CalculateShippingCosts(Parcel[] parcels)
        {
            var shippingCosts = new ShippingCostBreakdown()
            {
                Parcels = parcels.Select(parcel => ParcelType.CalculateShippingCost(parcel)).ToArray()
            };

            return SpeedyShipping.Calculate(shippingCosts);
        }
        public ShippingCostCalculator WithShippingSpeed(ShippingSpeed shippingSpeed)
        {
            SpeedyShipping = shippingSpeed switch
            {
                ShippingSpeed.Normal => new None() as IExtra,
                ShippingSpeed.Speedy => new SpeedyShipping(),
                _ => SpeedyShipping
            };

            return this;
        }
    }

    public enum ShippingSpeed
    {
        Normal,
        Speedy
    }
}