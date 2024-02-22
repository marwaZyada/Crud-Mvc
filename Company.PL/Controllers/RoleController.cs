using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                var roles =await _roleManager.Roles.ToListAsync();
                var mappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
                return View(mappedRole);
            }
            else
            {
                var roles=await _roleManager.FindByNameAsync(searchValue);
                var mappedRole = _mapper.Map<IdentityRole,RoleViewModel>(roles);
                return View(new List<RoleViewModel>() { mappedRole }) ;
            }
           
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        
        public async Task<IActionResult> Details(string Id,string ViewName="Details")
        {
            if (Id is null) return BadRequest();
            var role =await _roleManager.FindByIdAsync(Id);
            if(role is null)return NotFound();
            
           var mappedRole = _mapper.Map< IdentityRole, RoleViewModel>(role);
          

            return View(ViewName,mappedRole);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
      
        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute]string id)
        {
            if(id!=model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role =await _roleManager.FindByIdAsync(id);
                    role.Name = model.RoleName;
                   
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async  Task<IActionResult>Delete([FromRoute]string id,RoleViewModel model)
        {
            if (id != model.Id) return BadRequest();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View(model);
            }
            
        }
    }
}
