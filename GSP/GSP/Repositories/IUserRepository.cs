using AuthSystem.Areas.Identity.Data;

namespace GSP.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<ApplicationUser> ActivateUser(string userId);
        Task<ApplicationUser> DeactivateUser(string userId);
        Task<ApplicationUser> DeleteUser(string userId);
        Task<int> UserLogins(string userId);
    }
}
