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

        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        
        public TimeLogController(ApplicationDbContext db, ISessionHelper sessionHelper)
        {
            _db = db;
            _sessionHelper = sessionHelper;
        }

        // Get method to return TimeLog Projects view to include ProjectUsers and Projects with where condition   
        // to return distinct projects by User Id or all projects for PMO Role  
        public IActionResult Index()
        {
            var userId = _sessionHelper.GetUserId();
            var userRole = _sessionHelper.GetUserRole();


            var timeLogList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectUser.UserId == userId || userRole == "PMO");

            var myProjects = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectUser.UserId == userId || userRole == "PMO")
                                                              .GroupBy(p => p.Project.ProjectCode)
                                                              .Select(y => y.First());
            
            foreach (var project in myProjects)
            {
                var durationSum = timeLogList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var minDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var maxDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                project.Duration = Math.Round(durationSum / (decimal)7.5, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;

            }

            return View(myProjects);
        }

        // Get method to return My Projects view page 
        public IActionResult IndexTimeLog(string id)
        {
            var userId = _sessionHelper.GetUserId();
            var userRole = _sessionHelper.GetUserRole();
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
        // on the same date & validation to prevent time logs being created in the future, with linq query to retrieve ProjectUserId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeLog obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
                                            && x.ProjectUser.UserId == userId && x.Date == obj.Date);
            var dateValidation = (obj.Date > DateTime.Now || (DateTime.Now - obj.Date).TotalDays >= SD.Prevent_After_NoDays);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on this date, Please edit to update");
                    return View(obj);
                }
                if (dateValidation == true)
                {
                    ModelState.AddModelError("", "Cannot create a Time Log in the future or greather than 40 days in the past");
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
        // on the same date & validation to prevent time logs being edited after 40 days 
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeLog(TimeLog obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
            && x.ProjectUser.UserId == userId && x.Date == obj.Date);
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on this date, Please edit to update");
                    return View(obj);
                }
                var timeLog = await _db.timeLog.FindAsync(obj.Id);
                var dateValidation = (DateTime.Now - timeLog.Date).TotalDays >= SD.Prevent_After_NoDays;
                ViewBag.timeLogDate = timeLog.Date.ToLongDateString();

                if (dateValidation == true)
                {
                    ModelState.AddModelError("", "Cannot edit a Time Log > 40 days old");
                    return View(timeLog);
                }

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

        // Get method to return Delete TiemLog  page by Id
        public IActionResult Delete(int? id)
        {
            var userId = _sessionHelper.GetUserId();

            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            if (id == null)
            {
                return NotFound($"Unable to find Time Log");
            }

            var timeLog = _db.timeLog.FirstOrDefault(x => x.Id == id);

            ViewBag.timeLogDate = timeLog.Date.ToLongDateString();

            if (timeLog == null)
            {
                return NotFound($"Unable to find Time Log");
            }
            return View(timeLog);

        }

        // Post async method to delete Time Log by Id with custom error handling to notify the user if they try to delete
        // an active Time Log & validation to prevent deleting of time logs more than 40 days old 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTimeLog(int? Id)
        {
            var userId = _sessionHelper.GetUserId();
            var timeLog = _db.timeLog.Include(a => a.ProjectUser.ApplicationUser).FirstOrDefault(x => x.Id == Id);
            var dateValidation = (DateTime.Now - timeLog.Date).TotalDays >= SD.Prevent_After_NoDays;
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewBag.timeLogDate = timeLog.Date.ToLongDateString();

            try
            {
                if (dateValidation == true)
                {
                    ModelState.AddModelError("", "Cannot delete a Time Log > 40 days old");
                    return View(timeLog);
                }

                if (timeLog == null)
                {
                    return NotFound($"Unable to load Time Log with ID");
                }

                _db.timeLog.Remove(timeLog);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Time Log Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"Error {timeLog.ProjectUser.ApplicationUser.FullName} has logged time aganist this Project";
                ViewBag.ErrorMessage = $"The Time Log cannot be deleted as the user has logged time";
                return View("Error");
            }
        }

        // Get method to return API TimeLog data for datatables to include ProjectUsers and Projects with where condition   
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
