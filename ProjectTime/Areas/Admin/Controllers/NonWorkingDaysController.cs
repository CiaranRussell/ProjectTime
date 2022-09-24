using Microsoft.AspNetCore.Mvc;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NonWorkingDaysController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<NonWorkingDaysController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public NonWorkingDaysController(ApplicationDbContext db, ISessionHelper sessionHelper, ILogger<NonWorkingDaysController> logger)
        {
            _db = db;
            _sessionHelper = sessionHelper;
            _logger = logger;
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
            var userId = _sessionHelper.GetUserId();
            var dupCheck = _db.nonWorkingDays.FirstOrDefault(x => x.Date == obj.Date);
            if (dupCheck != null)
            {
                ModelState.AddModelError("", "Non-Working date already exists");
            }

            obj.CreatedByUserId = userId;

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
                _logger.LogError((EventId)101, "Invalid operation on edit get Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

            if (NonWorkingDaysSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }
            return View(NonWorkingDaysSearch);
        }

        // Post async method to edit Non-Working days with validation to prevent duplicate dates being created during edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NonWorkingDays obj)
        {
            var userId = _sessionHelper.GetUserId();
            var dupCheck = !_db.nonWorkingDays.Any(x => x.Id != obj.Id && x.Date == obj.Date);

            if (dupCheck == false)
            {
                ModelState.AddModelError("Date", "Non-Working already exists");
            }

            var nonWorkingDays = _db.nonWorkingDays.FirstOrDefault(x => x.Id == obj.Id);

            if (nonWorkingDays == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit post Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            nonWorkingDays.Date = obj.Date;
            nonWorkingDays.Description = obj.Description;
            nonWorkingDays.AllowTimeLog = obj.AllowTimeLog;
            nonWorkingDays.ModifiedByUserId = userId;
            nonWorkingDays.ModifyDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.nonWorkingDays.Update(nonWorkingDays);
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
                _logger.LogError((EventId)101, "Invalid operation on delete get Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

            if (NonWorkingDaysSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }
            return View(NonWorkingDaysSearch);
        }

        // Post async method to delete Non-Working day by Non-Working day Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var userId = _sessionHelper.GetUserId();
            var NonWorkingDaysSearch = _db.nonWorkingDays.FirstOrDefault(x => x.Id == id);

            if (NonWorkingDaysSearch == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete post Non-Working days, null object Id {0}:", DateTime.Now);
                return View("Error");
            }

            _db.nonWorkingDays.Remove(NonWorkingDaysSearch);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Non-Working day Deleted Successfully!!";
            _logger.LogWarning((EventId)102, "UserId {0} deleted Non-Working day object Id {1} on {2}", userId, NonWorkingDaysSearch.Id, DateTime.Now);
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
