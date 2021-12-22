namespace Shipping
{
    internal class None : IExtra
    {
        public ShippingCostBreakdown Calculate(ShippingCostBreakdown shippingCostBreakdown)
        {
            return shippingCostBreakdown;
        }
    }
}