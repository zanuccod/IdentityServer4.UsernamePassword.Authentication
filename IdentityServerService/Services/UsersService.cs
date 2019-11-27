using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServerService.Model;

namespace IdentityServerService.Services
{
    public class UsersService : IUserService
    {
        public Task<User> FindAsync(string username)
        {
            return Task.FromResult(new User
            {
                Username = "demo",
                Password = "demo".ToSha256(),
                UserId = 1,
                Email = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                Role = string.Empty
            });
        }

        public Task<User> FindAsync(long userId)
        {
            return Task.FromResult(new User
            {
                Username = "demo",
                Password = "demo".ToSha256(),
                UserId = 1,
                Email = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                Role = string.Empty
            });
        }
    }
}
