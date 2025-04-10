using Api.GRRInnovations.SmartValidation.Middlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;

namespace Api.GRRInnovations.SmartValidation.Tests.Unit
{
    public class MiddlewareTests : BaseTest
    {
        private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<IHostEnvironment> _envMock;

        public MiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            _nextMock = new Mock<RequestDelegate>();
            _envMock = new Mock<IHostEnvironment>();
        }

        [Fact]
        public async Task ExceptionMiddleware_ShouldHandleException_WhenInDevelopment()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _nextMock.Setup(x => x(It.IsAny<HttpContext>()))
                    .Throws(new Exception("Test exception"));

            _envMock.Setup(x => x.EnvironmentName).Returns("Development");

            var middleware = new ExceptionMiddleware(_nextMock.Object, _loggerMock.Object, _envMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task ExceptionMiddleware_ShouldContinue_WhenNoExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _nextMock.Setup(x => x(It.IsAny<HttpContext>()))
                    .Returns(Task.CompletedTask);

            _envMock.Setup(x => x.EnvironmentName).Returns("Development");

            var middleware = new ExceptionMiddleware(_nextMock.Object, _loggerMock.Object, _envMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Never);
        }
    }
} 