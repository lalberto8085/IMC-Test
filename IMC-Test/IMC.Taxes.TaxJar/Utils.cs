using IMC.Taxes.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.TaxJar
{
    public class Utils
    {
        public static string TranslateTaxExemptionType(ExemptionType exemptionType)
        {
            switch (exemptionType)
            {
                case ExemptionType.Wholesale:
                    return "wholesale";
                case ExemptionType.Government:
                    return "government";
                case ExemptionType.Marketplace:
                    return "marketplace";
                case ExemptionType.Other:
                    return "other";
                case ExemptionType.NonExempt:
                    return "non_exempt";
                default:
                    throw new ArgumentOutOfRangeException(nameof(exemptionType));
            }
        }

        public static ExemptionType? TranslateToExemptionTypeOrNull(string exemptionType)
        {
            if (exemptionType == null)
                return null;

            if (exemptionType.Equals("wholesale", StringComparison.OrdinalIgnoreCase))
                return ExemptionType.Wholesale;
            if (exemptionType.Equals("government", StringComparison.OrdinalIgnoreCase))
                return ExemptionType.Government;
            if (exemptionType.Equals("marketplace", StringComparison.OrdinalIgnoreCase))
                return ExemptionType.Marketplace;
            if (exemptionType.Equals("other", StringComparison.OrdinalIgnoreCase))
                return ExemptionType.Other;
            if (exemptionType.Equals("non_exempt", StringComparison.OrdinalIgnoreCase))
                return ExemptionType.NonExempt;

            throw new ArgumentException($"Value {exemptionType} not found", nameof(exemptionType));
        }
    }
}
