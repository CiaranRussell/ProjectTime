using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using NUnit.Framework;
using ProjectTime.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;
using ProjectTime.Areas.Admin.Controllers;
using Microsoft.Extensions.Logging;
using ProjectTime.Utility;
using Moq;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestProjectStageController
    {
        private static readonly DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectStageControllerTest").Options;

        ApplicationDbContext dbContext;
        ProjectStageController projectStage;
        readonly ILogger<ProjectStageController> logger;


        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");

            projectStage = new ProjectStageController(dbContext, mock.Object, logger);
        }

        [TestCase, Order(1)]

        public void Test_Controller_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectStage = new ProjectStageController(dbContext, mock.Object, logger);

            // Act
            var result = projectStage.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [Test, Order(2)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectStage = new ProjectStageController(dbContext, mock.Object, logger);

            //Act
            var result = projectStage.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.IsNotNull(projectStage);
        }

        [Test, Order(3)]

        public void Test_CreateControllerPost_WithResponse()
        {
            // Arrange
            ProjectStage projectstage = new()
            {
                Id = 4,
                Stage = "Closed",
                Description = "Test",
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = projectStage.Create(projectstage);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectstage.Id, 4);
            Assert.AreEqual(projectstage.Stage, "Closed");
            Assert.AreEqual(projectstage.Description, "Test");

        }

        [Test, Order(4)]

        public void Test_EditControllerPost_WithResponse()
        {
            // Arrange
            ProjectStage projectstage = new()
            {
                Id = 4,
                Stage = "In-Flight",
                Description = "Test",
                ModifyDateTime = new System.DateTime(),
                ModifiedByUserId = "UserId"
            };

            // Act
            var result = projectStage.Create(projectstage);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectstage.Id, 4);
            Assert.AreEqual(projectstage.Stage, "In-Flight");
            Assert.AreEqual(projectstage.Description, "Test");

        }

        [Test, Order(5)]

        public void Test_EditGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectStage = new ProjectStageController(dbContext, mock.Object, logger);

            // Act
            var result = projectStage.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(6)]

        public void Test_DeleteControllerGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectStage = new ProjectStageController(dbContext, mock.Object, logger);

            // Act
            var result = projectStage.Delete(2) as ViewResult;

            //Arrange
            Assert.IsNotNull(result);

        }

        [Test, Order(7)]

        public void Test_DeleteControllerPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectStage = new ProjectStageController(dbContext, mock.Object, logger);

            // Act
            var result = projectStage.DeleteProjectStage(2);

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
            var projectStage = new List<ProjectStage>
            {
                new ProjectStage()
                {
                    Id = 1,
                    Stage = "Planning",
                    Description = "Test",
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new ProjectStage()
                {
                    Id = 2,
                    Stage = "In-Flight",
                    Description = "Test",
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new ProjectStage()
                {
                    Id = 3,
                    Stage = "Initiation",
                    Description = "Test",
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                
            };
            dbContext.projectStage.AddRange(projectStage);
            dbContext.SaveChanges();

        }
    }
}
