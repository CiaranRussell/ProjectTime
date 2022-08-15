using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Data;
using ProjectTime.Models;
using System.Linq;

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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(user);

        }

        // Post async method to update user with validation to prevent duplicate email address been added 
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser appUser)
        {

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

        

        // Get method to return user roles by user Id   
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;
            

            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.UserName = user.FullName;

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRoles = new UserRolesViewModel
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

        // Post async method to allow the adimn user add or delete roles for a selected user with validation to prevent
        // removing all roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> userRoles,string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (! userRoles.Any(x => x.IsSelected == true))
            {
                ViewBag.ErrorTitle = $" User: {user.FullName}, has no Role Assigned ";
                ViewBag.ErrorMessage = $"All system user's must have at least 1 role assigned";
                return View("Error");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(userRoles);
            }
            result = await _userManager.AddToRolesAsync(user, userRoles.Where(x => x.IsSelected).Select(y => y.RoleName));

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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View(user);
            
        }

        // Post async method to delete User by identity userId
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(IdentityUser identityUser)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == identityUser.Id);

            try
            {
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                await _userManager.DeleteAsync(user);
                await _db.SaveChangesAsync();
                TempData["delete"] = "User Account Deleted Successfully!!";
                return RedirectToAction("Index");
            } 
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"{user.FullName} is Assigned to Projects or Departments";
                ViewBag.ErrorMessage = $"{user.FullName} cannot be deleted as this user is assigned to projects or departments";
                return View("Error");
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

        // get method to return lock / unlock User page by identity userId
        public async Task<IActionResult> LockUnlock(string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.UserName = user.FullName;
            ViewBag.Lockout = user.LockoutEnd;

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View(user);

        }

        // post method to Lock / unlock user accounts
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult LockUnlockUser(string id)
        {
            
            var user = _db.applicationUsers.FirstOrDefault(u => u.Id == id);

            if(user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.LockoutEnd!= null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
                
            }
            else
            {
                user.LockoutEnd= DateTime.Now.AddYears(100);
                
            }
            //ViewBag.Lockout = user.LockoutEnd;

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
