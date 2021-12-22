namespace Shipping
{
    internal abstract class ParcelType
    {
        public string Description => GetType().Name;
        public virtual double? MaxSizeInCm { get; } = null;
        public abstract double MaxWeightInKg { get; }
        public abstract decimal ShippingCostInDollars { get; }
        protected virtual ParcelType? Next { get; } = null;
        public virtual decimal ExcessWeightFeeInDollarsPerKg => 2.00m;

        private decimal CalculateShippingCostByWeight(Parcel parcel)
        {
            var excessWeight = (int)Math.Ceiling(parcel.WeightInKg - MaxWeightInKg);

            if (excessWeight < 0)
            {
                return ShippingCostInDollars;
            }

            return ShippingCostInDollars + (excessWeight * ExcessWeightFeeInDollarsPerKg);
        }

        public virtual bool TryCalculateShippingCost(Parcel parcel, out decimal shippingCost)
        {
            if (!ParcelIsWithinSizeLimit(parcel))
            {
                shippingCost = 0.00m;
                return false;
            }

            else
            {
                shippingCost = CalculateShippingCostByWeight(parcel);
                return true;
            }
        }

        private bool ParcelIsWithinSizeLimit(Parcel parcel)
        {
            if (MaxSizeInCm is null)
            {
                return true;
            }

            return
                parcel.WidthInCm < MaxSizeInCm &&
                parcel.HeightInCm < MaxSizeInCm &&
                parcel.BredthInCm < MaxSizeInCm;
        }

        internal static ShippingCostItem CalculateShippingCost(Parcel parcel)
        {
            var parcelType = (ParcelType)new ParcelType.Small();

            while (parcelType is not null)
            {
                var canCalculateShippingCosts = parcelType.TryCalculateShippingCost(parcel, out decimal shippingCost);

                if (!canCalculateShippingCosts)
                {
                    parcelType = parcelType.Next;
                    continue;
                }

                else if(parcelType.ShouldShipAsHeavyParcel(shippingCost))
                {
                    parcelType = new ParcelType.Heavy();
                    continue;
                }

                else
                {
                    return new ShippingCostItem
                    {
                        Description = parcelType.Description,
                        ShippingCostInDollars = shippingCost,
                    };
                }
            }
            throw new InvalidOperationException();
        }

        protected virtual bool ShouldShipAsHeavyParcel(decimal shippingCostInDollars)
        {
            return
                shippingCostInDollars > Heavy.ShippingCost &&
                this is not Heavy;
        }

        public class Heavy: ParcelType
        {
            internal static decimal ShippingCost = 50.00m;
            public override decimal ShippingCostInDollars => ShippingCost;
            public override double MaxWeightInKg => 50.00;
            public override decimal ExcessWeightFeeInDollarsPerKg => 1.00m;
        }

        public class Small : ParcelType
        {
            public override decimal ShippingCostInDollars => 3.00m;
            public override double? MaxSizeInCm => 10.0;
            public override double MaxWeightInKg => 1.00;
            protected override ParcelType Next => new ParcelType.Medium();
        }

        public class Medium : ParcelType
        {
            public override decimal ShippingCostInDollars => 8.00m;
            public override double? MaxSizeInCm => 50.0;
            public override double MaxWeightInKg => 3.00;
            protected override ParcelType Next => new ParcelType.Large();
        }

        public class Large: ParcelType
        {
            public override decimal ShippingCostInDollars => 15.00m;
            public override double? MaxSizeInCm => 100.0;
            public override double MaxWeightInKg => 6.00;
            protected override ParcelType Next => new ParcelType.XL();
        };

        public class XL : ParcelType
        {
            public override decimal ShippingCostInDollars => 25.00m;
            public override double MaxWeightInKg => 10.00;
        };
    }
}