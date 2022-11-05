using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.SuperUser.Controllers
{
    [Area("SuperUser")]
    public class ReportController : Controller
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProjectEstimateController> _logger;

        public ReportController(ISessionHelper sessionHelper, ApplicationDbContext db, ILogger<ProjectEstimateController> logger)
        {
            _sessionHelper = sessionHelper;
            _db = db;
            _logger = logger;
        }
        // get method to return Time Log Report view
        public IActionResult TimeLogReport()
        {
            return View();
        }

        // Get method API to return Time logs data for Time log report to include Projectusers, Applicationusers, Department, 
        // & Project 
        #region API CALLS
        [HttpGet]
        public IActionResult TimeLogReportAPI()
        {
            var timeLogList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                               .ThenInclude(a => a.ApplicationUser)
                                                               .ThenInclude(d => d.Department)
                                                               .Include(p => p.Project);

            return Json(new { data = timeLogList });
        }
        #endregion

        // Get method to return Project Estimate Summary Report view
        public IActionResult ProjectEstimateSummaryReport()
        {
            return View();
        }

        // Get method API to return Project Estimate data for Project Estimate Summary report grouped by ProjectId
        #region API CALLS
        [HttpGet]
        public IActionResult ProjectEstimateSummaryReportAPI()
        {
            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .Include(d => d.Department);



            var projectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Include(d => d.Department)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());
                                                              

            decimal projectTotalCost = 0;

            foreach (var project in projectEstimate)
            {
                var durationSum = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x?.DurationDays);

                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                   .Sum(x => (x?.DurationDays * (decimal)7.5) * x.Department.Rate);

                var minDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateFrom).DefaultIfEmpty().Min().ToShortDateString();

                var maxDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateTo).DefaultIfEmpty().Max().ToShortDateString();


                project.DurationDays = Math.Round((decimal)durationSum, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;
                projectTotalCost += project.TotalCost = Math.Round((decimal)totalCost, 2);

            }

            return Json(new { data = projectEstimate });
        }
        #endregion

        // Get method to return Project Estimate Report view
        public IActionResult ProjectEstimateReport()
        {
            return View();
        }

        // Get method API to return Project Estimate data for Project Estimate report 
        #region API CALLS
        [HttpGet]
        public IActionResult ProjectEstimateReportAPI()
        {
            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .Include(d => d.Department);

            foreach (var projectEstimate in projectEstimateList)
            {
                var totalCost = projectEstimateList.Where(x => x.DepartmentId == projectEstimate.DepartmentId
                                                          && x.DurationDays == projectEstimate.DurationDays)
                                                   .Sum(x => (x?.DurationDays * (decimal)7.5) * x.Department.Rate);

                projectEstimate.TotalCost = Math.Round((decimal)totalCost, 2);
            }



            return Json(new { data = projectEstimateList });
        }
        #endregion
    }
}
