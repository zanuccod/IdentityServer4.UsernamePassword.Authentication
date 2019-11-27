using System;
using System.Threading.Tasks;
using IdentityServerService.Model;

namespace IdentityServerService.Services
{
    public interface IUserService
    {
        Task<User> FindAsync(string username);
        Task<User> FindAsync(long userId);
    }
}
