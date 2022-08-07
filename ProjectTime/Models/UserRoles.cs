using Microsoft.AspNetCore.Identity;

namespace ProjectTime.Models
{
    public class UserRoles : IdentityRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }


    }
}
