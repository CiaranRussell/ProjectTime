using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ProjectTime.Areas.Admin.Controllers;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestNonWorkingDaysController
    {
        private static readonly DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "NonWorkingDaysControllerTest").Options;

        ApplicationDbContext dbContext;
        NonWorkingDaysController nonWorkingDays;
        readonly ILogger<NonWorkingDaysController> logger;


        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");

            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);
        }

        [TestCase, Order(1)]

        public void Test_Controller_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);

            // Act
            var result = nonWorkingDays.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [Test, Order(2)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);

            //Act
            var result = nonWorkingDays.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.IsNotNull(nonWorkingDays);
        }

        [Test, Order(3)]

        public void Test_CreateControllerPost_WithResponse()
        {
            // Arrange
            NonWorkingDays NWD = new()
            {
                Id = 5,
                Date = new System.DateTime(),
                Description = "BankHoliday5",
                AllowTimeLog = false,
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = nonWorkingDays.Create(NWD);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(NWD.Id, 5);
            Assert.AreEqual(NWD.Date, new System.DateTime());
            Assert.AreEqual(NWD.Description, "BankHoliday5");

        }

        [Test, Order(4)]

        public void Test_EditNWDsPost_WithResponse()
        {
            // Arrange
            NonWorkingDays NWD = new()
            {
                Id = 5,
                Date = new System.DateTime(),
                Description = "BankHolidayTestEdit",
                AllowTimeLog = true,
                ModifyDateTime = new System.DateTime(),
                ModifiedByUserId = "UserId"
            };

            // Act
            var result = nonWorkingDays.Edit(NWD);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(NWD.Date, new System.DateTime());
            Assert.AreEqual(NWD.Description, "BankHolidayTestEdit");

        }

        [Test, Order(5)]

        public void Test_EditNWDsGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);

            // Act
            var result = nonWorkingDays.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(6)]

        public void Test_DeleteNWDsGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);

            // Act
            var result = nonWorkingDays.Delete(2) as ViewResult;

            //Arrange
            Assert.IsNotNull(result);

        }

        [Test, Order(7)]

        public void Test_DeleteNWDsPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            nonWorkingDays = new NonWorkingDaysController(dbContext, mock.Object, logger);

            // Act
            var result = nonWorkingDays.DeleteNWD(2);

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
            var nonWorkingDays = new List<NonWorkingDays>
            {
                new NonWorkingDays()
                {
                    Id = 1,
                    Date = new System.DateTime(),
                    Description = "BankHoliday1",
                    AllowTimeLog = false,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new NonWorkingDays()
                {
                    Id = 2,
                    Date = new System.DateTime(),
                    Description = "BankHoliday2",
                    AllowTimeLog = false,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new NonWorkingDays()
                {
                    Id = 3,
                    Date = new System.DateTime(),
                    Description = "BankHoliday3",
                    AllowTimeLog = false,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new NonWorkingDays()
                {
                    Id = 4,
                    Date = new System.DateTime(),
                    Description = "BankHoliday4",
                    AllowTimeLog = false,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },

            };
            dbContext.nonWorkingDays.AddRange(nonWorkingDays);
            dbContext.SaveChanges();

        }
    }
}
