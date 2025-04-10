namespace Api.GRRInnovations.SmartValidation.Domain.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
