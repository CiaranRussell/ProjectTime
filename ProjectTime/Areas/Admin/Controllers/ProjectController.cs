using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;


namespace ProjectTime.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Project> objProjectList = _db.projects;
            return View(objProjectList);
        }

        // Get method to return create project page
        public IActionResult Create()
        {
            return View();
        }

        // Post method with validation to prevent duplicate Projects been added
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project obj)
        {
            var searchDepartmentName = _db.projects.FirstOrDefault(x => x.Name == obj.Name);

            var searchDepartmentProjectCode = _db.projects.FirstOrDefault(x => x.ProjectCode == obj.ProjectCode);

                if (searchDepartmentName != null)
                {
                    ModelState.AddModelError("Name", "Project Name already exists");
                }
            
                if (searchDepartmentProjectCode != null)
                {
                    ModelState.AddModelError("ProjectCode", "Project Code already exists");
                }

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
                if (id == null)
                {
                    return NotFound("Unable to find Project");
                }

                var departmentSearch = _db.projects.FirstOrDefault(x => x.Id == id);

                if (departmentSearch == null)
                {
                    return NotFound();
                }
                return View(departmentSearch);
        }

        // Post method to edit Projects with validation to prevent duplicate project name or project codes being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project obj)
        {
            var dupCheckProjectCode = !_db.projects.Any(x => x.Id != obj.Id && x.ProjectCode.ToLower().Trim() == obj.ProjectCode.ToLower().Trim());

            var dupCheckName = !_db.projects.Any(x => x.Id != obj.Id && x.Name.ToLower().Trim() == obj.Name.ToLower().Trim());

                if (dupCheckProjectCode == false)
                {
                    ModelState.AddModelError("ProjectCode", "Project Code already exists");
                }

                if (dupCheckName == false)
                {
                    ModelState.AddModelError("Name", "Project Name already exists");
                }

                if (ModelState.IsValid)  
                {
                    _db.projects.Update(obj);
                    await _db.SaveChangesAsync();
                    TempData["edit"] = "Department Updated Successfully!!";
                    return RedirectToAction("Index");
                }   
                return View(obj);

        }

        // Get method to return Delete Projects page
        public IActionResult Delete(int? id)
        {
                if (id == null)
                {
                    return NotFound("Unable to find Project");
                }

                var projectSearch = _db.projects.FirstOrDefault(x => x.Id == id);

                if (projectSearch == null)
                {
                    return NotFound("Unable to find Project");
                }
                return View(projectSearch);
        }

        // Post async method to delete Project by user Id with custom exception handling if the user tries to delete a project
        // with related members 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var projectSearch = _db.projects.FirstOrDefault(x => x.Id == id);

            try
            {
                if (projectSearch == null)
                {
                    return NotFound("Unable to find Project");
                }

                _db.projects.Remove(projectSearch);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Project Deleted Successfully!!";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"{projectSearch.Name} Project is in use";
                ViewBag.ErrorMessage = $"{projectSearch.Name} Project cannot be deleted as there are system users assigned " +
                $"in the Project";
                return View("Error");

            }

        }

        // Get method to return Projects data in API call for Data Tables 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
                IEnumerable<Project> objProjectList = _db.projects;
                return Json(new { data = objProjectList });
        }
        #endregion
    }

}
