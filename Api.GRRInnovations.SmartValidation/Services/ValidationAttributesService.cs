using Api.GRRInnovations.SmartValidation.Domain.Attributes;

namespace Api.GRRInnovations.SmartValidation.Services
{
    public class ValidationAttributesService
    {
        public List<string> Validate(object obj)
        {
            var errors = new List<string>();
            var proprieties = obj.GetType().GetProperties();

            foreach (var prop in proprieties)
            {
                var attributes = prop.GetCustomAttributes(typeof(CustomValidationAttribute), true);

                foreach (CustomValidationAttribute attribute in attributes)
                {
                    var value = prop.GetValue(obj);

                    if (!attribute.IsValid(value))
                    {
                        errors.Add($"{prop.Name}: {attribute.ErrorMessage}");
                    }
                }
            }

            return errors;
        }
    }
}
