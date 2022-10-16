using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;
using System.Collections.Generic;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectUserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProjectUserController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public ProjectUserController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<ProjectUserController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            _logger = logger;
        }

        // Get method to return projectUser list view to include Projects, users, department, and roles 
        public IActionResult Index()
        {
            IEnumerable<ProjectUser> objProjectUserList = _db.projectUsers.Include(p => p.Project).Include(u => u.ApplicationUser)
                                                                                          .ThenInclude(d => d.Department).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objProjectUserList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.UserId).RoleId;
                user.ApplicationUser.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return View(objProjectUserList);
        }

        // Get method to return create view using viewdata to retrieve project and user names in a list
        public IActionResult Create()
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");
            

            return View();
        }

        // Post asyn method to created project user assignment with validation to prevent duplication of users been assigned to projects 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProjectUser obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");
            var dupCheck = !_db.projectUsers.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId && x.UserId == obj.UserId);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "User is already assigned to this project");
                }

                obj.CreatedByUserId = userId;

                if (ModelState.IsValid)
                {
                    _db.projectUsers.Add(obj);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Project assigned to user successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on ProjectUser create object on {1}: " + ex.InnerException, userId, DateTime.Now);
                return View("Error");
            }
        }

        // Get method to return Edit view page by user Id with logging to catch null Id error's
        public IActionResult Edit(int? id)
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");

            if (id == null)
            {
                _logger.LogError((EventId)101,"Invalid operation on edit get ProjectUser, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            var user = _db.projectUsers.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                _logger.LogError((EventId)101,"Invalid operation on edit get ProjectUser, null object Id {0}:", DateTime.Now);
                return View("Error");
            }
            return View(user);
        }

        // Post async method to update Project users with validation to prevent duplication of Projectusers & logcheck validation to prevent
        // editing on ProjectUser that have created Time log's with logging to catch null Id error's
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(ProjectUser obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");
            var dupCheck = !_db.projectUsers.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId && x.UserId == obj.UserId);
            var logCheck = _db.timeLog.Any(x => x.ProjectUserId == obj.Id);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "User is already assigned to this project");
                    return View(obj);
                }
                if (logCheck == true)
                {
                    ModelState.AddModelError("", "Unable to edit, The Project User has created time logs");
                    return View(obj);
                }

                var projectUser = await _db.projectUsers.FindAsync(obj.Id);

                if (projectUser == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit post ProjectUser, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                projectUser.ProjectId = obj.ProjectId;
                projectUser.UserId = obj.UserId;
                projectUser.IsActive = obj.IsActive;
                projectUser.ModifiedByUserId = userId;
                projectUser.ModifyDateTime = DateTime.Now;
                
                if (ModelState.IsValid)
                {
                    _db.projectUsers.Update(projectUser);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Project User updated successfully!!";
                    return RedirectToAction("Index");
                }
                return View(projectUser);
            }
            catch (Exception ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on ProjectUser edit object on {1}: " + ex.InnerException, userId, DateTime.Now);
                return View("Error");
            }

        }

        // Get method to return Delete project User view by Id with logging to catch null Id error's
        public IActionResult Delete(int? id)
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");

            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get ProjectUser, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            var projectUser = _db.projectUsers.FirstOrDefault(x => x.Id == id);

            if (projectUser == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get ProjectUser, null object Id {0}:", DateTime.Now);
                return View("Error");
            }
            return View(projectUser);
            
        }

        // Post async method to delete Project user by Id with custom error handling to notify the user if they try to delete
        // an active project user 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProjectUser(int? id)
        {
            var userId = _sessionHelper.GetUserId();
            var projectusers = _db.projectUsers.Include(a => a.ApplicationUser).FirstOrDefault(x => x.Id == id);

            try
            {
                if (projectusers == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete post ProjectUser, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                _db.projectUsers.Remove(projectusers);
                await _db.SaveChangesAsync();
                _logger.LogWarning((EventId)102, "UserId {0} deleted ProjectUser object: {1} on {2}", userId, projectusers.Id, DateTime.Now);
                TempData["delete"] = "Project User Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on Id {1} projectUser object, database exception error {2}: " + ex.InnerException, userId, projectusers.Id, DateTime.Now);
                ViewBag.ErrorTitle = $"Error {projectusers.ApplicationUser.FullName} has logged time aganist this Project";
                ViewBag.ErrorMessage = $"The Project User cannot be deleted as the user has logged time against this project";
                return View("Error");
            }
        }


        // Get method to return projectUser data in API Call for Data tabels to include Projects, users, department, and roles  
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<ProjectUser> objProjectUserList = _db.projectUsers.Include(p => p.Project).Include(u => u.ApplicationUser)
                                                                                          .ThenInclude(d => d.Department).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objProjectUserList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.UserId).RoleId;
                user.ApplicationUser.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new {Data = objProjectUserList});
        }
        #endregion



    }
}
