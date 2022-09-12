using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using ProjectTime.Utility;
using ProjectTime.Models.ViewModels;

namespace ProjectTime.Areas.User.Controllers
{
    [Area("User")]
    
    public class TimeLogController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        
        public TimeLogController(ApplicationDbContext db, UserManager<ApplicationUser> userManger, ISessionHelper sessionHelper)
        {
            _db = db;
            _userManager = userManger;
            _sessionHelper = sessionHelper;
        }

        // Get method to return TimeLog Projects view to include ProjectUsers and Projects with where condition   
        // to return distinct projects by User Id or all projects for PMO Role  
        public IActionResult Index()
        {
            var userId = _sessionHelper.GetUserId();
            var userRole = _sessionHelper.GetUserRole();


            var wholeList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectUser.UserId == userId || userRole == "PMO");

            var objTimeLog = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectUser.UserId == userId || userRole == "PMO")
                                                              .GroupBy(p => p.Project.ProjectCode)
                                                              .Select(y => y.First());
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;

            foreach (var project in objTimeLog)
            {
                var durationSum = wholeList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);

                if (project.ProjectId == project.ProjectUser.ProjectId)
                {
                    ViewBag.totalDuration = Math.Round(durationSum / (decimal)7.5, 1);
                }
                

                if (project.Date < minDate)
                {
                    ViewBag.minDate = project.Date.ToShortDateString();
                }
                if (project.Date > maxDate)
                {
                    ViewBag.maxDate = project.Date.ToShortDateString();
                }

            }
            return View(objTimeLog);
        }

        // Get method to return TimeLog view page 
        public IActionResult IndexTimeLog()
        {
            var userId = _sessionHelper.GetUserId();
            var userRole = _sessionHelper.GetUserRole();
            string id = Request.Path.Value.Split('/').LastOrDefault();
            int projectId = Int32.Parse(id);

            var objTimeLog = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectId == projectId && u.ProjectUser.UserId == userId || userRole == "PMO");
            return View(objTimeLog);
        }

        // Get method to return create ProjectTime log with Linq query UserHelper utility to return Project Users Active Projects only
        public IActionResult Create()
        {
            
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");
            
            return View();
        }

        // Post async method to created timelog with validation to prevent duplication of time logs being created for the same project
        // on the same date, with linq query to retrieve ProjectUserId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeLogViewModel obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
            && x.ProjectUser.UserId == userId && x.Date == obj.Date);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on this date, Please edit to update");
                    return View(obj);
                }

                var projectId = obj.ProjectId;
                var ProjectUserId = UserHelper.GetProjectUserId(_db, userId, projectId);
                var timeLog = new TimeLog()
                { 

                   Duration = obj.Duration,
                   Date = obj.Date,
                   Description = obj.Description,
                   ProjectId = projectId,
                   ProjectUserId = ProjectUserId,
                   CreateDateTime = DateTime.Now,

                };

                if (ModelState.IsValid)
                {
                    _db.timeLog.Add(timeLog);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "ProjectTime Log added successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        // Get method to return Edit ProjectTime log with Linq query UserHelper utility to return Project Users Active Projects only
        public IActionResult Edit(TimeLog obj)
        {
            var userId = _sessionHelper.GetUserId();

            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            var timeLogSearch = _db.timeLog.FirstOrDefault(x => x.Id == obj.Id);

            ViewBag.timeLogDate = timeLogSearch.Date.ToLongDateString();

            if (timeLogSearch == null)
            {
                return NotFound($"Unable to find Time Log");
            }

            return View(timeLogSearch);
        }

        // Post async method to edit timelog with validation to prevent duplication of time logs being created for the same project
        // on the same date
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeLog(TimeLog obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
            && x.ProjectUser.UserId == userId && x.Date == obj.Date);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on this date, Please edit to update");
                    return View(obj);
                }
                var timeLog = await _db.timeLog.FindAsync(obj.Id);

                if (timeLog == null)
                {
                    return NotFound($"Unable to find Time Log");
                }
                timeLog.Duration = obj.Duration;
                timeLog.Description = obj.Description;
                timeLog.ProjectId = obj.ProjectId;


                if (ModelState.IsValid)
                {
                    _db.timeLog.Update(timeLog);
                    await _db.SaveChangesAsync();
                    TempData["edit"] = "ProjectTime log updated successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        // Get method to return TimeLog view to include ProjectUsers and Projects with where condition   
        // to return ProjectTime logs by for user by Project Id or all logs for PMO Role
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI(string id)
        {
            var userId = _sessionHelper.GetUserId();
            var userRole = _sessionHelper.GetUserRole();
            int projectId = Int32.Parse(id);

            var objTimeLog = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectId == projectId && u.ProjectUser.UserId == userId || u.ProjectId == projectId && userRole == "PMO");

            return Json(new { Data = objTimeLog });
        }
        #endregion
    }
}
