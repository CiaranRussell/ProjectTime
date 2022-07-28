using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Full Name is required")]
        [DisplayName("Full Name")]
        [MinLength(3, ErrorMessage ="Full Name must be greather than 3 characters!!")]
    
        public string FullName { get; set; }


    }
}
