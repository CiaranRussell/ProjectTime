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

        // get method to return projectUser list view
        public IActionResult Index()
        {
            IEnumerable<ProjectUser> objProjectuserList = _db.projectUsers.Include(p => p.Project).Include(u => u.ApplicationUser).ToList();
            return View(objProjectuserList);
        }

        // Get method to return create view using viewdata to retrieve project and user names in a list

        public IActionResult Create()
        {
            ViewData["projectId"] = new SelectList(_db.projects.ToList(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_db.applicationUsers.ToList(), "Id", "FullName");

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
                    ModelState.AddModelError("FullName", "is already assigned to this project");
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



        // get method to return projectUser data in API for Datatabels
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<ProjectUser> objProjectuserList = _db.projectUsers.Include(p => p.Project)
                                                          .Include(u => u.ApplicationUser).ToList();

            return Json(new {Data = objProjectuserList});
        }
        #endregion



    }
}
