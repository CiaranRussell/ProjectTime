using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using System.Collections.Generic;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectUserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectUserController(ApplicationDbContext db)
        {
            _db = db;
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
            var dupCheck = !_db.projectUsers.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId && x.UserId == obj.UserId);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "User is already assigned to this project");
                }

                if (ModelState.IsValid)
                {
                    _db.projectUsers.Add(obj);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Project assigned to user successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // Get method to return Edit view page by user Id to include Projects & Application Users
        public IActionResult Edit(int? id)
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");

            if (id == null)
            {
                return NotFound($"Unable to find Project User");
            }

            var user = _db.projectUsers.Include(p => p.Project).Include(a => a.ApplicationUser).FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound($"Unable to find Project User");
            }
            return View(user);
        }

        // Post async method to update Project users with validation to prevent duplication of users added to projects
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(ProjectUser obj)
        {
            var dupCheck = !_db.projectUsers.Any(x => x.Id != obj.Id && x.ProjectId == obj.ProjectId && x.UserId == obj.UserId);

            try
            {
                if (dupCheck == false)
                {
                    ModelState.AddModelError("", "User is already assigned to this project");
                    return View(obj);
                }

                var projectUser = await _db.projectUsers.FindAsync(obj.Id);

                if (projectUser == null)
                {
                    return NotFound("Unable to find Project User");
                }
                else
                {
                    projectUser.Id = obj.Id;
                    projectUser.ProjectId = obj.ProjectId;
                    projectUser.UserId = obj.UserId;
                    projectUser.IsActive = obj.IsActive;
                }

                if (ModelState.IsValid)
                {
                    _db.projectUsers.Update(projectUser);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Project User updated successfully!!";
                    return RedirectToAction("Index");
                }
                return View(projectUser);
            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        // Get method to return Delete project User page by Id
        public IActionResult Delete(int? id)
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["appuserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");

            if (id == null)
            {
                return NotFound($"Unable to find Project User");
            }

            var projectUser = _db.projectUsers.FirstOrDefault(x => x.Id == id);

            if (projectUser == null)
            {
                return NotFound($"Unable to find Project User");
            }
            return View(projectUser);
            
            
        }

        // Post async method to delete Project user by Id with custom error handling to notify the user if they try to delete
        // an active project user
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProjectUser(ProjectUser projectUser)
        {
            var projectusers = _db.projectUsers.FirstOrDefault(x => x.Id == projectUser.Id);

            try
            {
                if (projectusers == null)
                {
                    return NotFound($"Unable to load Porject User with ID");
                }

                _db.projectUsers.Remove(projectusers);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Project User Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"{projectusers.ApplicationUser.FullName} is Assigned to Projects";
                ViewBag.ErrorMessage = $"{projectusers.ApplicationUser.FullName} cannot be deleted as this user is assigned to projects";
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
