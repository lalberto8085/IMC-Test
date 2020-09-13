using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services.Models
{
    public class OrderTaxInfo
    {
        public decimal OrderTotalAmount { get; set; }
        public decimal Shipping { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal AmountToCollect { get; set; }
        public decimal Rate { get; set; }
        public bool HasNexus { get; set; }
        public bool FreightTaxable { get; set; }
        public string TaxSource { get; set; }
        public ExemptionType? ExemptionType { get; set; }
    }
}
