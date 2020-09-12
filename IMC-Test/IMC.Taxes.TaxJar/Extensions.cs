using IMC.Taxes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMC.Taxes.TaxJar
{
    public static class Extensions
    {
        public static string ToQueryString(this TaxLocationInfo locationInfo)
        {
            var dictionary = new Dictionary<string, object>
            {
                ["country"] = locationInfo.Country,
                ["state"] = locationInfo.State,
                ["city"] = locationInfo.City,
                ["street"] = locationInfo.Street
            };

            return dictionary.ToQueryString();
        }


        public static string ToQueryString(this IDictionary<string, object> data)
        {
            return String.Join("&", data.Where(pair => pair.Value != null).Select(pair => $"{pair.Key}={pair.Value}"));
        }
    }
}
