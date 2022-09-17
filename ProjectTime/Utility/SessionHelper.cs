using Microsoft.AspNetCore.Identity;
using ProjectTime.Data;
using ProjectTime.Models;
using System.Security.Claims;
using System.Web;

namespace ProjectTime.Utility
{
    public class SessionHelper : ISessionHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessionHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        // Session helper to return logged in UserId
        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Session helper to return logged in User Role
        public string GetUserRole()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.Role);
        }

    
    }
}
