using AuthSystem.Areas.Identity.Data;
using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GSP.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Task<ApplicationUser> ActivateUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> DeactivateUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var users = await applicationDbContext.Users.ToListAsync();
            return users;
        }

        public Task<int> UserLogins(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
