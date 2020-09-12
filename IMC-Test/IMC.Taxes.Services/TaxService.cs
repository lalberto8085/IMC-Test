using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services
{
    public class TaxService
    {
        private readonly ITaxCalculator taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            this.taxCalculator = taxCalculator;
        }

        public TaxRatesInfo TaxRatesForLocation(TaxLocationInfo locationInfo)
        {
            return taxCalculator.TaxRatesForLocation(locationInfo);
        }

        public OrderTaxInfo CalculateTaxesForOrder(OrderInfo orderInfo)
        {
            return taxCalculator.CalculateTaxesForOrder(orderInfo);
        }
    }
}
