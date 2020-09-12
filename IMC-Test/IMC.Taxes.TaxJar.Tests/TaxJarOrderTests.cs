using IMC.Taxes.TaxJar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMC.Taxes.TaxJar.Tests
{
    public class TaxJarOrderTests
    {
        private TaxJarOrder validOrder;

        [SetUp]
        public void Setup()
        {
            validOrder = new TaxJarOrder
            {
                from_country = "US",
                from_state = "Florida",
                from_city = "Miami",
                from_zip = "33193",
                to_country = "US",
                to_state = "CA",
                to_city = "San Francisco",
                to_zip = "94111",
                amount = 100m,
                shipping = 25m,
                customer_id = "customer",
                exemption_type = "non_exempt",
                nexus_addresses = new List<TaxJarNexusAddress> {
                    new TaxJarNexusAddress
                    {
                        id = "nexus1",
                        country = "US",
                        state = "FL",
                        city = "Miami",
                        zip = "33155"
                    }
                },
                line_items = new List<TaxJarLineItem>()
                {
                    new TaxJarLineItem
                    {
                        id = "line1",
                        discount = 0m,
                        product_tax_code = "P1TC",
                        unit_price = 22m,
                        quantity = 3
                    }
                }
            };
        }

        [Test]
        public void OriginalOrderIsValid()
        {
            Assert.IsEmpty(validOrder.ValidationErrors());
        }

        [Test]
        public void ValidatesRequiredFields()
        {
            validOrder.to_country = null;
            var errors = validOrder.ValidationErrors().ToList();
            Assert.Contains("Shipping country information required", errors);
        }

        [Test]
        public void AmountOrLineItemsRequired()
        {
            validOrder.line_items.Clear();
            validOrder.amount = null;
            var errors = validOrder.ValidationErrors().ToList();
            Assert.Contains("Either Amount or LineItems are required to calculate taxes", errors);
        }

        [Test]
        public void ZipCodeRequiredWhenShippingToUs()
        {
            validOrder.to_country = "US";
            validOrder.to_zip = null;
             var errors = validOrder.ValidationErrors().ToList();
            Assert.Contains("ZipCode required when shipping to US", errors);
        }

        [Test]
        public void StateIsRequiredWhenShippingToUS_CA()
        {
            validOrder.to_country = "CA";
            validOrder.to_state = null;
            var errors = validOrder.ValidationErrors().ToList();
            Assert.Contains("State is required when shipping to US or CA", errors);
        }

        [Test]
        public void NexusAddressOrFromInformationRequired()
        {
            validOrder.nexus_addresses.Clear();
            validOrder.from_country = null;
            var errors = validOrder.ValidationErrors().ToList();
            Assert.Contains("Either NexusAddress or From address information is required for tax calculation", errors);
        }
    }
}
