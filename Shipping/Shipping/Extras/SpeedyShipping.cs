namespace Shipping
{
    internal class SpeedyShipping : IExtra
    {
 
        public ShippingCostBreakdown Calculate(ShippingCostBreakdown shippingCostBreakdown)
        {
            shippingCostBreakdown.SpeedyShippingCostInDollars = shippingCostBreakdown.TotalCostInDollars;
            return shippingCostBreakdown;
        }
    }
}