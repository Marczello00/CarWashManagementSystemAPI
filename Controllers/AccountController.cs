using CarWashManagementSystem.Constants;
using CarWashManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CarWashManagementSystem.Controllers
{
    [ApiController]
    [Route("api/identity")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> AssignUserRole(string userEmail, string role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var newRole = await _roleManager.FindByNameAsync(role);
            if (newRole == null)
            {
                return new NotFoundResult();
            }

            if (await _userManager.IsInRoleAsync(user, newRole.Name!))
            {
                return new ConflictResult();
            }
            var result = await _userManager.AddToRoleAsync(user, newRole.Name!);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Assigning {role} role to {userEmail}");
                return new OkResult();
            }
            return new BadRequestResult();
        }
        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveUserRole(string userEmail, string role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var roleToDelete = await _roleManager.FindByNameAsync(role);
            if (roleToDelete == null)
            {
                return new NotFoundResult();
            }

            if (!await _userManager.IsInRoleAsync(user, roleToDelete.Name!))
            {
                return new NotFoundResult();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleToDelete.Name!);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Removing {role} role from {userEmail}");
                return new OkResult();
            }
            return new BadRequestResult();
        }
        [HttpGet("getMyRoles")]
        public async Task<IActionResult> GetMyRoles()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new NotFoundResult();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new OkObjectResult(roles);
        }
    }
}
