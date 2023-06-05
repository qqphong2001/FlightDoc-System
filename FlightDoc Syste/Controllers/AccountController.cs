using FlightDoc_Syste.Data;
using FlightDoc_Syste.Model;
using FlightDoc_Syste.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FlightDoc_Syste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountRepository accountRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _accountRepository = accountRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _accountRepository.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var result = await _accountRepository.SignInAsync(model);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var result = _accountRepository.SignOutAsync();

            return Ok(true);
        }


        [HttpPost("seedDataRole")]
        public async Task<IActionResult> SeedDataRole()
        {
            var rolenames = typeof(Role).GetFields().ToList();
            foreach (var r in rolenames)
            {
                var rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }

            var userAdmin = await _userManager.FindByNameAsync(("Admin"));
            if (userAdmin == null)
            {
                userAdmin = new ApplicationUser()
                {
                    UserName = "Admin123@gmail.com",
                    Email = "Admin123@gmail.com",
                    EmailConfirmed = true,

                };
                await _userManager.CreateAsync(userAdmin, "Admin@123");
                await _userManager.AddToRoleAsync(userAdmin, Role.SystemAdmin);
            }
           
            return Ok(true);
        }

    }
}
