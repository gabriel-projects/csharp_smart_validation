using Api.GRRInnovations.SmartValidation.Domain.Models;
using System.Net;

namespace Api.GRRInnovations.SmartValidation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var logData = ExceptionLogFormatter.Format(ex, context);

                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex, "❌ Exceção detalhada: {@LogData}", logData);
                }
                else
                {
                    _logger.LogError("❌ Erro não tratado em produção: {ExceptionType} - {Message}",
                        ex.GetType().Name, ex.Message);
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                object response = _env.IsDevelopment()
                    ? new { message = ex.Message, stackTrace = ex.StackTrace }
                    : new { message = "Ocorreu um erro inesperado." };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
