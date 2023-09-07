using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
        
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var ViewModelList = new List<UsersViewModel>();
                foreach(var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                   var ViewModel = new UsersViewModel
                   {
                       Id = user.Id,
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       UserName = user.UserName!,
                       Email = user.Email!,
                       Roles = roles
                   };
                   ViewModelList.Add(ViewModel);
                }
                return View(ViewModelList);
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
        public async Task<IActionResult> Add()
        {
            try
            {
                var roles = await _roleManager.Roles.Select(r => new RoleViewModel { Id=r.Id, Name=r.Name! }).ToListAsync();
                var ViewModel = new AddUserViewModel
                {
                    Roles = roles
                };
                return View(ViewModel);
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }
                if(await _userManager.FindByEmailAsync(model.Email)!=null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }
                if(await _userManager.FindByNameAsync(model.UserName)!=null)
                {
                    ModelState.AddModelError("UserName", "UserName already exists");
                    return View(model);
                }
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName
                };
                var result= await _userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("Roles", error.Description.Humanize(LetterCasing.Sentence));
                    }
                    return View(model);
              
                }
               await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.Name));
               return RedirectToAction(nameof(Index));
                  
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
        public async Task<IActionResult> EditProfile(string userId)
        {
              try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user==null)
                {
                    return NotFound();
                }
                
                var ViewModel = new EditProfileViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    
                };
                return View(ViewModel);
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }
            var userWithSameName = await _userManager.FindByNameAsync(model.UserName);
            if(userWithSameName != null && userWithSameName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "UserName already exists");
                return View(model);
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            try
            {
                var user= await _userManager.FindByIdAsync(userId);
                if(user==null)
                {
                    return NotFound();
                }
                var roles = await _roleManager.Roles.ToListAsync();
                var RolesViewModelList = new List<RoleViewModel>();
                foreach(var role in roles)
                {
                    
                    var RoleViewModel = new RoleViewModel
                    {
                        Id = role.Id,
                        Name = role.Name!,
                        IsSelected = await _userManager.IsInRoleAsync(user, role.Name!)
                    };
                    RolesViewModelList.Add(RoleViewModel);
                }
                var ViewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    Roles = RolesViewModelList
                };
                return View(ViewModel);
            }
            catch
            (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user==null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in model.Roles)
            {
                {
                    if(userRoles.Any(r=>r==role.Name) && ! role.IsSelected)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name!);
                    }
                    if(!userRoles.Any(r=>r==role.Name) && role.IsSelected)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
