using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ProjectTime.Controllers;
using Assert = NUnit.Framework.Assert;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestHomeController
    {
        [Test, Order(1)]

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

        [Test, Order(2)]
        public void Test_PrivacyControllerIndex_WithResponse()
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
            ViewResult result = controller.Privacy() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
