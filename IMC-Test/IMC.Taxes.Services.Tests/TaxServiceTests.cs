using IMC.Taxes.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services.Tests
{
    class TaxServiceTests
    {
        [Test]
        public void CanBeCreated()
        {
            var mock = new Mock<ITaxCalculator>();
            var service = new TaxService(mock.Object);
            Assert.NotNull(service);
        }

        [Test]
        public void TaxRatesCallsTaxCalculator()
        {
            var mockMethodCalled = false;
            var mock = new Mock<ITaxCalculator>();
            mock.Setup(calculator => calculator.TaxRatesForLocation(It.IsAny<TaxLocationInfo>())).Callback(() => mockMethodCalled = true);

            var service = new TaxService(mock.Object);
            var location = new TaxLocationInfo();
            service.TaxRatesForLocation(location);

            Assert.IsTrue(mockMethodCalled);
        }

        [Test]
        public void CalculateTaxesForOrderCallsTaxCalculator()
        {
            var mockMethodCalled = false;
            var mock = new Mock<ITaxCalculator>();
            mock.Setup(calculator => calculator.CalculateTaxesForOrder(It.IsAny<OrderInfo>())).Callback(() => mockMethodCalled = true);

            var service = new TaxService(mock.Object);
            var order = new OrderInfo();
            service.CalculateTaxesForOrder(order);

            Assert.IsTrue(mockMethodCalled);
        }
    }
}
