using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using IMC.Taxes.TaxJar.Models;
using IMC.Taxes.TaxJar.Tests.Fakes;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace IMC.Taxes.TaxJar.Tests
{
    public class TaxJarTaxCalculatorTests
    {
        private readonly TaxJarRates fakeRates = new TaxJarRates
        {
            country = "US",
            country_rate = 0.15m,
            freight_taxable = false,
            county = "MDC",
            county_rate = 0.1255m,
            zip = "33193"
        };
        private readonly TaxJarOrderTax fakeOrderTax = new TaxJarOrderTax
        {
            amount_to_collect = 10.5m,
            freight_taxable = true,
            has_nexus = false,
            order_total_amount = 30m,
            taxable_amount = 25m,
            exemption_type = "non_exempt"
        };

        private TaxLocationInfo validLocation;

        private OrderInfo validOrder;
        private Mock<IHttpClientFactory> clientFactory;

        [SetUp]
        public void SetUp()
        {
            clientFactory = new Mock<IHttpClientFactory>();
            validOrder = new OrderInfo
            {
                Amount = 25m,
                ExemptionType = ExemptionType.Marketplace,
                CustomerId = "customer1",
                Shipping = 32m,
                From = new Address
                {
                    Country = "US",
                    State = "NJ",
                    ZipCode = "07001"
                },
                To = new Address
                {
                    Country = "US",
                    State = "Florida",
                    ZipCode = "33155"
                },
                NexusAddresses = new NexusAddress[]
                {
                    new NexusAddress
                    {
                        Id = "Nexus1",
                        Country = "US",
                        State = "FL",
                        ZipCode = "32801"
                    }
                },
                LineItems = new LineItem[]
                {
                    new LineItem
                    {
                        Id = "Line1",
                        UnitPrice = 15m,
                        Quantity = 2,
                        Discount = 5m,
                        ProductTaxCode = "Code1"
                    }
                }
            };
            validLocation = new TaxLocationInfo
            {
                City = "Miami",
                Country = "US",
                State = "FL",
                ZipCode = "33193"
            };
        }

        [Test]
        public void CanCreate()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            Assert.NotNull(calculator);
        }

        [Test]
        public void LocationZipCodeValidation_null()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            validLocation.ZipCode = null;
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(validLocation), "The Location's zipcode is required");
        }


        [Test]
        public void LocationZipCodeValidation_empty()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            validLocation.ZipCode = "";
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(validLocation), "The Location's zipcode is required");
        }


        [Test]
        public void LocationZipCodeValidation_whiteSpace()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            validLocation.ZipCode = "   ";
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(validLocation), "The Location's zipcode is required");
        }

        [Test]
        public void CreatesNamedClient()
        {
            SetupNamedClientWithFakeAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            calculator.TaxRatesForLocation(validLocation);
            clientFactory.Verify(f => f.CreateClient("TaxJar"), Times.Once);
        }

        [Test]
        public void ReturnsRatesObject()
        {
            SetupNamedClientWithFakeAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var rates = calculator.TaxRatesForLocation(validLocation);
            Assert.IsTrue(CompareAllProperties(fakeRates.ToTaxRatesInfo(), rates), "Rates are not equal");
        }

        [Test]
        public void ReturnsNullOnBadRequest()
        {
            SetupNamedClientWithFailingAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var rates = calculator.TaxRatesForLocation(validLocation);
            Assert.IsNull(rates);
        }

        [Test]
        public void ReturnsNullOnExceptionThrown()
        {
            SetupNamedClientToThrow();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var rates = calculator.TaxRatesForLocation(validLocation);
            Assert.IsNull(rates);
        }

        [Test]
        public void CalculateTaxesChecksValidationMessages()
        {
            validOrder.To.State = null;
            clientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            Assert.Throws<ArgumentException>(() => calculator.CalculateTaxesForOrder(validOrder));
            clientFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void CalculateTaxesCreatesNamedClient()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            calculator.CalculateTaxesForOrder(validOrder);
            clientFactory.Verify(f => f.CreateClient("TaxJar"), Times.Once);
        }

        [Test]
        public void CalculateTaxesReturnsTaxesObject()
        {
            SetupNamedClientWithFakeAnswerForCalculateTaxes();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var taxInfo = calculator.CalculateTaxesForOrder(validOrder);
            Assert.IsTrue(CompareAllProperties(fakeOrderTax.ToOrderTaxInfo(), taxInfo));
        }

        [Test]
        public void CalculateTaxesReturnsNullOnBadRequest()
        {
            SetupNamedClientWithFailingAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var taxInfo = calculator.CalculateTaxesForOrder(validOrder);
            Assert.IsNull(taxInfo);
        }

        [Test]
        public void CalculateTaxesReturnsNullOnExceptionThrown()
        {
            SetupNamedClientToThrow();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var taxInfo = calculator.CalculateTaxesForOrder(validOrder);
            Assert.IsNull(taxInfo);
        }

        private bool CompareAllProperties<T>(T expected, T other)
        {
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.DeclaredOnly);
            return properties.All(p => p.GetValue(expected) == p.GetValue(other));
        }

        private void SetupNamedClientWithFakeAnswer()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { rate = fakeRates }), Encoding.UTF8, "application/json")
            };
            SetupNamedClientWithFakeResponse(response);
        }

        private void SetupNamedClientWithFailingAnswer()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            SetupNamedClientWithFakeResponse(response);
        }

        private void SetupNamedClientWithFakeAnswerForCalculateTaxes()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(fakeOrderTax), Encoding.UTF8, "application/json")
            };
            SetupNamedClientWithFakeResponse(response);
        }

        private void SetupNamedClientToThrow()
        {
            var handler = new ExceptionThrowingMessageHandler();
            SetupNamedClientWithHandler(handler);
        }

        private void SetupNamedClientWithFakeResponse(HttpResponseMessage response)
        {
            var fakeHandler = new FakeMessageHandler(response);
            SetupNamedClientWithHandler(fakeHandler);
        }

        private void SetupNamedClientWithHandler(HttpMessageHandler handler)
        {
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://www.fake-url.com/step")
            };
            clientFactory.Setup(f => f.CreateClient(It.Is<string>(value => value == "TaxJar"))).Returns(client);
        }
    }
}
