using System.Text.RegularExpressions;

namespace Api.GRRInnovations.SmartValidation.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomRegexAttribute : CustomValidationAttribute
    {
        private readonly string _pattern;

        public CustomRegexAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            return Regex.IsMatch(value.ToString(), _pattern);
        }
    }
}
