using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Areas.User.Controllers;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;


namespace ProjectTime.Areas.SuperUser.Controllers
{   
    [Area("SuperUser")]
    public class ActualVEstimateController : Controller
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TimeLogController> _logger;

        public ActualVEstimateController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<TimeLogController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexProjectTracker(string id)
        {
            return View();
        }

        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                                                        .Include(p => p.Project)
                                                                                        .Include(d => d.Department);


            var timeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                            .ThenInclude(a => a.ApplicationUser)
                                                            .ThenInclude(d => d.Department)
                                                            .Include(p => p.Project);


            var actualTimeLogs = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                                  .Include(p => p.Project)
                                                                  .GroupBy(p => p.ProjectId)
                                                                  .Select(y => y.First());


            foreach (var project in actualTimeLogs)
            {
                var durationSum = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.DurationDays);
                var minDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Min(x => x.DateFrom).ToShortDateString();
                var maxDate = projectEstimateList.Where(x => x.ProjectId == project.ProjectId).Max(x => x.DateTo).ToShortDateString();
                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                   .Sum(x => (x.DurationDays * (decimal)7.5) * x.Department.Rate);
                var actualDurationSum = timeLogs.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var actualMinDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var actualMaxDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                var actualTotalCost = timeLogs.Where(x => x.ProjectId == project.ProjectId)
                                              .Sum(x => x.Duration * project.ProjectUser.ApplicationUser.Department.Rate);

                project.EstimateDurationDays = Math.Round(durationSum, 1);
                project.EstimateMinDate = minDate;
                project.EstimateMaxDate = maxDate;
                project.EstimateTotalCost = Math.Round(totalCost, 2);
                project.Duration = Math.Round(actualDurationSum / (decimal)7.5, 1);
                project.MinDate = actualMinDate;
                project.MaxDate = actualMaxDate;
                project.TotalCost = Math.Round(actualTotalCost, 2);

            }

            return Json(new { data = actualTimeLogs });
        }
        #endregion

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
                
                var totalCost = projectEstimateList.Where(x => x.ProjectId == project.ProjectId)
                                                   .Sum(x => (x.DurationDays * (decimal)7.5) * x.Department.Rate);
                var actualDurationSum = timeLogs.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var actualMinDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var actualMaxDate = timeLogs.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                var actualTotalCost = timeLogs.Where(x => x.ProjectId == project.ProjectId)
                                              .Sum(x => x.Duration * project.ProjectUser.ApplicationUser.Department.Rate);
                
                project.TotalCost = Math.Round(totalCost, 2);
                project.ActualDurationDays = Math.Round(actualDurationSum / (decimal)7.5, 1);
                project.ActualMinDate = actualMinDate;
                project.ActualMaxDate = actualMaxDate;
                project.ActualTotalCost = Math.Round(actualTotalCost, 2);

            }

            return Json(new { data = projectEstimateList });
        }
        #endregion


    }


}
