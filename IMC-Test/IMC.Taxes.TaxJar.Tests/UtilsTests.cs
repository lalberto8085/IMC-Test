using IMC.Taxes.Services.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IMC.Taxes.TaxJar.Tests
{
    public class UtilsTests
    {
        [Test]
        public void CanTranslateExemptionType()
        {
            var values = Enum.GetValues(typeof(ExemptionType)).Cast<ExemptionType>().ToList();
            var translations = values.Select(Utils.TranslateTaxExemptionType).ToList();

            Assert.AreEqual(values.Count, translations.Count);

            var reversedValues = translations.Select(Utils.TranslateToExemptionTypeOrNull).ToList();

            Assert.AreEqual(values.Count, reversedValues.Count);
        }

        [Test]
        public void ReturnsNullIfNullInput()
        {
            Assert.IsNull(Utils.TranslateToExemptionTypeOrNull(null));
        }

        [Test]
        public void ThrowsIfExeptionTypeNotFound()
        {
            var input = "not a valid value";
            Assert.Throws<ArgumentException>(() => Utils.TranslateToExemptionTypeOrNull(input));
        }
    }
}
