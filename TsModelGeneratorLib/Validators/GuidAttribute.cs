using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class GuidAttribute : ValidationAttribute
    {
        static Regex _r = new("^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (! (value is string valueAsString))
            {
                return false;
            }

            return _r.IsMatch(valueAsString);
        }
    }


}
