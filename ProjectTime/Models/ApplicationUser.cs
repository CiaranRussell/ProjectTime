using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Full Name is required")]
        [DisplayName("Full Name")]
        [MinLength(3, ErrorMessage ="Full Name must be greather than 3 characters!!")]
    
        public string FullName { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [ValidateNever]
        public Department Department { get; set; }

        




    }
}
