using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace KKBank.Web.ViewModels.ValidationAttributes
{
    public class DateLessThanOrEqualToToday : ValidationAttribute
    {
        private const string DateTimeFormat = "dd/MM/yyyy";
        private DateTime date;

        public override string FormatErrorMessage(string name)
        {
            return "Date value should not be a future date!";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("Please choose a date!");
            }

            try
            {
                date = DateTime.ParseExact(value.ToString(), DateTimeFormat, null, DateTimeStyles.None);
            }
            catch
            {
                return new ValidationResult("Value should be a date!");
            }

            if (date.Date > DateTime.Now.Date)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
