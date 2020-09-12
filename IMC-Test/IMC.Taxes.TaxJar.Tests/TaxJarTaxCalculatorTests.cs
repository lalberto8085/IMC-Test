using IMC.Taxes.Services;
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
        private readonly TaxRatesInfo fakeRates = new TaxRatesInfo()
        {
            Country = "US",
            CountryRate = 0.25m,
            IsFreightTaxable = false,
            County = "MDC",
            CountyRate = 0.125m,
            ZipCode = "33193"
        };
        private readonly TaxLocationInfo fakeLocation = new TaxLocationInfo
        {
            City = "Miami",
            Country = "US",
            State = "FL",
            ZipCode = "33193"
        };

        private Mock<IHttpClientFactory> clientFactory;

        [SetUp]
        public void SetUp()
        {
            clientFactory = new Mock<IHttpClientFactory>();
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
            var badLocation = new TaxLocationInfo { ZipCode = null };
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(badLocation), "The Location's zipcode is required");
        }


        [Test]
        public void LocationZipCodeValidation_empty()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var badLocation = new TaxLocationInfo { ZipCode = "" };
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(badLocation), "The Location's zipcode is required");
        }


        [Test]
        public void LocationZipCodeValidation_whiteSpace()
        {
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var badLocation = new TaxLocationInfo { ZipCode = "   " };
            Assert.Throws<ArgumentException>(() => calculator.TaxRatesForLocation(badLocation), "The Location's zipcode is required");
        }

        [Test]
        public void CreatesNamedClient()
        {
            SetupNamedClientWithFakeAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            calculator.TaxRatesForLocation(fakeLocation);
            clientFactory.Verify(f => f.CreateClient("TaxJar"), Times.Once);
        }

        [Test]
        public void ReturnsRatesObject()
        {
            SetupNamedClientWithFakeAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var rates = calculator.TaxRatesForLocation(fakeLocation);
            Assert.IsTrue(CompareRates(fakeRates, rates), "Rates are not equal");
        }

        [Test]
        public void ReturnsNullOnBadRequest()
        {
            SetupNamedClientWithFailingAnswer();
            var calculator = new TaxJarTaxCalculator(clientFactory.Object);
            var rates = calculator.TaxRatesForLocation(fakeLocation);
            Assert.IsNull(rates);
        }

        private bool CompareRates(TaxRatesInfo expected, TaxRatesInfo other)
        {
            var properties = typeof(TaxRatesInfo).GetProperties(System.Reflection.BindingFlags.DeclaredOnly);
            return properties.All(p => p.GetValue(expected) == p.GetValue(other));
        }

        private void SetupNamedClientWithFakeAnswer()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { rate = new TaxJarRates(fakeRates) }), Encoding.UTF8, "application/json")
            };
            var fakeHandler = new FakeMessageHandler(response);
            var client = new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("http://www.fake-address.com")
            };

            clientFactory.Setup(factory => factory.CreateClient(It.Is<string>(value => value == "TaxJar"))).Returns(client);
        }

        private void SetupNamedClientWithFailingAnswer()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var fakeHandler = new FakeMessageHandler(response);
            var client = new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("http://www.wrong-address.com")
            };

            clientFactory.Setup(factory => factory.CreateClient(It.Is<string>(value => value == "TaxJar"))).Returns(client);
        }
    }
}
