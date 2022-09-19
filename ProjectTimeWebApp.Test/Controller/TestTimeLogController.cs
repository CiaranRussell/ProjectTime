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

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestTimeLogController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TimeLogControllerTest").Options;

        ApplicationDbContext dbContext;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();
        }

        [TestCase,Order(1)]
        
        public void Test_Controller_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            // Act
            var result = timeLogController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [TestCase, Order(2)]

        public void Test_Controller_IndexTimeLog_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            // Act
            var result = timeLogController.IndexTimeLog("1") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "IndexTimeLog");

        }

        [Test,Order(3)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            //Act
            var result = timeLogController.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.IsNotNull(timeLogController);
        }


        [Test, Order(4)]

        public void Test_CreateControllerPost_WithResponseAsync()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);
            TimeLogViewModel timeLog = new TimeLogViewModel() { Id = 4, ProjectId = 4, Date = DateTime.Now, Duration = (decimal)8.5, Description = "",};

            // Act
            var result = timeLogController.Create(timeLog);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(timeLog.Id, 4);
            Assert.AreEqual(timeLog.Duration, 8.5M);
            Assert.AreEqual(timeLog.Description, "");

        }

        [Test, Order(5)]

        public void Test_EditTimeLogPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);
            TimeLog timeLog = new TimeLog() { Id = 4, ProjectId = 4, Date = DateTime.Now, Duration = (decimal)6.5, Description = "TestEdit", ProjectUserId = 4 };

            // Act
            var result = timeLogController.EditTimeLog(timeLog);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(timeLog.Duration, 6.5M);
            Assert.AreEqual(timeLog.Description, "TestEdit");

        }

        [Test, Order(6)]

        public void Test_EditTimeLogtGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            // Act
            var result = timeLogController.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(7)]

        public void Test_DeleteTimeLogtGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            // Act
            var result = timeLogController.Delete(2) as ViewResult;

            //Arrange
            Assert.IsNotNull(result);

        }

        [Test,Order(8)]

        public void Test_DeleteTimeLogPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            var timeLogController = new TimeLogController(dbContext, mock.Object);

            // Act
            var result = timeLogController.DeleteTimeLog(2);

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
            if (!dbContext.timeLog.Any())
            {
                dbContext.timeLog.AddRange(new List<TimeLog>()
                {
                    new TimeLog()
                    {
                        Id = 1,
                        ProjectId = 1,
                        Date = new System.DateTime(),
                        Duration = (decimal)7.5,
                        Description = "",
                        ProjectUserId = 1,
                        CreateDateTime = new System.DateTime(),
                    },

                    new TimeLog()
                    {
                        Id = 2,
                        ProjectId = 2,
                        Date = new System.DateTime(),
                        Duration = (decimal)10.5,
                        Description = "Test",
                        ProjectUserId = 2,
                        CreateDateTime = new System.DateTime(),
                    },

                    new TimeLog()
                    {
                        Id = 3,
                        ProjectId = 3,
                        Date = new System.DateTime(),
                        Duration = (decimal)6.5,
                        Description = "",
                        ProjectUserId = 3,
                        CreateDateTime = new System.DateTime(),
                    },
                });
                dbContext.SaveChanges();

            }
        }
    }
}
