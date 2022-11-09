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
    public class TestReportsController
    {

        ApplicationDbContext dbContext;


        [TestCase, Order(1)]

        public void Test_Controller_TimeLogReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.TimeLogReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "TimeLogReport");

        }

        [TestCase, Order(2)]
        public void Test_Controller_ProjectEstimateSummaryReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.ProjectEstimateSummaryReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "ProjectEstimateSummaryReport");

        }

        [TestCase, Order(3)]
        public void Test_Controller_ProjectEstimateReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.ProjectEstimateReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "ProjectEstimateReport");

        }


        [TestCase, Order(4)]
        public void Test_Controller_ActualVEstimateEffortReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.ActualVEstimateEffortReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "ActualVEstimateEffortReport");

        }


        [TestCase, Order(5)]
        public void Test_Controller_ActualVEstimateVarianceReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.ActualVEstimateVarianceReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "ActualVEstimateVarianceReport");

        }

        [TestCase, Order(6)]
        public void Test_Controller_ActualVEstimateSummaryReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.ActualVEstimateSummaryReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "ActualVEstimateSummaryReport");

        }

        [TestCase, Order(7)]
        public void Test_Controller_RescourcingReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.RescourcingReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "RescourcingReport");

        }

        [TestCase, Order(8)]

        public void Test_Controller_TimeLogSummaryReport_ReturnsSuccess()
        {
            // Arrange
            var reportController = new ReportController(dbContext);

            // Act
            var result = reportController.TimeLogSummaryReport() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "TimeLogSummaryReport");

        }

    }
}