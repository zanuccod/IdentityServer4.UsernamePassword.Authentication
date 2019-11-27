using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;
using System.Collections.Generic;
using IdentityModel;

namespace IdentityServerService.Services
{
    public class ProfileService : IProfileService
    {
        // services
        private readonly IUserService _userRepository;

        public ProfileService(IUserService userRepository)
        {
            _userRepository = userRepository;
        }

        // Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                // depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await _userRepository.FindAsync(context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.Id, user.UserId.ToString() ??  string.Empty),
                            new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                            new Claim(JwtClaimTypes.GivenName, user.Firstname  ??  string.Empty),
                            new Claim(JwtClaimTypes.FamilyName, user.Lastname  ??  string.Empty),
                            new Claim(JwtClaimTypes.Email, user.Email  ??  string.Empty),

                            // roles
                            new Claim(JwtClaimTypes.Role, user.Role)
                        };

                        // set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    // get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);

                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        // get user from db (find user by user id)
                        var user = await _userRepository.FindAsync(long.Parse(userId.Value));

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Id, user.UserId.ToString() ??  string.Empty),
                                new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                                new Claim(JwtClaimTypes.GivenName, user.Firstname  ??  string.Empty),
                                new Claim(JwtClaimTypes.FamilyName, user.Lastname  ??  string.Empty),
                                new Claim(JwtClaimTypes.Email, user.Email  ??  string.Empty),

                                // roles
                                new Claim(JwtClaimTypes.Role, user.Role)
                            };

                            context.IssuedClaims = claims.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}: Erorr {ex}");
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                // get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);

                if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                {
                    var user = await _userRepository.FindAsync(long.Parse(userId.Value));

                    if (user != null)
                    {
                        context.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}: Erorr {ex}");
            }
        }
    }
}
