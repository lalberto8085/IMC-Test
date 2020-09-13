using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMC.Taxes.TaxJar.Models
{
    public class TaxJarOrder
    {
        public string from_country { get; set; }
        public string from_zip { get; set; }
        public string from_state { get; set; }
        public string from_city { get; set; }
        public string from_street { get; set; }
        public string to_country { get; set; }
        public string to_zip { get; set; }
        public string to_state { get; set; }
        public string to_city { get; set; }
        public string to_street { get; set; }
        public decimal? amount { get; set; }
        public decimal shipping { get; set; }
        public string customer_id { get; set; }
        public string exemption_type { get; set; }

        public List<TaxJarNexusAddress> nexus_addresses { get; set; }
        public List<TaxJarLineItem> line_items { get; set; }

        public TaxJarOrder()
        {
            nexus_addresses = new List<TaxJarNexusAddress>();
            line_items = new List<TaxJarLineItem>();
        }

        public TaxJarOrder(OrderInfo order) : this()
        {
            from_country = order.From.Country;
            from_zip = order.From.ZipCode;
            from_state = order.From.State;
            from_city = order.From.City;
            from_street = order.From.Street;
            to_country = order.To.Country;
            to_zip = order.To.ZipCode;
            to_state = order.To.State;
            to_city = order.To.City;
            to_street = order.To.Street;
            amount = order.Amount;
            shipping = order.Shipping;
            customer_id = order.CustomerId;
            exemption_type = TranslateExemptionType(order.ExemptionType.Value);

            nexus_addresses.AddRange(order.NexusAddresses.Select(a => new TaxJarNexusAddress(a)));
            line_items.AddRange(order.LineItems.Select(item => new TaxJarLineItem(item)));
        }

        public IEnumerable<string> ValidationErrors()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(to_country))
                errors.Add("Shipping country information required");
            if (!amount.HasValue && line_items.Count == 0)
                errors.Add("Either Amount or LineItems are required to calculate taxes");
            if (to_country == "US" && string.IsNullOrWhiteSpace(to_zip))
                errors.Add("ZipCode required when shipping to US");
            if ((to_country == "US" || to_country == "CA") && string.IsNullOrWhiteSpace(to_state))
                errors.Add("State is required when shipping to US or CA");
            if (nexus_addresses.Count == 0 && (string.IsNullOrWhiteSpace(from_country)))
                errors.Add("Either NexusAddress or From address information is required for tax calculation");
            if (nexus_addresses.Any(na => string.IsNullOrWhiteSpace(na.country) || string.IsNullOrWhiteSpace(na.state)))
                errors.Add("Nexus address requires Country and State code");
            return errors;
        }

        private string TranslateExemptionType(ExemptionType? exemptionType)
        {
            if (!exemptionType.HasValue)
                return null;
            return Utils.TranslateTaxExemptionType(exemptionType.Value);
        }
    }
}
