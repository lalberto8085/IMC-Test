using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services
{
    public interface ITaxCalculator
    {
        TaxRatesInfo TaxRatesForLocation(TaxLocationInfo locationInfo);
    }
}
