using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services.Models
{
    public class LineItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ProductTaxCode { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Discount { get; set; }
    }
}
