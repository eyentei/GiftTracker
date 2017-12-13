using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GiftTrackerClasses
{
    public class RequiredField : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "Please enter some text");
            }

            return new ValidationResult(true, null);
        }
    }
    public class DateValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var date = value is DateTime;
            if (!date || value == null)
            {
                return new ValidationResult(false, "Date can't be empty");
            }
            return new ValidationResult(true, null);
        }
    }
}
