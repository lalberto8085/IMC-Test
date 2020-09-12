using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using IMC.Taxes.TaxJar;

namespace IMC.Taxes.TaxJar.Tests
{
    public class ExtensionsTests
    {
        [Test]
        public void NullDictionaryToQueryString()
        {
            IDictionary<string, object> data = null;
            Assert.Throws<ArgumentNullException>(() => data.ToQueryString());
        }

        [Test]
        public void EmptyDictionaryReturnsEmptyString()
        {
            var data = new Dictionary<string, object>();
            Assert.AreEqual("", data.ToQueryString());
        }

        [Test]
        public void SingleElementDictionaryHasNoAmpersands()
        {
            var data = new Dictionary<string, object>
            {
                ["single"] = 4
            };
            Assert.AreEqual("single=4", data.ToQueryString());
        }

        [Test]
        public void PairsWithNullValuesAreIgnored()
        {
            var data = new Dictionary<string, object>()
            {
                ["first"] = "1",
                ["second"] = null
            };
            Assert.AreEqual("first=1", data.ToQueryString());
        }

        [Test]
        public void MultipleValueConcatenation()
        {
            var data = new Dictionary<string, object>()
            {
                ["first"] = "1",
                ["second"] = 2
            };
            Assert.AreEqual("first=1&second=2", data.ToQueryString());
        }
    }
}
