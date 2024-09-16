using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class TimeAttribute : ValidationAttribute
    {
        private static Regex _r12 = new Regex("(((0[1-9])|(1[0-2])):([0-5])(0|5)(:[0-5][0-9])\\s(A|P|a|p)(M|m))");

        private static Regex _r24 = new Regex("^([0-1][0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$");

        public bool Is24hFormat { get; set; } = true;

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

            return Is24hFormat ? _r24.IsMatch(valueAsString) : _r24.IsMatch(valueAsString);
        }
    }
}
