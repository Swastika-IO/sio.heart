using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace TTS.Lib.Attributes
{
    public class CountrySpecificAttribute : Attribute, IActionConstraint
    {
        private readonly string _countryCode;

        public CountrySpecificAttribute(string countryCode)
        {
            _countryCode = countryCode;
        }

        public int Order
        {
            get
            {
                return 0;
            }
        }

        public bool Accept(ActionConstraintContext context)
        {
            string culture = context.RouteContext.RouteData.Values["culture"].ToString();
            return string.Equals(
                culture,
                _countryCode,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
