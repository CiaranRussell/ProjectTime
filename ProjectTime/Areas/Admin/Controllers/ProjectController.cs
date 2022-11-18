using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProjectController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public ProjectController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<ProjectController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            _logger = logger;

        }
        public IActionResult Index()
        {
            IEnumerable<Project> objProjectList = _db.projects.Include(p => p.ProjectStage).ToList();
            return View(objProjectList);
        }

        // Get method to return create project view to include FK relationship for Project Stage
        public IActionResult Create()
        {
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");

            return View();
        }

        // Post async method with validation to prevent duplicate Projects been added to include FK relationship for Project Stage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");
            var searchProjectName = _db.projects.FirstOrDefault(x => x.Name == obj.Name);
            var searchProjectCode = _db.projects.FirstOrDefault(x => x.ProjectCode == obj.ProjectCode);

            if (searchProjectName != null)
            {
                ModelState.AddModelError("Name", "Project Name already exists");
            }

            if (searchProjectCode != null)
            {
                ModelState.AddModelError("ProjectCode", "Project Code already exists");
            }

            obj.CreatedByUserId = userId;

            if (ModelState.IsValid)
            {
                _db.projects.Add(obj);
                await _db.SaveChangesAsync();
                TempData["save"] = "Project created Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        // Get method to return Edit Projects page
        public IActionResult Edit(int? id)
        {
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");
            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Project, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var projectSearch = _db.projects.FirstOrDefault(x => x.Id == id);

            if (projectSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Project, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(projectSearch);
        }

        // Post method to edit Projects with validation to prevent duplicate project name or project codes being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project obj)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");
            var dupCheckProjectCode = !_db.projects.Any(x => x.Id != obj.Id && x.ProjectCode.ToLower().Trim() == obj.ProjectCode.ToLower().Trim());
            var dupCheckName = !_db.projects.Any(x => x.Id != obj.Id && x.Name.ToLower().Trim() == obj.Name.ToLower().Trim());

            if (!dupCheckProjectCode)
            {
                ModelState.AddModelError("ProjectCode", "Project Code already exists");
            }

            if (!dupCheckName)
            {
                ModelState.AddModelError("Name", "Project Name already exists");
            }

            var project = _db.projects.FirstOrDefault(x => x.Id == obj.Id);

            if (project == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit post Project, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            project.Name = obj.Name;
            project.ProjectCode = obj.ProjectCode;
            project.Description = obj.Description;
            project.ModifyDateTime = DateTime.Now;
            project.ModifiedByUserId = userId;
            project.ProjectStageId = obj.ProjectStageId;

            if (ModelState.IsValid)
            {
                _db.projects.Update(project);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Department Updated Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        // Get method to return Delete Projects page
        public IActionResult Delete(int? id)
        {
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");
            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var projectSearch = _db.projects.FirstOrDefault(x => x.Id == id);

            if (projectSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(projectSearch);
        }

        // Post async method to delete Project by user Id with custom exception handling if the user tries to delete a project
        // with related members 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int? id)
        {
            var userId = _sessionHelper.GetUserId();
            ViewData["projectStageId"] = new SelectList(_db.projectStage.ToList(), "Id", "Stage");
            var projectSearch = _db.projects.FirstOrDefault(x => x.Id == id);

            try
            {
                if (projectSearch == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete post Project, null object Id {date}:", DateTime.Now);
                    return View("Error");
                }

                _db.projects.Remove(projectSearch);
                await _db.SaveChangesAsync();
                _logger.LogWarning((EventId)102, "UserId {id} deleted Project object: {name} on {date}", userId, projectSearch.Name, DateTime.Now);
                TempData["delete"] = "Project Deleted Successfully!!";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {id} on {name} project object, database exception error {date}: " + ex.InnerException, userId, projectSearch.Name, DateTime.Now);
                ViewBag.ErrorTitle = $"{projectSearch.Name} Project is in use";
                ViewBag.ErrorMessage = $"{projectSearch.Name} Project cannot be deleted as there are system users assigned " +
                $"to the Project";
                return View("Error");

            }

        }

        // Get method to return Projects data in API call for Data Tables to include Project Stage 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<Project> objProjectList = _db.projects.Include(p => p.ProjectStage).ToList();
            return Json(new { data = objProjectList });
        }
        #endregion
    }

}
