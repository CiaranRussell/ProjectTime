using Microsoft.AspNetCore.Mvc;
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

        // Post method with validation to prevent duplicate department names being created
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
                TempData["save"] = "Department created Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        // Get method to return Edit department page
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

            if (departmentSearch == null)
            {
                return NotFound();
            }
            return View(departmentSearch);
        }

        // Post method to edit departments with validation to prevent duplicate department names being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department obj)
        {
           
                var dupCheck = !_db.departments.Any(x => x.Id != obj.Id && x.Name.ToLower() == obj.Name.ToLower() 
                                                                             && x.Name.Trim() == obj.Name.Trim());

                if (dupCheck == false)
                {
                    ModelState.AddModelError("Name", "Depatment Name already exists");
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

        // Get method to return Delete department page
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

            if (departmentSearch == null)
            {
                return NotFound();
            }
            return View(departmentSearch);
        }

        // Post method to delete departments
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var departmentSearch = _db.departments.FirstOrDefault(x => x.Id == id);

            if (departmentSearch == null)
            {
                return NotFound();
            }

            _db.departments.Remove(departmentSearch);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Department Deleted Successfully!!";
            return RedirectToAction("Index");
            
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
