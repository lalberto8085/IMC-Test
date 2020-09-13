using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using IMC.Taxes.TaxJar.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public OrderTaxInfo CalculateTaxesForOrder(OrderInfo orderInfo)
        {
            var taxJarOrder = new TaxJarOrder(orderInfo);
            var validationErrors = taxJarOrder.ValidationErrors();
            if (validationErrors.Any())
            {
                var aggregatedMessage = string.Join(Environment.NewLine, validationErrors);
                throw new ArgumentException("There is something wrong with the order data", nameof(orderInfo), new Exception(aggregatedMessage));
            }

            using (var client = clientFactory.CreateClient("TaxJar"))
            {
                var content = new StringContent(JsonConvert.SerializeObject(taxJarOrder), Encoding.UTF8, "application/json");
                try
                {
                    var response = client.PostAsync("taxes", content)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = ParseJsonResponseIntoAnonymousType(response, new { tax = new TaxJarOrderTax() });
                        return result.tax.ToOrderTaxInfo();
                    }
                    // log the response error
                    return null;
                }
                catch (Exception)
                {
                    // logging should be added here
                    // I decided to swallow the exeption instead of bubling it up
                    // this might change depending on how we want to wire the service 
                    // to deal with it
                    return null;
                }
            }
        }

        public TaxRatesInfo TaxRatesForLocation(TaxLocationInfo locationInfo)
        {
            if (string.IsNullOrWhiteSpace(locationInfo.ZipCode))
                throw new ArgumentException("The Location's zipcode is required", nameof(locationInfo.ZipCode));

            using (var client = clientFactory.CreateClient("TaxJar"))
            {
                try
                {
                    var response = client.GetAsync($"rates/{locationInfo.ZipCode}?{locationInfo.ToQueryString()}")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = ParseJsonResponseIntoAnonymousType(response, new { rate = new TaxJarRates() });
                        return result.rate.ToTaxRatesInfo();
                    }
                    // ToDo: log the error/warning 
                    return null;
                }
                catch (Exception)
                {
                    // logging should be added here
                    // I decided to swallow the exeption instead of bubling it up
                    // this might change depending on how we want to wire the service 
                    // to deal with it
                    return null;
                }
            }
        }

        private T ParseJsonResponseIntoAnonymousType<T>(HttpResponseMessage response, T sampleObject)
        {
            var contentString = response.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            var result = JsonConvert.DeserializeAnonymousType(contentString, sampleObject);
            return result;
        }
    }
}
