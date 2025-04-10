using Api.GRRInnovations.SmartValidation.Filters;
using Api.GRRInnovations.SmartValidation.Middlewares;
using Api.GRRInnovations.SmartValidation.SerilogConfigs;
using Api.GRRInnovations.SmartValidation.Services;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Api.GRRInnovations.SmartValidation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;

                if (env.IsDevelopment())
                {
                    loggerConfiguration
                        .MinimumLevel.Debug()
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                            theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate)
                        .Enrich.FromLogContext()
                    .WriteTo.File(
                        new IndentedJsonFormatter(),
                        "Logs/log-dev.json",
                        rollingInterval: RollingInterval.Day);
                }
                else
                {
                    loggerConfiguration
                        .MinimumLevel.Information()
                        .WriteTo.Console(new IndentedJsonFormatter())
                        .Enrich.FromLogContext();
                }
            });

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ValidationAttributesService>();
            builder.Services.AddScoped<ValidationFilter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
