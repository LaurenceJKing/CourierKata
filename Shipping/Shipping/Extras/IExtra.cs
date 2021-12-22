namespace Shipping
{
    internal interface IExtra
    {
        public ShippingCostBreakdown Calculate(ShippingCostBreakdown shippingCostBreakdown);
    }
}