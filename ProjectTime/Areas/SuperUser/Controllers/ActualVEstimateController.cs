using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Areas.User.Controllers;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.SuperUser.Controllers
{   
    [Area("SuperUser")]
    [Authorize(Roles = SD.Role_SuperUser)]
    public class ActualVEstimateController : Controller
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        

        public ActualVEstimateController(ApplicationDbContext db, ISessionHelper sessionHelper)
        {
            _db = db;
            _sessionHelper = sessionHelper;
        }

        // Get method to return Project Tracking view
        public IActionResult Index()
        {
            return View();
        }

        // Get method to return Project Tracking Effort view by project ID
        public IActionResult IndexProjectTracker(string id)
        {
            var userId = _sessionHelper.GetUserId();
            int projectId = Int32.Parse(id);

            var projectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.Project)
                                                                                    .Include(p => p.ProjectUser)
                                                                                    .Where(x => x.ProjectId == projectId);

            foreach (var project in projectEstimate)
            {
                ViewBag.projectName = project.Project.Name;
            }

            return View(projectEstimate);
        }

        // Get method to return Project Tracking Cost view by project ID
        public IActionResult IndexProjectTrackerCost(string id)
        {
            var userId = _sessionHelper.GetUserId();
            int projectId = Int32.Parse(id);

            var projectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.Project)
                                                                                    .Where(x => x.ProjectId == projectId)
                                                                                    .Include(p => p.ProjectUser)
                                                                                    .Where(x => x.ProjectId == projectId);

            foreach (var project in projectEstimate)
            {
                ViewBag.projectName = project.Project.Name;
            }

            return View(projectEstimate);
        }

        // Get method to return API Project Tracking data for datatables with where conditions   
        // to return Project Tracking and Loop to calculate and display Actual V estimate and return project aggregated
        // summary values by project Id 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            var userId = _sessionHelper.GetUserId();

            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department);


            var timeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                            .ThenInclude(a => a.ApplicationUser)
                                                            .ThenInclude(d => d.Department)
                                                            .Include(p => p.Project);


            var actualTimeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                                  .ThenInclude(a => a.ApplicationUser)
                                                                  .ThenInclude(d => d.Department)
                                                                  .Include(p => p.Project)
                                                                  .Where(u => u.ProjectUser.UserId == userId)
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
                                                   .Sum(x => (x?.DurationDays * (decimal)7.5) * x.Department.Rate);

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
                project.Duration = Math.Round((decimal)actualDurationSum / (decimal)7.5, 1);
                project.MinDate = actualMinDate;
                project.MaxDate = actualMaxDate;
                project.TotalCost = Math.Round((decimal)actualTotalCost, 2);

            }

            return Json(new { data = actualTimeLogs });
        }
        #endregion

        // Get method to return API Project Tracking data for datatables with where conditions   
        // to return Project Tracking and Loop to calculate and display Actual timelogs V project estimate and return project aggregated
        // summary values by project Id 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexProjectTrackerAPI(string id)
        {
            int projectId = Int32.Parse(id);

            var timeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                            .ThenInclude(a => a.ApplicationUser)
                                                            .ThenInclude(d => d.Department)
                                                            .Include(p => p.Project);
                                                            

            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department)
                                                                                        .Where(u => u.ProjectId == projectId);

            foreach (var project in projectEstimateList)
            {
                
                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId && x.DepartmentId == project.DepartmentId
                                                    && x.DurationDays == project.DurationDays)
                                                   .Sum(x => (x?.DurationDays * (decimal)7.5) * x.Department.Rate);

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
                project.ActualDurationDays = Math.Round((decimal)actualDurationSum / (decimal)7.5, 1);
                project.ActualMinDate = actualMinDate;
                project.ActualMaxDate = actualMaxDate;
                project.ActualTotalCost = Math.Round((decimal)actualTotalCost, 2);
                project.DurationDaysVariance = Math.Round(project.DurationDays,1) - Math.Round((decimal)actualDurationSum / (decimal)7.5, 1);
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

    }

}
