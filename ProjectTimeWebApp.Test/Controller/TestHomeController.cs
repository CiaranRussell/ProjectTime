using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Controllers;


namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestHomeController
    {
        [TestMethod]

        public void Test_HomeControllerIndex_WithResponse()
        {
            // Create Logger to overload Ilogger in HomeController Constructor 

            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<HomeController>();

            //// Arrange
            HomeController controller = new(logger);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
