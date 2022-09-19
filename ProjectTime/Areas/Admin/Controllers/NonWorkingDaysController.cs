using Microsoft.AspNetCore.Mvc;
using ProjectTime.Data;
using ProjectTime.Models;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NonWorkingDaysController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NonWorkingDaysController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get method to return Non-Working Days list
        public IActionResult Index()
        {
            IEnumerable<NonWorkingDays> objNonWorkingDaysList = _db.nonWorkingDays;
            return View(objNonWorkingDaysList);
        }

        // Get method to return create Non-Working Days page
        public IActionResult Create()
        {
            return View();
        }

        // Post async method with validation to prevent duplicate Non-Working Days date being created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(NonWorkingDays obj)
        {
            var dupCheck = _db.nonWorkingDays.FirstOrDefault(x => x.Date == obj.Date);
            if (dupCheck != null)
            {
                ModelState.AddModelError("", "Non-Working date already exists");
            }

            if (ModelState.IsValid)
            {
                _db.nonWorkingDays.Add(obj);
                await _db.SaveChangesAsync();
                TempData["save"] = "Non-Working Day Created Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get method to return Edit Non-Working days page
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound($"Unable to find Non-Working day");
            }

            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

            if (NonWorkingDaysSearch == null)
            {
                return NotFound($"Unable to find Department");
            }
            return View(NonWorkingDaysSearch);
        }

        // Post async method to edit Non-Working days with validation to prevent duplicate dates being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NonWorkingDays obj)
        {

            var dupCheck = !_db.nonWorkingDays.Any(x => x.Id != obj.Id && x.Date == obj.Date);

            if (dupCheck == false)
            {
                ModelState.AddModelError("Date", "Non-Working already exists");
            }

            if (ModelState.IsValid)
            {
                _db.nonWorkingDays.Update(obj);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Non-Working day Updated Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        // Get method to return Delete Non-Working day page by Department Id
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound($"Unable to find Non-Wroking day");
            }

            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

            if (NonWorkingDaysSearch == null)
            {
                return NotFound($"Unable to find Non-Wroking day");
            }
            return View(NonWorkingDaysSearch);
        }

        // Post async method to delete Non-Working day by Non-Working day Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

                if (NonWorkingDaysSearch == null)
                {
                    return NotFound($"Unable to find Non-Wroking day");
                }

                _db.nonWorkingDays.Remove(NonWorkingDaysSearch);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Non-Working day Deleted Successfully!!";
                return RedirectToAction("Index");
        }



        // Get method to return Non-Working days data in API call for Data Tables 
        #region API CALLS
        [HttpGet]

        public IActionResult IndexAPI()
        {
            IEnumerable<NonWorkingDays> objNonWorkingDaysList = _db.nonWorkingDays;
            return Json(new { data = objNonWorkingDaysList });
        }
        #endregion 


    }
}
