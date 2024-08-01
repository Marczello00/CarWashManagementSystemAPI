using CarWashManagementSystem.Interfaces;
using CarWashManagementSystem.Models;
using System.Security.Claims;

namespace CarWashManagementSystem.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(ClaimTypes.Email)!.Value;
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var roles2 = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
            return new CurrentUser(userId, email, roles);
        }
    }
}
