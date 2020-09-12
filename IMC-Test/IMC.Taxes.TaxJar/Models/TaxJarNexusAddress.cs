using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.TaxJar.Models
{
    public class TaxJarNexusAddress
    {
        public string id { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string street { get; set; }

        public TaxJarNexusAddress()
        {
        }

        public TaxJarNexusAddress(NexusAddress address)
        {
            id = address.Id;
            country = address.Country;
            zip = address.ZipCode;
            state = address.State;
            city = address.City;
            street = address.Street;
        }
    }
}
