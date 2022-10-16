using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using ProjectTime.Controllers;
using NUnit.Framework;
using ProjectTime.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;
using Microsoft.Extensions.Logging;
using ProjectTime.Utility;
using Moq;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestProjectController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectControllerTest").Options;

        ApplicationDbContext dbContext;
        ProjectController projectController;
        ILogger<ProjectController> logger;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);
        }

        [Test, Order(1)]

        public void Test_CreateConrollerPost_WithResponse()
        {
            // Arrange
            Project project = new Project()
            { 
                Id = 5, 
                Name = "TestProject5", 
                ProjectCode = "UTM005", 
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = projectController.Create(project);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(project.Id, 5);
            Assert.AreEqual(project.ProjectCode, "UTM005");
        }

        [Test, Order(2)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);

            // Act
            var result = projectController.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");

        }

        [Test, Order(3)]

        public void Test_ProjectController_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);

            // Act
            var result = projectController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [Test, Order(4)]

        public void Test_DeleteProjectGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);

            // Act
            var result = projectController.Delete(3);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(5)]

        public void Test_DeleteProjectPost_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);

            // Act
            var result = projectController.DeleteProject(1);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(6)]

        public void Test_EditProjectPost_WithResponse()
        {
            // Arrange
            Project project = new Project()
            { 
                Id = 1, 
                Name = "TestProject7", 
                ProjectCode = "UTM007", 
                ModifyDateTime = new System.DateTime(),
                ModifiedByUserId = "UserId"
            };

            // Act
            var result = projectController.Edit(project);


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(project.Name, "TestProject7");
            Assert.AreEqual(project.ProjectCode, "UTM007");

        }

        [Test, Order(7)]

        public void Test_EditProjectGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectController = new ProjectController(dbContext, mock.Object, logger);

            // Act
            var result = projectController.Edit(3);


            // Assert
            Assert.IsNotNull(result);

        }

        [OneTimeTearDown]

        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        public void SeedDatabase()
        {
            var project = new List<Project>
            {
                new Project()
                { 
                    Id = 1, 
                    Name ="TestProject1", 
                    ProjectCode = "UTM001", 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Project()
                { 
                    Id = 2, 
                    Name ="TestProject2", 
                    ProjectCode = "UTM002", 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Project()
                { 
                    Id = 3, 
                    Name ="TestProject3", 
                    ProjectCode = "UTM003", 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Project()
                { 
                    Id = 4, 
                    Name ="TestProject4", 
                    ProjectCode = "UTM004", 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },


            };
            dbContext.projects.AddRange(project);
            dbContext.SaveChanges();

        }
    }
}
