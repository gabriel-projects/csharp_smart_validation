using Api.GRRInnovations.SmartValidation.Filters;
using Api.GRRInnovations.SmartValidation.Middlewares;
using Api.GRRInnovations.SmartValidation.Services;
using Microsoft.AspNetCore.Builder;

namespace Api.GRRInnovations.SmartValidation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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
