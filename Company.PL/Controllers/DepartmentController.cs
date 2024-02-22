using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Model;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class DepartmentController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper map;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper _map)
        {
            
          _unitOfWork = unitOfWork;
            map = _map;
        }

        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            var mappedDepartment=map.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentvm)
        {
            if (ModelState.IsValid)
            {
                var mappedDepartment = map.Map<DepartmentViewModel, Department>(departmentvm);
               await _unitOfWork.DepartmentRepository.AddAsync(mappedDepartment);
               await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(departmentvm);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department =await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            var mappedDepartment = map.Map < Department,DepartmentViewModel >(department);
            return View(viewName, mappedDepartment);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            //if (id is null)
            //{
            //    return BadRequest();
            //}
            //var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel departmentvm)
        {
            if (id != departmentvm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = map.Map< DepartmentViewModel, Department>(departmentvm);
                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                   await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentvm);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentvm,[FromRoute] int id)
        {
            if (id != departmentvm.Id)
                return BadRequest();
            try
            {
                var mappedDepartment = map.Map<DepartmentViewModel,Department>(departmentvm);
                _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentvm);
            }
          
        }
    }
}
