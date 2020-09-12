using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services.Models
{
    public class OrderInfo
    {
        public Address From { get; set; }
        public Address To { get; set; }
        public decimal? Amount { get; set; }
        public decimal Shipping { get; set; }
        public string CustomerId { get; set; }
        public ExemptionType? ExemptionType { get; set; }

        public IEnumerable<NexusAddress> NexusAddresses { get; set; }
        public IEnumerable<LineItem> LineItems { get; set; }
    }
}
