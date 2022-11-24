using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ISessionHelper _sessionHelper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger, ISessionHelper sessionHelper)

        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _logger = logger;
            _sessionHelper = sessionHelper;
        }

        // Get method to return list of users to include the department and roles 
        public IActionResult Index()
        {
            var objUsersList = _db.applicationUsers.Include(d => d.Department).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objUsersList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return View(objUsersList);


        }

        // Get method to return Edit User page by identity userId
        [HttpGet]
        public IActionResult Edit(IdentityUser appUser)
        {
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            var user = _userManager.Users.Include(d => d.Department).FirstOrDefault(x => x.Id == appUser.Id);

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit get User, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            return View(user);

        }

        // Post async method to update user with validation to prevent duplicate email address been added 
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser appUser)
        {
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");
            var dupCheckEmail = !_userManager.Users.Any(x => x.Id != appUser.Id && x.Email.ToLower().Trim() ==
                                                                               appUser.Email.ToLower().Trim());
            if (!dupCheckEmail)
            {
                ModelState.AddModelError("Email", "Email address already exists");
                return View(appUser);
            }

            var user = await _userManager.FindByIdAsync(appUser.Id);

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on edit post User, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            else
            {
                user.Id = appUser.Id;
                user.FullName = appUser.FullName;
                user.Email = appUser.Email;
                user.DepartmentId = appUser.DepartmentId;

            }

            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();
                TempData["edit"] = "User Updated Successfully!!";
                return RedirectToAction("Index");
            }
            return View(user);

        }



        // Get method to return user role view by user Id with logging and validation to return error view if no UserId is found   
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;


            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.UserName = user.FullName;

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on ManageUserRoles get, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRoles = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    Name = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoles.IsSelected = true;
                }
                else
                {
                    userRoles.IsSelected = false;
                }

                model.Add(userRoles);

            }
            return View(model);

        }

        // Post async method to allow the adimn user add or remove roles for a selected user with validation to prevent
        // removing all roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> userRoles, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UserName = user.FullName;

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on ManageUserRoles post, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            if (!userRoles.Any(x => x.IsSelected))
            {
                ModelState.AddModelError("", "No Role Assigned!!");
                return View(userRoles);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(userRoles);
            }
            result = await _userManager.AddToRolesAsync(user, userRoles.Where(x => x.IsSelected).Select(y => y.Name));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(userRoles);
            }
            await _db.SaveChangesAsync();
            TempData["edit"] = "User Role Updated Successfully!!";
            return RedirectToAction("Index");

        }

        // Get method to return Delete User page by identity userId
        public IActionResult Delete(IdentityUser identityUser)
        {
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");


            var user = _userManager.Users.Include(d => d.Department).FirstOrDefault(x => x.Id == identityUser.Id);
            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on delete get User, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(user);

        }

        // Post async method to delete User by identity userId
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(IdentityUser identityUser)
        {
            var userId = _sessionHelper.GetUserId();
            var user = _userManager.Users.FirstOrDefault(x => x.Id == identityUser.Id);

            try
            {
                if (user == null)
                {
                    _logger.LogError((EventId)101, "Invalid operation on delete post User, null object Id {date}:", DateTime.Now);
                    return View("Error");
                }

                await _userManager.DeleteAsync(user);
                await _db.SaveChangesAsync();
                TempData["delete"] = "User Account Deleted Successfully!!";
                _logger.LogWarning((EventId)102, "UserId {user} deleted User object Id {id} on {date}", userId, identityUser.Id, DateTime.Now);
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError((EventId)100, $"Invalid operation by UserId {{0}} on {{1}} User object, database exception error {{2}}: {ex.InnerException}", userId, user.FullName, DateTime.Now);
                ViewBag.ErrorTitle = $"{user.FullName} is Assigned to a Role or Projects";
                ViewBag.ErrorMessage = $"{user.FullName} cannot be deleted as this user is assigned to a role or project";
                return View("Error");
            }
        }

        //Get method to return Users data in API call for Data Tables to include the department and roles 
        #region API CALLS
        [HttpGet]
        public IActionResult IndexAPI()
        {
            var objUsersList = _db.applicationUsers.Include(d => d.Department).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objUsersList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = objUsersList });
        }
        #endregion

        // Get method to return lock / unlock User page by identity userId
        public async Task<IActionResult> LockUnlock(string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.UserName = user.FullName;
            ViewBag.Lockout = user.LockoutEnd;

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on LockUnLock get User, null object Id {date}:", DateTime.Now);
                return View("Error");
            }
            return View(user);

        }

        // post method to Lock / unlock user accounts
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult LockUnlockUser(string id)
        {

            var user = _db.applicationUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                _logger.LogError((EventId)101, "Invalid operation on LockUnlockUser post, null object Id {date}:", DateTime.Now);
                return View("Error");
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;

            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }

            _db.SaveChanges();

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                TempData["delete"] = "User Account locked Successfully!!";
            }
            else
            {
                TempData["edit"] = "User Account Unlocked Successfully!!";
            }
            return RedirectToAction("Index");
        }

    }
}
