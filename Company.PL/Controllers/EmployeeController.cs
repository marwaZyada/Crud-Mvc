using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Model;
using Company.PL.settings;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            mapper = _mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searchValue))
            {
                employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees=_unitOfWork.EmployeeRepository.GetEmployeesByName(searchValue);
            }

            var mappedEmployees=mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(employees);
           
            return View(mappedEmployees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.department =await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeevm)
        {
            if (ModelState.IsValid)
            {
                employeevm.ImageName = DocumentSetting.UploadFile(employeevm.Image, "Images");
                var mappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeevm);
             await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                int result =await _unitOfWork.Complete();
                if (result > 0)
                {
                    TempData["message"] = "new employee is created";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeevm);
        }
        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if(id is null)
            {
                return BadRequest();
            }
            var employee =await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if(employee is null)
            {
                return NotFound();
            }
            var mappedEmployee = mapper.Map< Employee, EmployeeViewModel>(employee);
            return View(viewName, mappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.department =await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeevm,[FromRoute] int id)
        {
            if (id == employeevm.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        employeevm.ImageName = DocumentSetting.UploadFile(employeevm.Image, "Images");
                        var mappedEmployee = mapper.Map< EmployeeViewModel, Employee>(employeevm);
                        _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                     await _unitOfWork.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
              
            }
            return View(employeevm);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeevm,[FromRoute] int? id)
        {
            if (id != employeevm.Id)
                return BadRequest();
            {
                try
                {
                    string ImageName = employeevm.ImageName;
                    var mappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeevm);
                    _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                   int result=await _unitOfWork.Complete();
                    if (result > 0)
                    {
                        DocumentSetting.DeleteFile(ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employeevm);
                }
            }
          

        }
    }

}
