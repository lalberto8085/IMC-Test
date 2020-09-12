using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.TaxJar.Models
{
    public class TaxJarRates
    {
        public string city { get; set; }
        public decimal? city_rate { get; set; }
        public decimal? combined_district_rate { get; set; }
        public decimal? combined_rate { get; set; }
        public string country { get; set; }
        public decimal? country_rate { get; set; }
        public string county { get; set; }
        public decimal? county_rate { get; set; }
        public bool freight_taxable { get; set; }
        public string state { get; set; }
        public decimal? state_rate { get; set; }
        public string zip { get; set; }
        public decimal? distance_sale_threshold { get; set; }
        public string name { get; set; }
        public decimal? parking_rate { get; set; }
        public decimal? reduced_rate { get; set; }
        public decimal? standard_rate { get; set; }
        public decimal? super_reduced_rate { get; set; }

        public TaxJarRates()
        {
        }

        public TaxJarRates(TaxRatesInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            city = info.City;
            city_rate = info.CityRate;
            combined_rate = info.CombinedRate;
            combined_district_rate = info.CombinedDistrictRate;
            country = info.Country;
            country_rate = info.CountryRate;
            county = info.County;
            county_rate = info.CountyRate;
            distance_sale_threshold = info.DistanceSaleThreshold;
            freight_taxable = info.IsFreightTaxable;
            name = info.Name;
            parking_rate = info.ParkingRate;
            reduced_rate = info.ReducedRate;
            standard_rate = info.StandardRate;
            state = info.State;
            state_rate = info.StateRate;
            super_reduced_rate = info.SuperReducedRate;
            zip = info.ZipCode;
        }

        public TaxRatesInfo ToTaxRatesInfo()
        {
            return new TaxRatesInfo
            {
                City = city,
                CityRate = city_rate,
                CombinedRate = combined_rate,
                CombinedDistrictRate = combined_district_rate,
                Country = country,
                CountryRate = country_rate,
                County = county,
                CountyRate = county_rate,
                DistanceSaleThreshold = distance_sale_threshold,
                IsFreightTaxable = freight_taxable,
                Name = name,
                ParkingRate = parking_rate,
                ReducedRate = reduced_rate,
                StandardRate = standard_rate,
                State = state,
                StateRate = state_rate,
                SuperReducedRate = super_reduced_rate,
                ZipCode = zip
            };
        }
    }
}
