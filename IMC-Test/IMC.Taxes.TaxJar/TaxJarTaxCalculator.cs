using IMC.Taxes.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IMC.Taxes.TaxJar
{
    public class TaxJarTaxCalculator : ITaxCalculator
    {
        private readonly IHttpClientFactory clientFactory;

        public TaxJarTaxCalculator(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public TaxRatesInfo TaxRatesForLocation(TaxLocationInfo locationInfo)
        {
            if (string.IsNullOrWhiteSpace(locationInfo.ZipCode))
                throw new ArgumentException("The Location's zipcode is required", nameof(locationInfo.ZipCode));

            using (var client = clientFactory.CreateClient("TaxJar"))
            {
                var response = client.GetAsync($"rates/{locationInfo.ZipCode}?{locationInfo.ToQueryString()}").ConfigureAwait(false).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var result = JsonConvert.DeserializeAnonymousType(content, new { rate = new TaxJarRates() });
                    return result.rate.ToTaxRatesInfo();
                }
                // ToDo: log the error/warning 
                return null;
            }
        }
    }
}
