using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class ProjectUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]

        public Project Project { get; set; }

        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [ValidateNever]
        public string CreatedByUserId { get; set; }

        [ValidateNever]
        public string? ModifiedByUserId { get; set; }

        public DateTime ModifyDateTime { get; set; }


    }
}
