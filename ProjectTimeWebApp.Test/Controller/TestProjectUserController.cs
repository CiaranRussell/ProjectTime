using Microsoft.EntityFrameworkCore;
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
using Microsoft.Extensions.Logging;
using ProjectTime.Utility;
using Moq;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestProjectUserController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectUserControllerTest").Options;

        ApplicationDbContext dbContext;
        ProjectUserController projectUserController;
        ILogger<ProjectUserController> logger;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectUserController = new ProjectUserController(dbContext, mock.Object, logger);
        }

        [Test, Order(1)]

        public void Test_CreateConrollerPost_WithResponse()
        {
            // Arrange
            ProjectUser projectUser = new ProjectUser()
            { 
                Id = 5, 
                ProjectId = 5, 
                UserId = "UsertestId5", 
                IsActive = true, 
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = projectUserController.Create(projectUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectUser.Id, 5);
            Assert.AreEqual(projectUser.UserId, "UsertestId5");
            Assert.AreEqual(projectUser.IsActive, true);
        }

        [Test, Order(2)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectUserController = new ProjectUserController(dbContext, mock.Object, logger);

            // Act
            var result = projectUserController.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");

        }

        [Test, Order(3)]

        public void Test_ProjectUserController_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectUserController = new ProjectUserController(dbContext, mock.Object, logger);

            // Act
            var result = projectUserController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [Test, Order(4)]

        public void Test_DeleteProjectUserGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectUserController = new ProjectUserController(dbContext, mock.Object, logger);

            // Act
            var result = projectUserController.Delete(3);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(5)]

        public void Test_DeleteProjectUserPost_WithResponse()
        {
            // Arrange
            ProjectUser projectUser = new ProjectUser()
            { 
                Id = 6, 
                ProjectId = 6, 
                UserId = "UsertestId6", 
                IsActive = true, 
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId",
                ModifiedByUserId = "UserId",
                ModifyDateTime = new System.DateTime(),
            };

            // Act
            var result = projectUserController.DeleteProjectUser(projectUser);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(6)]

        public void Test_EditProjectUserPost_WithResponse()
        {
            // Arrange
            ProjectUser projectUser = new ProjectUser()
            { 
                Id = 3, 
                ProjectId = 7, 
                UserId = "UsertestId7", 
                IsActive = true, 
                ModifyDateTime = new System.DateTime(),
                ModifiedByUserId = "UserId"
            };

            // Act
            var result = projectUserController.EditProject(projectUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(projectUser.ProjectId, 7);
            Assert.AreEqual(projectUser.UserId, "UsertestId7");

        }

        [Test, Order(7)]

        public void Test_EditProjectUserGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            projectUserController = new ProjectUserController(dbContext, mock.Object, logger);

            // Act
            var result = projectUserController.Edit(3);

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
            var projectUser = new List<ProjectUser>
            {
                new ProjectUser()
                { 
                    Id = 1, 
                    ProjectId = 1, 
                    UserId ="UsertestId1", 
                    IsActive = true, 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId",
                    ModifiedByUserId="UserId",
                    ModifyDateTime = new System.DateTime(),
                },
                new ProjectUser()
                { 
                    Id = 2, 
                    ProjectId = 2, 
                    UserId ="UsertestId2", 
                    IsActive = false, 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId",
                    ModifiedByUserId="UserId",
                    ModifyDateTime = new System.DateTime()
                },
                new ProjectUser() 
                { 
                    Id = 3,
                    ProjectId = 3, 
                    UserId ="UsertestId3", 
                    IsActive = false, 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId",
                    ModifiedByUserId="UserId",
                    ModifyDateTime = new System.DateTime()
                },
                new ProjectUser()
                { 
                    Id = 4, 
                    ProjectId = 4, 
                    UserId ="UsertestId4", 
                    IsActive = true, 
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId",
                    ModifiedByUserId="UserId",
                    ModifyDateTime = new System.DateTime()
                }
            };
            dbContext.projectUsers.AddRange(projectUser);
            dbContext.SaveChanges();


        }
    }
}
