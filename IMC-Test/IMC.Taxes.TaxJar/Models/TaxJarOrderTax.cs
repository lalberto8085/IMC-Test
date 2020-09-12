using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.TaxJar.Models
{
    /* 
     * there is a lot more information on the order from TaxJar
     * I decided to ignore it for brevity, 
     * Adding it won't impact functionality or the rest of the code
     * in any way other than modifying the fields on this class
     */
    public class TaxJarOrderTax
    {
        public decimal order_total_amount { get; set; }
        public decimal shipping { get; set; }
        public decimal taxable_amount { get; set; }
        public decimal amount_to_collect { get; set; }
        public decimal rate { get; set; }
        public bool has_nexus { get; set; }
        public bool freight_taxable { get; set; }
        public string tax_source { get; set; }
        public string exemption_type { get; set; }
    }
}
