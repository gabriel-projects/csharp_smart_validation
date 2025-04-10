using Api.GRRInnovations.SmartValidation.Domain.Attributes;
using Api.GRRInnovations.SmartValidation.Domain.Models;
using Api.GRRInnovations.SmartValidation.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Xunit;

namespace Api.GRRInnovations.SmartValidation.Tests.Unit
{
    public class FilterTests : BaseTest
    {
        [Fact]
        public void ValidationFilter_ShouldReturnBadRequest_WhenEmailIsInvalid()
        {
            // Arrange
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                new ModelStateDictionary()
            );

            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "user", new User { Name = "Test User", Email = "invalid-email" } } },
                null
            );

            var filter = new ValidationFilter();

            // Act
            filter.OnActionExecuting(context);

            // Assert
            Assert.IsType<BadRequestObjectResult>(context.Result);

            var result = context.Result as BadRequestObjectResult;
            Assert.NotNull(result.Value);


            var response = JsonSerializer.Deserialize<ErrorResponse>(JsonSerializer.Serialize(result.Value));
            Assert.Equal("Erro de validação", response.Message);
            Assert.NotEmpty(response.Errors);
            Assert.Contains("Email inválido", response.Errors.FirstOrDefault().ToString());
        }

        [Fact]
        public void ValidationFilter_ShouldContinue_WhenEmailIsValid()
        {
            // Arrange
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                new ModelStateDictionary()
            );

            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "user", new User { Name = "Test User", Email = "test@example.com" } } },
                null
            );

            var filter = new ValidationFilter();

            // Act
            filter.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }
    }
} 