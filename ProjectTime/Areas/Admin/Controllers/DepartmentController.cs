using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DepartmentController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public DepartmentController(ApplicationDbContext db, ILogger<DepartmentController> logger, ISessionHelper sessionHelper)
        {
            _db = db;
            _logger = logger;
            _sessionHelper = sessionHelper;
        }

        // Get method to return department list
        public IActionResult Index()
        {
            IEnumerable<Department> objDepartmentList = _db.departments;
                return View(objDepartmentList);
        }

        // Get method to return create department page
        public IActionResult Create()
        {
                return View();
        }

        // Post async method with validation to prevent duplicate department names being created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department obj)
        {
                var userId = _sessionHelper.GetUserId();
                var searchDepartment = _db.departments.FirstOrDefault(x => x.Name == obj.Name);
                if (searchDepartment != null)
                {
                    ModelState.AddModelError("Name", "Department Name already exists");
                }

                obj.CreatedByUserId = userId;
                 
                if (ModelState.IsValid)
                {
                    _db.departments.Add(obj);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Department Created Successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);
            
        }

        // Get method to return Edit department page
        public IActionResult Edit(int? id)
        {
                if (id == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit get department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

                if (departmentSearch == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit get department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }
                return View(departmentSearch);
        }

        // Post async method to edit departments with validation to prevent duplicate department names being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department obj)
        {
                var userId = _sessionHelper.GetUserId();
                var dupCheck = !_db.departments.Any(x => x.Id != obj.Id && x.Name.ToLower().Trim() == obj.Name.ToLower().Trim());

                if (dupCheck == false)
                {
                    ModelState.AddModelError("Name", "Department Name already exists");
                }

                var department = _db.departments.FirstOrDefault(x => x.Id == obj.Id);

                if (department == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on edit get department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                department.Name = obj.Name;
                department.Rate = obj.Rate;
                department.ModifyDateTime = DateTime.Now;
                department.ModifiedByUserId = userId;

                if (ModelState.IsValid)
                {
                    _db.departments.Update(department);
                    await _db.SaveChangesAsync();
                    TempData["edit"] = "Department Updated Successfully!!";
                    return RedirectToAction("Index");
                }
                return View(obj);

        }

        // Get method to return Delete department page by Department Id
        public IActionResult Delete(int? id)
        {
                if (id == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete get department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

                if (departmentSearch == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete get department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }
                return View(departmentSearch);
        }

        // Post async method to delete departments by department Id with custom exception handling if the user tries to delete a department
        // with related members 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);
            var userId = _sessionHelper.GetUserId();
            
            try
            {
                if (departmentSearch == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on post action delete department, null object Id {0}:", DateTime.Now);
                    return View("Error");
                }

                _db.departments.Remove(departmentSearch);
                await _db.SaveChangesAsync();
                _logger.LogWarning((EventId)102, "UserId {0} deleted {1} department object, on {2}", userId, departmentSearch.Name, DateTime.Now);
                TempData["delete"] = "Department Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                
                _logger.LogError((EventId)100, "Invalid operation by UserId {0} on {1} department object, database exception error {2}: " + ex.InnerException, userId, departmentSearch.Name, DateTime.Now);
                ViewBag.ErrorTitle = $"{departmentSearch.Name} Department is in use";
                ViewBag.ErrorMessage = $"{departmentSearch.Name} Department cannot be deleted as there are system users assigned " +
                $"to the department, please use the edit functionaility to change department name or update rates";
                return View("Error");
                
            }
              
        }

        // Get method to return Department data in API call for Data Tables 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<Department> objDepartmentList = _db.departments;
            return Json(new {data = objDepartmentList});
        }
        #endregion 
    }
}
