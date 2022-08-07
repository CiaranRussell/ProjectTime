using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;


namespace ProjectTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {   
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager)

        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
        }

        // Get method to return list of users
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
            var user = _userManager.Users.FirstOrDefault(x => x.Id == appUser.Id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //var userRoles = await _roleManager.GetRoleNameAsync(user);
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");
            
            var model = new ApplicationUser
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                //Role = user.Role,
                Department = user.Department,
            };
            return View(model);

        }

        // Post async method to update user with validation to prevent duplicate email address been added 
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser appUser)
        {
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            var dupCheckEmail = !_userManager.Users.Any(x => x.Id != appUser.Id && x.Email.ToLower().Trim() ==
                                                                               appUser.Email.ToLower().Trim());

            if (dupCheckEmail == false)
            {
                ModelState.AddModelError("Email", "Eamil address already exists");
                return View(appUser);
            }

            var user = await _userManager.FindByIdAsync(appUser.Id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            else
            {
                user.Id = appUser.Id;
                user.FullName = appUser.FullName;
                user.Email = appUser.Email;
                user.Department = appUser.Department;
                //user.Role = appUser.Role;
            }

            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();
                TempData["edit"] = "User Updated Successfully!!";
                return RedirectToAction("Index");
            }
            return View(user);

        }
        // Get user & roles by user Id  
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            //var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new List<UserRoles>();

            foreach (var role in _roleManager.Roles)
            {
                var userRoles = new UserRoles
                {
                    RoleId = role.Id,
                    RoleName = role.Name
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

        // Get method to return Delete User page by identity userId
        public IActionResult Delete(IdentityUser identityUser)
        {
            ViewData["departmentId"] = new SelectList(_db.departments.ToList(), "Id", "Name");

            var user = _userManager.Users.FirstOrDefault(x => x.Id == identityUser.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View(user);
            
        }

        // Post async method to delete User by identity userId
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(IdentityUser identityUser)
        {
            try
            {
                var userSearch = _userManager.Users.FirstOrDefault(x => x.Id == identityUser.Id);

                if (userSearch == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                await _userManager.DeleteAsync(userSearch);
                await _db.SaveChangesAsync();
                TempData["delete"] = "User Account Deleted Successfully!!";
                return RedirectToAction("Index");
            } 
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        //Get method to return Users data in API call for Data Tables
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


    }
}
