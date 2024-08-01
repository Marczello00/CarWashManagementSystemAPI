using CarWashManagementSystem.Models;

namespace CarWashManagementSystem.Interfaces
{
    public interface IUserContextService
    {
        public CurrentUser? GetCurrentUser();
    }
}
