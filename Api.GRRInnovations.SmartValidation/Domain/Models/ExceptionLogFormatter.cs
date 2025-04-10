namespace Api.GRRInnovations.SmartValidation.Domain.Models
{
    public static class ExceptionLogFormatter
    {
        public static object Format(Exception ex, HttpContext context)
        {
            return new
            {
                ExceptionType = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Path = context.Request.Path,
                Method = context.Request.Method,
                Timestamp = DateTime.UtcNow,
                QueryString = context.Request.QueryString.ToString(),
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
