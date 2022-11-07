using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using NUnit.Framework;
using ProjectTime.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;
using ProjectTime.Areas.User.Controllers;
using ProjectTime.Utility;
using System.Linq;
using System;
using Moq;
using ProjectTime.Models.ViewModels;
using Microsoft.Extensions.Logging;
using ProjectTime.Areas.SuperUser;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestProjectEstimateController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectEstimateControllerTest").Options;

        ApplicationDbContext dbContext;
        ILogger<ProjectEstimateController> logger;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();
        }

        [TestCase, Order(1)]

        public void Test_Controller_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext,  logger);

            // Act
            var result = projectEstimateController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [TestCase, Order(2)]

        public void Test_Controller_IndexProjectEstimate_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);

            // Act
            var result = projectEstimateController.IndexProjectEstimate("1") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "IndexProjectEstimate");

        }

        [Test, Order(3)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);

            //Act
            var result = projectEstimateController.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.IsNotNull(projectEstimateController);
        }

        [Test, Order(4)]

        public void Test_CreateControllerPost_WithResponseAsync()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);
            ProjectEstimateViewModel projectEstimate = new ProjectEstimateViewModel()
            {
                Id = 4,
                ProjectId = 4,
                DepartmentId = 4,
                DateFrom = new System.DateTime(),
                DateTo = new System.DateTime(),
                DurationDays = (decimal)7.5,
                Description = "",
                


            };

            // Act
            var result = projectEstimateController.Create(projectEstimate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectEstimate.Id, 4);
            Assert.AreEqual(projectEstimate.DurationDays, 7.5M);
            Assert.AreEqual(projectEstimate.Description, "");

        }


        [Test, Order(5)]

        public void Test_EditPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);
            ProjectEstimate projectEstimate = new ProjectEstimate()
            {
                Id = 4,
                ProjectId = 4,
                DepartmentId = 4,
                DateFrom = new System.DateTime(),
                DateTo = new System.DateTime(),
                DurationDays = (decimal)9,
                Description = "Test Edit",
                ModifyDateTime = new System.DateTime(),
                MinDate = "",
                MaxDate = "",
                TotalCost = (decimal)100,
                ActualDurationDays = (decimal)1,
                ActualMinDate = "",
                ActualMaxDate = "",
                ActualTotalCost = (decimal)100,
                TotalCostVariance = (decimal)100,
                DurationDaysVariance = (decimal)100,
                UnderOverBudget = "",
                UnderOverDuration = "",

            };

            // Act
            var result = projectEstimateController.EditProjectEstimate(projectEstimate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectEstimate.Id, 4);
            Assert.AreEqual(projectEstimate.DurationDays, 9M);
            Assert.AreEqual(projectEstimate.Description, "Test Edit");

        }

        [Test, Order(6)]

        public void Test_EditGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);

            // Act
            var result = projectEstimateController.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(7)]

        public void Test_DeleteGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);

            // Act
            var result = projectEstimateController.Delete(2) as ViewResult;

            //Arrange
            Assert.IsNotNull(result);

        }

        [Test, Order(8)]

        public void Test_DeletePost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var projectEstimateController = new ProjectEstimateController(mock.Object, dbContext, logger);

            // Act
            var result = projectEstimateController.DeleteProjectEstimate(1);

            // Arrange
            Assert.IsNotNull(result);

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        public void SeedDatabase()
        {
            if (!dbContext.projectEstimates.Any())
            {
                dbContext.projectEstimates.AddRange(new List<ProjectEstimate>()
                {
                    new ProjectEstimate()
                    {
                        Id = 1,
                        ProjectId = 1,
                        DepartmentId = 1,
                        DateFrom = new System.DateTime(),
                        DateTo = new System.DateTime(),
                        DurationDays = (decimal)7.5,
                        Description = "",
                        ProjectUserId = 1,
                        CreateDateTime = new System.DateTime(),
                        MinDate = "",
                        MaxDate = "",
                        TotalCost = (decimal)100,
                        ActualDurationDays = (decimal)1,
                        ActualMinDate = "",
                        ActualMaxDate = "",
                        ActualTotalCost = (decimal)100,
                        TotalCostVariance = (decimal)100,
                        DurationDaysVariance = (decimal)100,
                        UnderOverBudget = "",
                        UnderOverDuration = "",

                    },

                    new ProjectEstimate()
                    {
                        Id = 2,
                        ProjectId = 2,
                        DepartmentId = 2,
                        DateFrom = new System.DateTime(),
                        DateTo = new System.DateTime(),
                        DurationDays = (decimal)7.5,
                        Description = "",
                        ProjectUserId = 2,
                        CreateDateTime = new System.DateTime(),
                        MinDate = "",
                        MaxDate = "",
                        TotalCost = (decimal)100,
                        ActualDurationDays = (decimal)1,
                        ActualMinDate = "",
                        ActualMaxDate = "",
                        ActualTotalCost = (decimal)100,
                        TotalCostVariance = (decimal)100,
                        DurationDaysVariance = (decimal)100,
                        UnderOverBudget = "",
                        UnderOverDuration = "",
                    },

                    new ProjectEstimate()
                    {
                        Id = 3,
                        ProjectId = 3,
                        DepartmentId = 3,
                        DateFrom = new System.DateTime(),
                        DateTo = new System.DateTime(),
                        DurationDays = (decimal)7.5,
                        Description = "",
                        ProjectUserId = 3,
                        CreateDateTime = new System.DateTime(),
                        MinDate = "",
                        MaxDate = "",
                        TotalCost = (decimal)100,
                        ActualDurationDays = (decimal)1,
                        ActualMinDate = "",
                        ActualMaxDate = "",
                        ActualTotalCost = (decimal)100,
                        TotalCostVariance = (decimal)100,
                        DurationDaysVariance = (decimal)100,
                        UnderOverBudget = "",
                        UnderOverDuration = "",
                    },
                });
                dbContext.SaveChanges();

            }
        }
    }
}
