namespace Shipping
{
    public record ParcelSize
    {
        public string Description { get; private init; }
        public double? MaxSizeInCm { get; private init; }
        public decimal ShippingCostInDollars { get; private init; }

        private ParcelSize? NextSizeUp { get; init; }

        public static ParcelSize Small => new()
        {
            Description = nameof(Small),
            ShippingCostInDollars = 3.00m,
            MaxSizeInCm = 10,
            NextSizeUp = ParcelSize.Medium
        };

        public static ParcelSize Medium => new()
        {
            Description = nameof(Medium),
            ShippingCostInDollars = 8.00m,
            MaxSizeInCm = 50,
            NextSizeUp = ParcelSize.Large
        };

        public static ParcelSize Large => new()
        {
            Description = nameof(Large),
            ShippingCostInDollars = 15.00m,
            MaxSizeInCm = 100,
            NextSizeUp= ParcelSize.XL
        };

        public static ParcelSize XL => new()
        {
            Description = nameof(XL),
            ShippingCostInDollars = 25.00m
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


        internal static ParcelSize CalculateForParcel(Parcel parcel)
        {
            var parcelSize = ParcelSize.Small;

            while (parcelSize is not null)
            {
                if (parcelSize.CheckParcelIsWithinSizeLimit(parcel))
                {
                    return parcelSize;
                }

                parcelSize = parcelSize.NextSizeUp;
            }

            throw new InvalidOperationException();
        }
    }
}