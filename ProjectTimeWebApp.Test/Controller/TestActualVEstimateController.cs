using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;
using ProjectTime.Areas.SuperUser.Controllers;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestActualVEstimateController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
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
            var actualVEstimateController = new ActualVEstimateController(dbContext);

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
            var actualVEstimateController = new ActualVEstimateController(dbContext);

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
            var actualVEstimateController = new ActualVEstimateController(dbContext);

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


