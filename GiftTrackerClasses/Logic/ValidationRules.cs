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
                return new ValidationResult(false, "Name can't be empty");
            }

            return ValidationResult.ValidResult;
        }
    }
    public class EmptyDateValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var date = value is DateTime;
            if (!date || value == null)
            {
                return new ValidationResult(false, "Date can't be empty");
            }
            return ValidationResult.ValidResult;
        }
    }
    public class IncorrectDateValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if ((DateTime)value < DateTime.Now.Date)
            {
                return new ValidationResult(false, "Date can't be less than current date");
            }
            return ValidationResult.ValidResult;
        }
    }
    public class EmptyCollectionValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Please, select at least one");
            }

            return new ValidationResult(false, value as string);

        }
    }
}
