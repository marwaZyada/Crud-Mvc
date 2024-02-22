using AutoMapper;
using Company.DAL.Model;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var users = await _userManager.Users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Fname = u.FName,
                    Lname = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result

                }).ToListAsync();

                return View(users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(SearchValue);
                var mappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    Fname = user.FName,
                    Lname = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            var mappeduser = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(viewName, mappeduser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel uservm, [FromRoute] string id)
        {
            if (id == uservm.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        var user =await _userManager.FindByIdAsync(id);
                        user.FName = uservm.Fname;
                        user.LName = uservm.Lname;
                        user.PhoneNumber = uservm.PhoneNumber;
                      await  _userManager.UpdateAsync(user);
                       
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }

            }
            return View(uservm);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete( [FromRoute] string id, UserViewModel uservm)
        {
          
                try
                {

                var user =await _userManager.FindByIdAsync(id);
                   await _userManager.DeleteAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                  return View(uservm);
                    //return RedirectToAction("Error","Home");
                }
       
        }



    }

}

