using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.SuperUser.Controllers
{
    [Area("SuperUser")]
    [Authorize(Roles = SD.Role_SuperUser + "," + SD.Role_Admin)]
    public class ReportController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db)
        {
            _db = db;
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
                                                               .Include(p => p.Project)
                                                               .ToList();

            return Json(new { data = timeLogList });
        }
        #endregion

        // get method to return Time Log Summary Report view
        public IActionResult TimeLogSummaryReport()
        {
            return View();
        }

        // Get method API to return Time logs data for Time log Summary report to include ProjectUser, Projects, with loop to return 
        // min, max & duration values
        #region API CALLS
        [HttpGet]
        public IActionResult TimeLogSummaryReportAPI()
        {

            var timeLogList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                               .Include(p => p.Project)
                                                               .ToList();


            var projectSummary = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .ThenInclude(p => p.ProjectStage)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());

            foreach (var project in projectSummary)
            {
                var durationSum = timeLogList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var minDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var maxDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                project.Duration = Math.Round(durationSum / SD.WorkingDay, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;

            }

            return Json(new { Data = projectSummary });
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
                                                              .Include(d => d.Department)
                                                              .ToList();



            var projectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .ThenInclude(p => p.ProjectStage)
                                                              .Include(d => d.Department)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());


            decimal projectTotalCost = 0;

            foreach (var project in projectEstimate)
            {
                var durationSum = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x?.DurationDays);

                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                   .Sum(x => (x?.DurationDays * SD.WorkingDay) * x.Department.Rate);

                var minDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateFrom).DefaultIfEmpty().Min().ToShortDateString();

                var maxDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateTo).DefaultIfEmpty().Max().ToShortDateString();


                project.DurationDays = Math.Round((decimal)durationSum, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;
                decimal value = project.TotalCost = Math.Round((decimal)totalCost, 2);
                projectTotalCost += value;

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
                                                              .Include(d => d.Department)
                                                              .ToList();

            foreach (var projectEstimate in projectEstimateList)
            {
                var totalCost = projectEstimateList.Where(x => x.DepartmentId == projectEstimate.DepartmentId
                                                          && x.DurationDays == projectEstimate.DurationDays)
                                                   .Sum(x => (x?.DurationDays * SD.WorkingDay) * x.Department.Rate);

                projectEstimate.TotalCost = Math.Round((decimal)totalCost, 2);
            }



            return Json(new { data = projectEstimateList });
        }
        #endregion

        // Get method to return Actual V Estimate Effort Report view
        public IActionResult ActualVEstimateEffortReport()
        {
            return View();
        }

        // Get method to return Actual V Estimate Variance Report view
        public IActionResult ActualVEstimateVarianceReport()
        {
            return View();
        }

        // Get method to return API Project Tracking data for datatables report with where conditions   
        // to return Project Tracking and Loop to calculate and display Actual timelogs V project estimate and return project aggregated
        // summary values by Department 
        #region API CALLS
        [HttpGet]
        public IActionResult ActualVEstimateReportAPI()
        {
            var timeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                .ThenInclude(a => a.ApplicationUser)
                                                .ThenInclude(d => d.Department)
                                                .Include(p => p.Project)
                                                .ToList();


            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department)
                                                                                        .ToList();


            foreach (var project in projectEstimateList)
            {

                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId && x.DepartmentId == project.DepartmentId
                                                    && x.DurationDays == project.DurationDays)
                                                   .Sum(x => (x?.DurationDays * SD.WorkingDay) * x.Department.Rate);

                var actualDurationSum = timeLogs.Where(x => x.ProjectUser.ApplicationUser.DepartmentId == project.DepartmentId
                                                 && x.ProjectId == project.ProjectId)
                                                .Sum(x => x?.Duration);

                var actualMinDate = timeLogs.Where(x => x.ProjectId == project.ProjectId
                                             && x.ProjectUser.ApplicationUser.DepartmentId == project.DepartmentId)
                                            .Select(x => x.Date).DefaultIfEmpty().Min().ToShortDateString();

                var actualMaxDate = timeLogs.Where(x => x.ProjectId == project.ProjectId
                                             && x.ProjectUser.ApplicationUser.DepartmentId == project.DepartmentId)
                                            .Select(x => x.Date).DefaultIfEmpty().Max().ToShortDateString();

                var actualTotalCost = timeLogs.Where(x => x.ProjectId == project.ProjectId
                                               && x.ProjectUser.ApplicationUser.DepartmentId == project.DepartmentId)
                                              .Sum(x => x?.Duration * project.Department.Rate);

                project.TotalCost = Math.Round((decimal)totalCost, 2);
                project.ActualDurationDays = Math.Round((decimal)actualDurationSum / SD.WorkingDay, 1);
                project.ActualMinDate = actualMinDate;
                project.ActualMaxDate = actualMaxDate;
                project.ActualTotalCost = Math.Round((decimal)actualTotalCost, 2);
                project.DurationDaysVariance = Math.Round(project.DurationDays, 1) - Math.Round((decimal)actualDurationSum / SD.WorkingDay, 1);
                project.TotalCostVariance = Math.Round((decimal)totalCost, 2) - Math.Round((decimal)actualTotalCost, 2);

                if (project.TotalCostVariance >= 0)
                {
                    project.UnderOverBudget = SD.On_Budget;
                }
                else
                {
                    project.UnderOverBudget = SD.Over_Budget;
                }

                if (project.DurationDaysVariance >= 0)
                {
                    project.UnderOverDuration = SD.Under_Duration;
                }
                else
                {
                    project.UnderOverDuration = SD.Over_Duration;
                }


            }

            return Json(new { data = projectEstimateList });
        }
        #endregion

        // Get method to return Actual V Estimate Summary Report view
        public IActionResult ActualVEstimateSummaryReport()
        {
            return View();
        }

        // Get method to return API Project Tracking data for datatables Actual V Estiamte summary report with where conditions   
        // to return Project Tracking and Loop to calculate and display Actual V estimate and return project aggregated
        // summary values grouped by project Id
        #region API CALLS
        [HttpGet]

        public IActionResult ActualVEstimateSummaryReportAPI()
        {
            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department)
                                                                                        .ToList();


            var timeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                            .ThenInclude(a => a.ApplicationUser)
                                                            .ThenInclude(d => d.Department)
                                                            .Include(p => p.Project)
                                                            .ToList();


            var actualTimeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                                  .ThenInclude(a => a.ApplicationUser)
                                                                  .ThenInclude(d => d.Department)
                                                                  .Include(p => p.Project)
                                                                  .ThenInclude(p => p.ProjectStage)
                                                                  .GroupBy(p => p.ProjectId)
                                                                  .Select(y => y.First());


            foreach (var project in actualTimeLogs)
            {

                var durationSum = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x?.DurationDays);

                var minDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateFrom).DefaultIfEmpty().Min().ToShortDateString();

                var maxDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                 .Select(x => x.DateTo).DefaultIfEmpty().Max().ToShortDateString();

                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                   .Sum(x => (x?.DurationDays * SD.WorkingDay) * x.Department.Rate);

                var actualDurationSum = timeLogs.Where(x => x.ProjectId == project.ProjectId).Sum(x => x?.Duration);

                var actualMinDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Select(x => x.Date)
                                            .DefaultIfEmpty().Min().ToShortDateString();

                var actualMaxDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Select(x => x.Date)
                                            .DefaultIfEmpty().Max().ToShortDateString();

                var actualTotalCost = timeLogs.Where(x => x.ProjectId == project.ProjectId)
                                              .Sum(x => x?.Duration * x.ProjectUser.ApplicationUser.Department.Rate);

                project.EstimateDurationDays = Math.Round((decimal)durationSum, 1);
                project.EstimateMinDate = minDate;
                project.EstimateMaxDate = maxDate;
                project.EstimateTotalCost = Math.Round((decimal)totalCost, 2);
                project.Duration = Math.Round((decimal)actualDurationSum / SD.WorkingDay, 1);
                project.MinDate = actualMinDate;
                project.MaxDate = actualMaxDate;
                project.TotalCost = Math.Round((decimal)actualTotalCost, 2);
                project.DurationDaysVariance = Math.Round(project.EstimateDurationDays, 1) - Math.Round(project.Duration, 1);
                project.TotalCostVariance = Math.Round((decimal)totalCost, 2) - Math.Round((decimal)actualTotalCost, 2);

                if (project.TotalCostVariance >= 0)
                {
                    project.UnderOverBudget = SD.On_Budget;
                }
                else
                {
                    project.UnderOverBudget = SD.Over_Budget;
                }

                if (project.DurationDaysVariance >= 0)
                {
                    project.UnderOverDuration = SD.Under_Duration;
                }
                else
                {
                    project.UnderOverDuration = SD.Over_Duration;
                }

            }

            return Json(new { data = actualTimeLogs });
        }
        #endregion

        // Get method to return rescourcing Report view
        public IActionResult RescourcingReport()
        {
            return View();
        }

        // Get method to return API Project Estimate data for datatables rescourcing report with where conditions   
        // to filter report with only department estiamntes with dates > current date 
        #region API CALLS
        [HttpGet]

        public IActionResult RescourcingReportAPI()
        {
            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department)
                                                                                        .Where(x => x.DateTo > DateTime.Now)
                                                                                        .ToList();
            return Json(new { data = projectEstimateList });
        }
        #endregion
    }
}
