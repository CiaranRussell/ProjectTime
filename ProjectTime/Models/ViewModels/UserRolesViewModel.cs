using Microsoft.AspNetCore.Identity;

namespace ProjectTime.Models
{
    public class UserRolesViewModel : IdentityRole
    {
        public string RoleId { get; set; }
        public string Name { get; set; }

        //[CheckBoxRequired(ErrorMessage = "User must have a role")]
        public bool IsSelected { get; set; }

    }

}
