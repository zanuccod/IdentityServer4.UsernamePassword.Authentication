using System;
namespace IdentityServerService.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public long UserId { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
