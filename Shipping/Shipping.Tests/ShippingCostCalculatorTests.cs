using Xunit;
using FluentAssertions;
using AutoFixture.Xunit2;

namespace Shipping.Tests
{
    public class ShippingCostCalculatorTests
    {
        public ShippingCostCalculator SUT { get; } = new ShippingCostCalculator();

        public class SmallParcels: ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfSmallParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 1.0 } },
                { new Parcel() { WidthInCm = 9.9, BredthInCm = 9.9, HeightInCm = 9.9 } }
            };

            [Theory]
            [MemberData(nameof(ExamplesOfSmallParcels))]
            public void Small_parcels_have_dimentions_of_less_than_10cm(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].Description.Should().Be(ParcelSize.Small.Description);
            }

            [Theory]
            [MemberData(nameof(ExamplesOfSmallParcels))]
            public void Small_parcels_cost_3_dollars_to_ship(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(3.00m);
            }
        }

        public class MediumParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfMediumParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 10.0 } },
                { new Parcel() { WidthInCm = 49.9, BredthInCm = 49.9, HeightInCm = 49.9 } }
            };

            [Theory]
            [MemberData(nameof(ExamplesOfMediumParcels))]
            public void Medium_parcels_have_dimentions_that_are_less_than_50cm(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].Description.Should().Be(ParcelSize.Medium.Description);
            }

            [Theory]
            [MemberData(nameof(ExamplesOfMediumParcels))]
            public void Medium_parcels_cost_8_dollars_to_ship(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(8.00m);
            }
        }

        public class LargeParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfLargeParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 50.0 } },
                { new Parcel() { WidthInCm = 99.9, BredthInCm = 99.9, HeightInCm = 99.9 } }
            };

            [Theory]
            [MemberData(nameof(ExamplesOfLargeParcels))]
            public void Large_parcels_have_dimentions_that_are_less_than_100cm(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].Description.Should().Be(ParcelSize.Large.Description);
            }

            [Theory]
            [MemberData(nameof(ExamplesOfLargeParcels))]
            public void Large_parcels_cost_15_dollars_to_ship(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(15.00m);
            }
        }

        public class XLParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfXLParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 100.0 } },
                { new Parcel() { WidthInCm = 999.9, BredthInCm = 999.9, HeightInCm = 999.9 } }
            };

            [Theory]
            [MemberData(nameof(ExamplesOfXLParcels))]
            public void Large_parcels_have_dimentions_that_are_less_than_100cm(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].Description.Should().Be(ParcelSize.XL.Description);
            }

            [Theory]
            [MemberData(nameof(ExamplesOfXLParcels))]
            public void Large_parcels_cost_15_dollars_to_ship(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(25.00m);
            }
        }

        public class Without_speedy_shipping: ShippingCostCalculatorTests
        {

            [Fact]
            public void The_total_cost_is_the_sum_of_the_shipping_costs_of_each_parcel()
            {
                var parcels = new[]
                {
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 1.0 }, //small parcel: $3
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 10.0 }, //medium parcel:$8
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 50.0 }, //large pacel: $15
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 100.0 } //XL parcel: $25
            };

                var shippingCostBreakdown = SUT
                    .WithShippingSpeed(ShippingSpeed.Normal)
                    .CalculateShippingCosts(parcels);

                shippingCostBreakdown.SpeedyShippingCostInDollars.Should().Be(0.00m);
                shippingCostBreakdown.TotalCostInDollars.Should().Be(51.00m);
            }
        }

        public class WithSpeedyShipping: ShippingCostCalculatorTests
        {
            [Fact]
            public void The_total_cost_is_twice_the_sum_of_the_shipping_costs_of_each_parcel()
            {
                var parcels = new[]
                {
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 1.0 }, //small parcel: $3
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 10.0 }, //medium parcel:$8
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 50.0 }, //large pacel: $15
                new Parcel{ WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 100.0 } //XL parcel: $25
            };

                var shippingCostBreakdown = SUT
                    .WithShippingSpeed(ShippingSpeed.Speedy)
                    .CalculateShippingCosts(parcels);

                shippingCostBreakdown.SpeedyShippingCostInDollars.Should().Be(51.00m);
                shippingCostBreakdown.TotalCostInDollars.Should().Be(102.00m);
            }

        }
    }


}