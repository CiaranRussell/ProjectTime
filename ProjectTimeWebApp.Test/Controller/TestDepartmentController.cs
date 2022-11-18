using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ProjectTime.Controllers;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace ProjectTimeWebApp.Test.Controller
{
    [TestClass]
    public class TestDepartmentController
    {
        private static readonly DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DepartmentControllerTest").Options;

        ApplicationDbContext dbContext;
        readonly ILogger<DepartmentController> logger;
        DepartmentController departmentController;

        [OneTimeSetUp]

        public void Setup()
        {
            dbContext = new ApplicationDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();

            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            departmentController = new DepartmentController(dbContext, logger, mock.Object);
        }

        [Test, Order(1)]

        public void Test_CreateDepartmentPost_WithResponse()
        {
            // Arrange
            Department department = new()
            {
                Id = 6,
                Name = "TestDepartment6",
                Rate = 23.75M,
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = departmentController.Create(department);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(department.Id, 6);
            Assert.AreEqual(department.Rate, 23.75M);
        }

        [Test, Order(2)]

        public void Test_CreateControllerGet_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            DepartmentController department = new(dbContext, logger, mock.Object);

            // Act
            var result = department.Create() as ViewResult;

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");

        }

        [Test, Order(3)]

        public void Test_DepartmentController_Index_ReturnsSuccess()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            DepartmentController department = new(dbContext, logger, mock.Object);

            // Act
            var result = department.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

        }

        [Test, Order(4)]

        public void Test_DeleteDepartmentGet_WithResponse()
        {

            // Act
            var result = departmentController.Delete(1);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(5)]

        public void Test_DeleteDepartmentPost_WithResponse()
        {
            // Arrange
            _ = new Department()
            {
                Id = 7,
                Name = "TestDepartment10",
                Rate = 23.75M,
                CreateDateTime = new System.DateTime(),
                CreatedByUserId = "UserId"
            };

            // Act
            var result = departmentController.DeleteDepartment(7);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test, Order(6)]

        public void Test_EditDepartmentPost_WithResponse()
        {
            // Arrange
            Department department = new()
            {
                Id = 6,
                Name = "TestDepartment7",
                Rate = 23.75M,
                ModifyDateTime = new System.DateTime(),
                ModifiedByUserId = "UserId"
            };

            // Act
            var result = departmentController.Edit(department);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(department.Name, "TestDepartment7");

        }

        [Test, Order(7)]

        public void Test_EditDepartmentGet_WithResponse()
        {
            // Arrange
            var mock = new Mock<ISessionHelper>();
            mock.Setup(p => p.GetUserId()).Returns("UserId");
            

            // Act
            var result = departmentController.Edit(6);

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
            var department = new List<Department>
            {
                new Department()
                {
                    Id = 1,
                    Name ="TestDepartment1",
                    Rate = 25.50M,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Department()
                {
                    Id = 2,
                    Name ="TestDepartment2",
                    Rate = 33.98M,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Department()
                {
                    Id = 3,
                    Name ="TestDepartment3",
                    Rate = 28.50M,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Department()
                {
                    Id = 4,
                    Name ="TestDepartment4",
                    Rate = 39.98M,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId"
                },
                new Department()
                {
                    Id = 5,
                    Name ="TestDepartment5",
                    Rate = 56.98M,
                    CreateDateTime = new System.DateTime(),
                    CreatedByUserId = "UserId" }

            };
            dbContext.departments.AddRange(department);
            dbContext.SaveChanges();

        }



    }


}
