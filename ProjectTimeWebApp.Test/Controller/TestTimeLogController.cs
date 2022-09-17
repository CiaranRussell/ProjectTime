﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using ProjectTime.Controllers;
using NUnit.Framework;
using ProjectTime.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;
using ProjectTime.Areas.Admin.Controllers;
using Microsoft.VisualStudio.Services.Common;
using ProjectTime.Areas.User.Controllers;
using ProjectTime.Utility;
using System.Linq;
using System;
using Moq;
using AutoMoq;
using System.Threading.Tasks;
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
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

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


        [Test,Order(4)]

        public void Test_CreateControllerPost_WithResponseAsync()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserRole()).Returns("UserRole");
            var timeLogController = new TimeLogController(dbContext, mock.Object);
            TimeLog timeLog = new TimeLog() { Id =4, ProjectId = 4, Date = DateTime.Now, Duration = (decimal)8.5, Description = "", ProjectUserId = 4 };
            
            // Act
            var result = timeLogController.Create(timeLog);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(timeLog.Id, 4);
            Assert.AreEqual(timeLog.Duration, 8.5M);
            Assert.AreEqual(timeLog.Description, "");

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
                        Description = "",
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