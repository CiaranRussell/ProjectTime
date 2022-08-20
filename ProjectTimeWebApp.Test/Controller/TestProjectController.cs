using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTime.Data;
using ProjectTime.Controllers;
using NUnit.Framework;
using ProjectTime.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Assert = NUnit.Framework.Assert;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestProjectController
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectControllerTest").Options;

        ApplicationDbContext dbContext;
        ProjectController projectController;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            projectController = new ProjectController(dbContext);
        }

        [Test, Order(1)]

        public void Test_CreateConrollerPost_WithResponse()
        {
            // Arrange
            Project project = new Project() { Id = 5, Name = "TestProject5", ProjectCode = "UTM005", CreateDateTime = new System.DateTime() };

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
            ProjectController project = new ProjectController(dbContext);

            // Act
            var result = projectController.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");

        }

        [Test, Order(3)]

        public void Test_ProjectController_Index_ReturnsSuccess()
        {
            // Arrange
            ProjectController project = new ProjectController(dbContext);

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

            // Act
            var result = projectController.Delete(3);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(5)]

        public void Test_DeleteProjectPost_WithResponse()
        {
            // Arrange
            Project project = new Project() { Id = 6, Name = "TestProject6", ProjectCode = "UTM006", CreateDateTime = new System.DateTime() };

            // Act
            var result = projectController.DeleteConfirm(6);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(6)]

        public void Test_EditProjectPost_WithResponse()
        {
            // Arrange
            Project project = new Project() { Id = 1, Name = "TestProject7", ProjectCode = "UTM007", CreateDateTime = new System.DateTime() };

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
            

            // Act
            var result = projectController.Edit(3);


            // Assert
            Assert.IsNotNull(result);
            


        }

        [OneTimeTearDown]

        public void CeanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        public void SeedDatabase()
        {
            var project = new List<Project>
            {
                new Project() { Id = 1, Name ="TestProject1", ProjectCode = "UTM001", CreateDateTime = new System.DateTime() },
                new Project() { Id = 2, Name ="TestProject2", ProjectCode = "UTM002", CreateDateTime = new System.DateTime() },
                new Project() { Id = 3, Name ="TestProject3", ProjectCode = "UTM003", CreateDateTime = new System.DateTime() },
                new Project() { Id = 4, Name ="TestProject4", ProjectCode = "UTM004", CreateDateTime = new System.DateTime() },


            };
            dbContext.projects.AddRange(project);
            dbContext.SaveChanges();

        }
    }
}
