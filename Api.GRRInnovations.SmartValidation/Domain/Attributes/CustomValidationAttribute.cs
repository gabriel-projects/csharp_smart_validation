namespace Api.GRRInnovations.SmartValidation.Domain.Attributes
{
    public abstract class CustomValidationAttribute : Attribute
    {
        public required string ErrorMessage { get; set; }

        public abstract bool IsValid(object value);
    }
}
