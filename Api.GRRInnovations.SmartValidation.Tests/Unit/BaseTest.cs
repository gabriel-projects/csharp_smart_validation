using Xunit;

namespace Api.GRRInnovations.SmartValidation.Tests.Unit
{
    public abstract class BaseTest
    {
        protected BaseTest()
        {
            // Common setup for all tests
        }

        protected T CreateInstance<T>() where T : class
        {
            return Activator.CreateInstance<T>();
        }

        protected void AssertException<T>(Action action) where T : Exception
        {
            var exception = Assert.Throws<T>(action);
            Assert.NotNull(exception);
        }
    }
} 