﻿using Company.DAL.Model;
using Company.PL.settings;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
		{
			
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Register(RegesterViewModel model)
		{
            if(ModelState.IsValid)
            {
				var User = new ApplicationUser()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					IsAgree = model.IsAgree,
					FName = model.Fname,
					LName = model.Lname,

				};
				var result=await _userManager.CreateAsync(User,model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}

            }
			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var result =await _userManager.CheckPasswordAsync(user, model.Password);
					if(result)
					{
						var loginResult=await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						
						if(loginResult.Succeeded)
						return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError(string.Empty, "InCorrect Password");

					}

				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email not Found");
				}
			}
			return View(model);
		}

		public new async Task<IActionResult> SignOut()
		{

			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		public IActionResult ForgetPassword()
		{
			return View();
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendMail(ForgetPasswordViewModel model)
		{
			
			if (ModelState.IsValid) {
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
			{
					var token=await _userManager.GeneratePasswordResetTokenAsync(user);
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = model.Email,Token=token },Request.Scheme);
					var Email = new Email()
					{
						Subject="Reset Password",
						To=model.Email,
						Body=ResetPasswordLink
					};
					EmailSettings.SendEmail(Email);
					return RedirectToAction(nameof(CheckYourInbox));
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Email is not Exist");
			}
		}
			
				return View("ForgetPassword",model);
			
			

					}
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{

				string email = TempData["email"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				string token = TempData["token"] as string;
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
				if (result.Succeeded)
					return RedirectToAction(nameof(Login));

				else

					foreach (var item in result.Errors)

						ModelState.AddModelError(string.Empty, item.Description);


			}
				return View(model);
			}
			
		}

	}


