using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SharedModel.Repository
{
    public interface IUserRepository
    {
        string Id();
        string Name();
        string Role();
        string Email();
        string UserName();
        string Claim(string Key);
        object Props();
    }
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor accessor;

        public UserRepository(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string Claim(string Key)
        {
            string val = accessor.HttpContext.User.Claims
                .FirstOrDefault(s => s.Type == Key)?.Value;
            return val;
        }

        public string Email()
            => Claim(ClaimTypes.Email);

        public string Id()
            => "2f446a5e-694e-4a84-8e87-5123d9bf376d" ?? Claim(ClaimTypes.NameIdentifier)??
            (accessor.HttpContext.Request.Cookies.TryGetValue("user_id",out var _id)
            ?_id: null);

        public string Name()
            => Claim(ClaimTypes.GivenName);

        public string Role()
            => Claim(ClaimTypes.Role);

        public object Props()
            => new
            {
                Name = Name(),
                UserName = UserName(),
                Id = Id(),
                Email = Email(),
                Authenticated = accessor.HttpContext.User.Identity.IsAuthenticated,
                Role = Role(),
                Image = Claim("ImageUri")

            };

        public string UserName()
            => Claim(ClaimTypes.Name);
    }
}

