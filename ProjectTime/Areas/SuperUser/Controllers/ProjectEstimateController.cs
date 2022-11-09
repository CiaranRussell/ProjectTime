using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTime.Data;
using ProjectTime.Models.ViewModels;
using ProjectTime.Utility;
using ProjectTime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProjectTime.Areas.SuperUser
{
    [Area("SuperUser")]
    [Authorize(Roles = SD.Role_SuperUser)]
    public class ProjectEstimateController : Controller
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProjectEstimateController> _logger;

        public ProjectEstimateController(ISessionHelper sessionHelper, ApplicationDbContext db, ILogger<ProjectEstimateController> logger)
        {
            _sessionHelper = sessionHelper;
            _db = db;
            _logger = logger;
        }
        // Get method to return Project Estimates 
        public IActionResult Index()
        {
            
            return View();
        }

        // Get method to return a list of Project Estimate logs by projectId for the logged in user
        public IActionResult IndexProjectEstimate(string id)
        {
            var userId = _sessionHelper.GetUserId();
            int projectId = Int32.Parse(id);

            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .Include(d => d.Department)
                                                              .Where(u => u.ProjectId == projectId)
                                                              .ToList();

            var objProjectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(d => d.Department)
                                                              .Include(p => p.Project)
                                                              .Where(u => u.ProjectId == projectId && u.ProjectUser.UserId == userId)
                                                              .ToList();

            decimal projectTotalCost = 0;

            foreach (var projectEstimate in objProjectEstimate)
            {
                var totalCost = projectEstimateList.Where(x => x.DepartmentId == projectEstimate.DepartmentId
                                                          && x.DurationDays == projectEstimate.DurationDays)
                                                   .Sum(x => (x.DurationDays * (decimal)7.5) * x.Department.Rate);
                projectEstimate.TotalCost = Math.Round(totalCost, 2);
                projectTotalCost += totalCost;
                ViewBag.ProjectTotalCost = Math.Round(projectTotalCost,2);

            }

            return View(objProjectEstimate);
        }

        // Get method to return create ProjectEstimate view with Linq query UserHelper utility to return ProjectUser addigned projects
        public IActionResult Create()
        {

            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetSuperUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            return View();
        }

        // Post async method to create Project Estimate with validation to prevent duplication of Project Estimate being created for the same
        // Project department & validatation to prevent user from inputting durartion days that are > to/from required period,
        // with linq query utility helper to retrieve ProjectUserId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectEstimateViewModel obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(UserHelper.GetSuperUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");
            var dupCheck = !_db.projectEstimates.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
                                            && x.ProjectUser.UserId == userId && x.DepartmentId == obj.DepartmentId);
            var totalDays = (obj.DateTo - obj.DateFrom).TotalDays;
            var days = Convert.ToDecimal(totalDays);
            var dateToFrom = days > obj.DurationDays;
            
            try
            {
            
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Project Estiamte has already been logged for this Department, use edit to update.");
                    return View(obj);
                }
                if (dateToFrom == false)
                {
                    ModelState.AddModelError("", "Department from / to date is less than required duration days.");
                    return View(obj);
                }

                var projectId = obj.ProjectId;
                var ProjectUserId = UserHelper.GetProjectUserId(_db, userId, projectId);
                var projectEstimate = new ProjectEstimate()
                {

                    DurationDays = obj.DurationDays,
                    DateFrom = obj.DateFrom,
                    DateTo = obj.DateTo,
                    Description = obj.Description,
                    ProjectId = projectId,
                    DepartmentId = obj.DepartmentId,
                    ProjectUserId = ProjectUserId,
                    CreateDateTime = DateTime.Now,

                };

                if (ModelState.IsValid)
                {
                    _db.projectEstimates.Add(projectEstimate);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Project Estimate added successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

            }
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on Project Estimate create object on {1}: " + ex.InnerException, userId, DateTime.Now);
                return View("Error");
            }

        }

        // Get method to return Edit Project Estimate log with Linq query UserHelper utility to return Project Users Projects only
        public IActionResult Edit(int id)
        {
            var userId = _sessionHelper.GetUserId();

            ViewData["projectId"] = new SelectList(UserHelper.GetSuperUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            var projectEstimate = _db.projectEstimates.FirstOrDefault(x => x.Id == id);


            if (projectEstimate == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Project Estimate, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            return View(projectEstimate);
        }

        // Post async method to edit timelog with validation to prevent duplication of time logs being created for the same department
        // & validation to prevent time logs being edited after 40 days 
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProjectEstimate(ProjectEstimate obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.projectEstimates.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId
            && x.ProjectUser.UserId == userId && x.DepartmentId == obj.DepartmentId);
            ViewData["projectId"] = new SelectList(UserHelper.GetSuperUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");
            var totalDays = (obj.DateTo - obj.DateFrom).TotalDays;
            var days = Convert.ToDecimal(totalDays);
            var dateToFrom = days > obj.DurationDays;

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "Project Estimate has already been logged for this Department");
                    return View(obj);
                }
                if(dateToFrom == false)
                {
                    ModelState.AddModelError("", "Department from / to date is less than required duration days.");
                    return View(obj);
                }
                var projectEstimate = await _db.projectEstimates.FindAsync(obj.Id);
                
                if (projectEstimate == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit post Project Estimate, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }
                
                projectEstimate.ProjectId = obj.ProjectId;
                projectEstimate.DepartmentId = obj.DepartmentId;
                projectEstimate.DateFrom = obj.DateFrom;
                projectEstimate.DateTo = obj.DateTo;
                projectEstimate.DurationDays = obj.DurationDays;
                projectEstimate.Description = obj.Description;
                projectEstimate.ModifyDateTime = DateTime.Now;


                if (ModelState.IsValid)
                {
                    _db.projectEstimates.Update(projectEstimate);
                    await _db.SaveChangesAsync();
                    TempData["edit"] = "Project Estimate log updated successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

            }
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on Project Estimate edit object on {1}: " + ex.InnerException, userId, DateTime.Now);
                return View("Error");
            }

        }

        // Get method to return Delete Project Estimate page by Id
        public IActionResult Delete(int? id)
        {
            var userId = _sessionHelper.GetUserId();

            ViewData["projectId"] = new SelectList(UserHelper.GetSuperUserProjects(_db, userId).ToList(), "Id", "Name");
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project Estimate, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            var projectEstimate = _db.projectEstimates.FirstOrDefault(x => x.Id == id);

            

            if (projectEstimate == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project Estimate, null object Id {0}:", DateTime.Now);
                return View("Error");
            }
            return View(projectEstimate);

        }

        // Post async method to delete Project estimate by Id with custom error handling to notify the user if they try to delete
        // an active Project Estimate
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProjectEstimate(int? id)
        {
            var userId = _sessionHelper.GetUserId();
            var projectEstimate = _db.projectEstimates.Include(a => a.ProjectUser.ApplicationUser).FirstOrDefault(x => x.Id == id);
          
            try
            {
                if (projectEstimate == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete post Project estimate, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                _db.projectEstimates.Remove(projectEstimate);
                await _db.SaveChangesAsync();
                _logger.LogWarning((EventId)102, "UserId {0} deleted TimeLog object Id {1} on {2}", userId, projectEstimate.Id, DateTime.Now);
                TempData["delete"] = "Project Estimate Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on TimeLog object, database exception error {1}: " + ex.InnerException, userId, DateTime.Now);
                ViewBag.ErrorTitle = $"Error {projectEstimate.ProjectUser.ApplicationUser.FullName} has logged an Estimate aganist this Project";
                ViewBag.ErrorMessage = $"The Project Estimate cannot be deleted as the user has logged time";
                return View("Error");
            }
        }

        // Get method to return API Project Estimate summary data for datatables with where condition   
        // to return Project estimates for user by Project Id
        #region API CALLS
        [HttpGet]

        public IActionResult IndexApi()
        {
            var userId = _sessionHelper.GetUserId();

            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Include(d => d.Department)
                                                              .ToList();


            var myProjectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .ThenInclude(p => p.Project)
                                                              .Include(d => d.Department)
                                                              .Where(u => u.ProjectUser.UserId == userId)
                                                              .GroupBy(p => p.ProjectId)
                                                              .Select(y => y.First());

            decimal projectTotalCost = 0;

            foreach (var project in myProjectEstimate)
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

                return Json(new { data = myProjectEstimate });
        }
        #endregion

        // Get method to return API Project Estimate data for datatables with where condition   
        // to return Project estimates for user by Project Id
        #region API CALLS
        [HttpGet]

        public IActionResult IndexProjectEstimateAPI(string id)
        {
            var userId = _sessionHelper.GetUserId();
            int projectId = Int32.Parse(id);

            var projectEstimateList = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(p => p.Project)
                                                              .Include(d => d.Department)
                                                              .Where(u => u.ProjectId == projectId)
                                                              .ToList();

            var objProjectEstimate = (IEnumerable<ProjectEstimate>)_db.projectEstimates.Include(p => p.ProjectUser)
                                                              .Include(d => d.Department)
                                                              .Include(p => p.Project)
                                                              .Where(u => u.ProjectId == projectId && u.ProjectUser.UserId == userId)
                                                              .ToList();

            foreach (var projectEstimate in objProjectEstimate)
            {
                var totalCost = projectEstimateList.Where(x => x.DepartmentId == projectEstimate.DepartmentId 
                                                          && x.DurationDays == projectEstimate.DurationDays)
                                                   .Sum(x => (x?.DurationDays * (decimal)7.5) * x.Department.Rate);

                projectEstimate.TotalCost = Math.Round((decimal)totalCost, 2);
            }

            return Json(new { data = objProjectEstimate });
        }
        #endregion



    }
}
