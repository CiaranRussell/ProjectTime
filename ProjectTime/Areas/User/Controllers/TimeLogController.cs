using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Models.ViewModels;
using ProjectTime.Utility;

namespace ProjectTime.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_SuperUser + "," + SD.Role_Admin + "," + SD.Role_User)]

    public class TimeLogController : Controller
    {

        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        private readonly IEnumerable<NonWorkingDays> nonWorkingDays;
        private readonly ILogger<TimeLogController> _logger;

        public TimeLogController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<TimeLogController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            nonWorkingDays = _db.nonWorkingDays.ToList();
            _logger = logger;
        }

        // Get method to return TimeLog Projects view to include ProjectUsers and Projects with where condition   
        // to return distinct projects by User Id or all projects for PMO Role  
        public IActionResult Index()
        {
            var userId = _sessionHelper.GetUserId();


            var timeLogList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                               .ThenInclude(p => p.Project)
                                                               .Where(u => u.ProjectUser.UserId == userId)
                                                               .ToList();

            var myProjects = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectUser.UserId == userId)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());

            foreach (var project in myProjects)
            {
                var durationSum = timeLogList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var minDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var maxDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                project.Duration = Math.Round(durationSum / SD.WorkingDay, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;

            }

            return View(myProjects);
        }

        // Get method to return a list of timelogs by projectId for the logged in user or the all timelogs for the PMO user role view page 
        public IActionResult IndexTimeLog()
        {

            return View();
        }

        // Get method to return create ProjectTime log with Linq query UserHelper utility to return Project Users Active Projects only
        public IActionResult Create()
        {

            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            return View();
        }

        // Post async method to created timelog with validation to prevent duplication of time logs being created for the same project
        // on the same date & validation to prevent time logs being created in the future and on Non-Working days, with linq query
        // to retrieve ProjectUserId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeLogViewModel obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
                                            && x.ProjectUser.UserId == userId && x.Date == obj.Date);
            var dateValidation = (obj.Date > DateTime.Now || (DateTime.Now - obj.Date).TotalDays >= SD.Prevent_After_NoDays);

            try
            {
                foreach (var item in nonWorkingDays)
                {
                    if (obj.Date == item.Date && !item.AllowTimeLog)
                    {
                        ModelState.AddModelError("", "The log date is listed as a Non-Working day, Please contact the PMO's office for assistance");
                        return View(obj);
                    }
                }
                if (!dupCheck)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on the log date");
                    return View(obj);
                }
                if (dateValidation)
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
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {id} on TimeLog create object on {date}: " + ex.InnerException, userId, DateTime.Now);
                return View("Error");
            }

        }

        // Get method to return Edit ProjectTime log with Linq query UserHelper utility to return Project Users Active Projects only
        public IActionResult Edit(int id)
        {
            var userId = _sessionHelper.GetUserId();

            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            var timeLogSearch = _db.timeLog.FirstOrDefault(x => x.Id == id);

            ViewBag.timeLogDate = timeLogSearch.Date.ToLongDateString();

            if (timeLogSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get TimeLog, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            return View(timeLogSearch);
        }

        // Post async method to edit timelog with validation to prevent duplication of time logs being created for the same project
        // on the same date & validation to prevent time logs being edited after 40 days 
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeLog(TimeLog obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.timeLog.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
            && x.ProjectUser.UserId == userId && x.Date == obj.Date);
            ViewData["projectId"] = new SelectList(UserHelper.GetUserProjects(_db, userId).ToList(), "Id", "Name");

            try
            {
                if (!dupCheck)
                {
                    ModelState.AddModelError("", "Time has already been logged for this project on this date, Please edit to update");
                    return View(obj);
                }
                var timeLog = await _db.timeLog.FindAsync(obj.Id);
                var dateValidation = (DateTime.Now - timeLog.Date).TotalDays >= SD.Prevent_After_NoDays;
                ViewBag.timeLogDate = timeLog.Date.ToLongDateString();

                if (dateValidation)
                {
                    ModelState.AddModelError("", "Cannot edit a Time Log > 40 days old");
                    return View(timeLog);
                }

                if (timeLog == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit post TimeLog, null object Id {date}:", DateTime.Now);
                    return View("Error");
                }
                timeLog.Duration = obj.Duration;
                timeLog.Description = obj.Description;
                timeLog.ProjectId = obj.ProjectId;
                timeLog.ModifyDateTime = DateTime.Now;


                if (ModelState.IsValid)
                {
                    _db.timeLog.Update(timeLog);
                    await _db.SaveChangesAsync();
                    TempData["edit"] = "ProjectTime log updated successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

            }
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {id} on TimeLog edit object on {date}: " + ex.InnerException, userId, DateTime.Now);
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
                _logger.LogError((EventId)101, "Invalid operation on delete get TimeLog, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var timeLog = _db.timeLog.FirstOrDefault(x => x.Id == id);

            ViewBag.timeLogDate = timeLog.Date.ToLongDateString();

            if (timeLog == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete post TimeLog, null object Id {date}:", DateTime.Now);
                return View("Error");
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
                if (dateValidation)
                {
                    ModelState.AddModelError("", "Cannot delete a Time Log > 40 days old");
                    return View(timeLog);
                }

                if (timeLog == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete post TimeLog, null object Id {date}:", DateTime.Now);
                    return View("Error");
                }

                _db.timeLog.Remove(timeLog);
                await _db.SaveChangesAsync();
                _logger.LogWarning((EventId)102, "UserId {id} deleted TimeLog object Id {id} on {date}", userId, timeLog.Id, DateTime.Now);
                TempData["delete"] = "Time Log Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, $"Invalid operation by UserId {{id}} on TimeLog object, database exception error {{date}}: {ex.InnerException}", userId, DateTime.Now);
                ViewBag.ErrorTitle = $"Error {timeLog.ProjectUser.ApplicationUser.FullName} has logged time aganist this Project";
                ViewBag.ErrorMessage = $"The Time Log cannot be deleted as the user has logged time";
                return View("Error");
            }
        }

        // Get method to return TimeLog Projects view to include ProjectUsers and Projects with where condition   
        // to return distinct projects by User Id or all projects for PMO Role
        #region API CALLS
        [HttpGet]
        public IActionResult IndexTimeLogAPI()
        {
            var userId = _sessionHelper.GetUserId();


            var timeLogList = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                               .Include(p => p.Project)
                                                               .ThenInclude(p => p.ProjectStage)
                                                               .Where(u => u.ProjectUser.UserId == userId)
                                                               .ToList();

            var myProjects = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .ThenInclude(p => p.ProjectStage)
                                                              .Where(u => u.ProjectUser.UserId == userId)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());

            foreach (var project in myProjects)
            {
                var durationSum = timeLogList.Where(x => x.ProjectId == project.ProjectId).Sum(x => x.Duration);
                var minDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Min(x => x.Date).ToShortDateString();
                var maxDate = timeLogList.Where(x => x.ProjectId == project.ProjectId).Max(x => x.Date).ToShortDateString();
                project.Duration = Math.Round(durationSum / SD.WorkingDay, 1);
                project.MinDate = minDate;
                project.MaxDate = maxDate;

            }

            return Json(new { Data = myProjects });
        }
        #endregion

        // Get method to return API TimeLog data for datatables with where condition   
        // to return ProjectTime logs for user by Project Id or all logs for PMO Role
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI(int id)
        {
            var userId = _sessionHelper.GetUserId();
            var projectId = id;

            var objTimeLog = (IEnumerable<TimeLog>)_db.timeLog.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Where(u => u.ProjectId == projectId && u.ProjectUser.UserId == userId)
                                                              .ToList();

            return Json(new { Data = objTimeLog });
        }
        #endregion
    }
}
