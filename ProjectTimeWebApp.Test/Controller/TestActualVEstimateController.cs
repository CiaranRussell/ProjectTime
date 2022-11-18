using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ProjectTime.Areas.SuperUser.Controllers;
using ProjectTime.Data;
using ProjectTime.Utility;
using Assert = NUnit.Framework.Assert;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestActualVEstimateController
    {
        private static readonly DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ActualVEstimateControllerTest").Options;

        ApplicationDbContext dbContext;


        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

        }

        [TestCase, Order(1)]

        public void Test_Controller_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var actualVEstimateController = new ActualVEstimateController(dbContext, mock.Object);

            // Act
            var result = actualVEstimateController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [TestCase, Order(2)]

        public void Test_Controller_IndexProjectTracker_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var actualVEstimateController = new ActualVEstimateController(dbContext, mock.Object);

            // Act
            var result = actualVEstimateController.IndexProjectTracker("1") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "IndexProjectTracker");

        }

        [TestCase, Order(3)]

        public void Test_Controller_IndexProjectTrackerCost_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var actualVEstimateController = new ActualVEstimateController(dbContext, mock.Object);

            // Act
            var result = actualVEstimateController.IndexProjectTrackerCost("1") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "IndexProjectTrackerCost");

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }
    }

}


