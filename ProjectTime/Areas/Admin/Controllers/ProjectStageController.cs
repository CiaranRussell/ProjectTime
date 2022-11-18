using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProjectStageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProjectStageController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public ProjectStageController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<ProjectStageController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            _logger = logger;
        }

        // Get method to return Project Stage View 
        public IActionResult Index()
        {
            return View();
        }

        // Get method to return create Project Stage view
        public IActionResult Create()
        {
            return View();
        }

        // Post async method with validation to prevent duplicate Project Stage being created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectStage obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = _db.projectStage.FirstOrDefault(x => x.Stage == obj.Stage);
            if (dupCheck != null)
            {
                ModelState.AddModelError("", "Project Stage already exists");
            }

            obj.CreatedByUserId = userId;

            if (ModelState.IsValid)
            {
                _db.projectStage.Add(obj);
                await _db.SaveChangesAsync();
                TempData["save"] = "Project Stage Created Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get method to return Edit Project Stage view
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Project Stage, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var projectStageSearch = _db.projectStage.FirstOrDefault(x => x.Id == id);

            if (projectStageSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Project Stage, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(projectStageSearch);
        }

        // Post async method to edit Project Stage with validation to prevent duplicate stage names being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectStage obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.projectStage.Any(x => x.Id != obj.Id && x.Stage == obj.Stage);

            if (!dupCheck)
            {
                ModelState.AddModelError("Stage", "Project Stage already exists");
            }

            var projectStage = _db.projectStage.FirstOrDefault(x => x.Id == obj.Id);

            if (projectStage == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit post Project Stage, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            projectStage.Stage = obj.Stage;
            projectStage.Description = obj.Description;
            projectStage.ModifiedByUserId = userId;
            projectStage.ModifyDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.projectStage.Update(projectStage);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Project Stage Updated Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        // Get method to return Delete Project Stage view by Id
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project Stage, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var projectStageSearch = _db.projectStage.FirstOrDefault(x => x.Id == id);

            if (projectStageSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Project Stage, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(projectStageSearch);
        }

        // Post async method to delete Project Stage by Project Stage Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProjectStage(int? id)
        {
            var userId = _sessionHelper.GetUserId();
            var projectStageSearch = _db.projectStage.FirstOrDefault(x => x.Id == id);

            try
            {
                if (projectStageSearch == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete get Project Stage, null object Id {date}:", DateTime.Now);
                    return View("Error");
                }

                _db.projectStage.Remove(projectStageSearch);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Project Stage Deleted Successfully!!";
                _logger.LogWarning((EventId)102, "UserId {id} deleted Project Stage object Id {id} on {date}", userId, projectStageSearch.Id, DateTime.Now);
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, "Invalid operation by UserId {id} on Id {id} Project Stage, database exception error {date}: " + ex.InnerException, userId, projectStageSearch.Id, DateTime.Now);
                ViewBag.ErrorTitle = $"Error {projectStageSearch.Stage} is assigned to Projects";
                ViewBag.ErrorMessage = $"The Project Stage cannot be deleted as it has been assigned to Projects";
                return View("Error");
            }
        }

        // Get method to return Project Stage data in API call for Data Tables 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<ProjectStage> objProjectStageList = _db.projectStage;
            return Json(new { data = objProjectStageList });
        }
        #endregion 

    }
}
