using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static ProjectTime.Models.CustomValidation;

namespace ProjectTime.Models
{
    public class UserRolesViewModel : IdentityRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        [CheckBoxRequired(ErrorMessage = "User Must have a role")]
        public bool IsSelected { get; set; }

    }

}
