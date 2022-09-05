using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Data;
using NotesApp.Models;
using NotesApp.ViewModels;
using BC = BCrypt.Net.BCrypt;

namespace NotesApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<NoteUser> userManager;
		private readonly SignInManager<NoteUser> loginManager;
        private ApplicationDbContext? Context { get; }
        public AccountController(UserManager<NoteUser> userManager, SignInManager<NoteUser> loginManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.loginManager = loginManager;
			this.Context = context;
		}

		[HttpGet, AllowAnonymous]
		public IActionResult Register()
		{
			UserRegistrationModel userRegistrationModel = new UserRegistrationModel();
			return View(userRegistrationModel);
		}

		[HttpPost, AllowAnonymous]
		public async Task<IActionResult> Register(UserRegistrationModel userRegistrationModel)
		{
			if(ModelState.IsValid)
			{
				var user1 = await userManager.FindByEmailAsync(userRegistrationModel.Email);
				if(user1 == null)
				{
					var user = new NoteUser
					{
						UserName = userRegistrationModel.UserName,
						Email = userRegistrationModel.Email
					};

					user.Password = BC.HashPassword(userRegistrationModel.Password);
                    var result = await userManager.CreateAsync(user);
					if(result.Succeeded)
					{
						return RedirectToAction("Login", "Account");
					}
					else
					{
						if(result.Errors.Any())
						{
							foreach (var Error in result.Errors)
								ModelState.AddModelError("message", Error.Description);
						}
						return View(Request);
					}
				}
				else
				{
					ModelState.AddModelError("message", "Email already exists.");
					return View(Request);
				}
			}
			return View(Request);
		}

		[HttpGet, AllowAnonymous]
		public IActionResult Login()
		{
			UserLoginModel userLoginModel = new UserLoginModel();
			return View(userLoginModel);
		}

		[HttpPost, AllowAnonymous]
		public async Task<IActionResult> Login(UserLoginModel userLoginModel)
		{
			if(ModelState.IsValid)
			{
                var user = await userManager.FindByEmailAsync(userLoginModel.Email);
				/*if (await userManager.CheckPasswordAsync(user, userLoginModel.Password) == false)
                {
					ModelState.AddModelError("message", "Invalid input");
					return View(userLoginModel);
				}*/
				bool verified = BC.Verify(userLoginModel.Password, user.Password);
				/*var result = await loginManager.PasswordSignInAsync(user.UserName, userLoginModel.Password, true, true);
				if(result.Succeeded)
				{
					await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserRole", "Admin"));
					return Redirect("/Home/Index");
				}*/
                var result = BC.Verify(userLoginModel.Password, user.Password);
                if (result)
				{
					await loginManager.SignInAsync(user, true);
					// await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserRole", "Admin"));
					return Redirect("/Home/Index");
				}
				else
				{
					ModelState.AddModelError("message", "Invalid login attempt.");
					return View(userLoginModel);
				}
            }
			return View(userLoginModel);
		}

		[HttpPost, Authorize]
		public async Task<IActionResult> Logout()
		{
			await loginManager.SignOutAsync();
			return Redirect("/");
		}
	}
}
