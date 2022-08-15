using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;


namespace ProjectTime.Controllers
{
    [Area("Admin")]

    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
                _db = db;
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
                var searchDepartment = _db.departments.FirstOrDefault(x => x.Name == obj.Name);
                if (searchDepartment != null)
                {
                    ModelState.AddModelError("Name", "Department Name already exists");
                }

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
                    return NotFound($"Unable to find Department");
                }

                var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

                if (departmentSearch == null)
                {
                    return NotFound($"Unable to find Department");
                }
                return View(departmentSearch);
        }

        // Post async method to edit departments with validation to prevent duplicate department names being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department obj)
        {
           
                var dupCheck = !_db.departments.Any(x => x.Id != obj.Id && x.Name.ToLower().Trim() == obj.Name.ToLower().Trim());

                if (dupCheck == false)
                {
                    ModelState.AddModelError("Name", "Department Name already exists");
                }

                if (ModelState.IsValid)
                {
                    _db.departments.Update(obj);
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
                    return NotFound($"Unable to find Department");
                }

                var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

                if (departmentSearch == null)
                {
                    return NotFound($"Unable to find Department");
                }
                return View(departmentSearch);
        }

        // Post async method to delete departments by department Id with custom exception handling if the user tries to delete a department
        // with related members 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id, Department department)
        {
            var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

            try
            {
                if (departmentSearch == null)
                {
                    return NotFound($"Unable to find Department");
                }

                _db.departments.Remove(departmentSearch);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Department Deleted Successfully!!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {

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
