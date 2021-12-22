namespace Shipping
{
    public record ParcelSize
    {
        public string Description { get; private init; }
        public double? MaxSizeInCm { get; private init; }
        public double MaxWeightInKg { get; private init; }
        public decimal ShippingCostInDollars { get; private init; }

        private const decimal WeightLimitExceededFee = 2.00m;

        private ParcelSize? NextSizeUp { get; init; }

        public static ParcelSize Small => new()
        {
            Description = nameof(Small),
            ShippingCostInDollars = 3.00m,
            MaxSizeInCm = 10,
            MaxWeightInKg = 1,
            NextSizeUp = ParcelSize.Medium
        };

        public static ParcelSize Medium => new()
        {
            Description = nameof(Medium),
            ShippingCostInDollars = 8.00m,
            MaxSizeInCm = 50,
            MaxWeightInKg = 3,
            NextSizeUp = ParcelSize.Large
        };

        public static ParcelSize Large => new()
        {
            Description = nameof(Large),
            ShippingCostInDollars = 15.00m,
            MaxSizeInCm = 100,
            MaxWeightInKg = 6,
            NextSizeUp= ParcelSize.XL
        };

        public static ParcelSize XL => new()
        {
            Description = nameof(XL),
            ShippingCostInDollars = 25.00m,
            MaxWeightInKg = 10
        };

        private bool CheckParcelIsWithinSizeLimit(Parcel parcel)
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

        private decimal CalculateShippingCost(Parcel parcel)
        {
            var excessWeight = (int)Math.Ceiling(parcel.WeightInKg - MaxWeightInKg);

            if(excessWeight < 0)
            {
                return ShippingCostInDollars;
            }

            return ShippingCostInDollars + (excessWeight * 2.00m);
        }


        internal static ShippingCostItem CalculateForParcel(Parcel parcel)
        {
            var parcelSize = ParcelSize.Small;

            while (parcelSize is not null)
            {
                if (parcelSize.CheckParcelIsWithinSizeLimit(parcel))
                {
                    return new ShippingCostItem
                    {
                        ParcelSize = parcelSize,
                        ShippingCostInDollars = parcelSize.CalculateShippingCost(parcel)
                    };
                }

                parcelSize = parcelSize.NextSizeUp;
            }

            throw new InvalidOperationException();
        }
    }
}