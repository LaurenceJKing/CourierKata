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
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 1.0,  WeightInKg = 0.5 } },
                { new Parcel() { WidthInCm = 9.9, BredthInCm = 9.9, HeightInCm = 9.9, WeightInKg = 0.99 } }
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

            [Theory]
            [MemberData(nameof(ExamplesOfSmallParcels))]
            public void Small_parcels_under_1kg_do_not_incur_an_extra_fee(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(3.00m);
            }

            [Theory]
            [InlineData(1.01, 5.00)]
            [InlineData(2.01, 7.00)]
            [InlineData(17.00, 35.00)]
            public void Small_parcels_over_1kg_incur_an_extra_fee_of_2_dollars_per_kg(double weightInKg, decimal expectedCostInDollars)
            {
                var parcel = new Parcel { WidthInCm = 1, BredthInCm = 1, HeightInCm = 1, WeightInKg = weightInKg };
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(expectedCostInDollars);
            }
        }

        public class MediumParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfMediumParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 10.0, WeightInKg = 1.00 } },
                { new Parcel() { WidthInCm = 49.9, BredthInCm = 49.9, HeightInCm = 49.9, WeightInKg = 2.9} }
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

            [Theory]
            [MemberData(nameof(ExamplesOfMediumParcels))]
            public void Medium_parcels_under_3kg_do_not_incur_an_extra_fee(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(8.00m);
            }

            [Theory]
            [InlineData(3.01, 10.00)]
            [InlineData(4.01, 12.00)]
            [InlineData(17.00, 36.00)]
            public void Medium_parcels_over_3kg_incur_an_extra_fee_of_2_dollars_per_kg(double weightInKg, decimal expectedCostInDollars)
            {
                var parcel = new Parcel { WidthInCm = 1, BredthInCm = 1, HeightInCm = 10, WeightInKg = weightInKg };
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(expectedCostInDollars);
            }
        }

        public class LargeParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfLargeParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 50.0, WeightInKg = 3.00 } },
                { new Parcel() { WidthInCm = 99.9, BredthInCm = 99.9, HeightInCm = 99.9, WeightInKg = 5.99 } }
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

            [Theory]
            [MemberData(nameof(ExamplesOfLargeParcels))]
            public void Large_parcels_under_6kg_do_not_incur_an_extra_fee(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(15.00m);
            }

            [Theory]
            [InlineData(6.01, 17.00)]
            [InlineData(7.01, 19.00)]
            [InlineData(17.00, 37.00)]
            public void Large_parcels_over_6kg_incur_an_extra_fee_of_2_dollars_per_kg(double weightInKg, decimal expectedCostInDollars)
            {
                var parcel = new Parcel { WidthInCm = 1, BredthInCm = 1, HeightInCm = 50, WeightInKg = weightInKg };
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(expectedCostInDollars);
            }
        }

        public class XLParcels : ShippingCostCalculatorTests
        {
            public static TheoryData<Parcel> ExamplesOfXLParcels =>
            new()
            {
                { new Parcel() { WidthInCm = 1.0, BredthInCm = 1.0, HeightInCm = 100.0, WeightInKg = 6.00 } },
                { new Parcel() { WidthInCm = 999.9, BredthInCm = 999.9, HeightInCm = 999.9, WeightInKg = 9.99 } }
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

            [Theory]
            [MemberData(nameof(ExamplesOfXLParcels))]
            public void XL_parcels_under_10kg_do_not_incur_an_extra_fee(Parcel parcel)
            {
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(25.00m);
            }

            [Theory]
            [InlineData(10.01, 27.00)]
            [InlineData(11.01, 29.00)]
            [InlineData(17.00, 39.00)]
            public void XL_parcels_over_10kg_incur_an_extra_fee_of_2_dollars_per_kg(double weightInKg, decimal expectedCostInDollars)
            {
                var parcel = new Parcel { WidthInCm = 1, BredthInCm = 1, HeightInCm = 100, WeightInKg = weightInKg };
                var shippingCostBreakdown = SUT.CalculateShippingCosts(new[] { parcel });

                shippingCostBreakdown.Parcels[0].ShippingCostInDollars.Should().Be(expectedCostInDollars);
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