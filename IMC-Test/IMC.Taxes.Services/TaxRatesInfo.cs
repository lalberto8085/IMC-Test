using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services
{
    public class TaxRatesInfo
    {
        public string Country { get; set; }
        public decimal? CountryRate { get; set; }
        public string State { get; set; }
        public decimal? StateRate { get; set; }
        public string County { get; set; }
        public decimal? CountyRate { get; set; }
        public string City { get; set; }
        public decimal? CityRate { get; set; }
        public decimal? CombinedRate { get; set; }
        public decimal? CombinedDistrictRate { get; set; }
        public bool IsFreightTaxable { get; set; }
        public string ZipCode { get; set; }
        public decimal? DistanceSaleThreshold { get; set; }
        public string Name { get; set; }
        public decimal? ParkingRate { get; set; }
        public decimal? ReducedRate { get; set; }
        public decimal? StandardRate { get; set; }
        public decimal? SuperReducedRate { get; set; }
    }
}
