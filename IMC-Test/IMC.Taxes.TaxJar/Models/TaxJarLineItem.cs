using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.TaxJar.Models
{
    public class TaxJarLineItem
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public string product_tax_code { get; set; }
        public decimal? unit_price { get; set; }
        public decimal? discount { get; set; }

        public TaxJarLineItem()
        {
        }

        public TaxJarLineItem(LineItem lineItem)
        {
            id = lineItem.Id;
            quantity = lineItem.Quantity;
            product_tax_code = lineItem.ProductTaxCode;
            unit_price = lineItem.UnitPrice;
            discount = lineItem.Discount;
        }
    }
}
