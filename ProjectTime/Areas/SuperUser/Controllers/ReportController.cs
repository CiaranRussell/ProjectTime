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
        public IActionResult TimeLogReport()
        {
            return View();
        }

        // Get method API to return Time logs data for Time log report
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
    }
}
