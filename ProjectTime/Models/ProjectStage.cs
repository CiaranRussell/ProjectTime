using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models
{
    public class ProjectStage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Stage is required")]
        [DisplayName("Project Stage")]
        public string Stage { get; set; }

        [ValidateNever]
        public string? Description { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [ValidateNever]
        public string? CreatedByUserId { get; set; }

        [ValidateNever]
        public string? ModifiedByUserId { get; set; }

        public DateTime ModifyDateTime { get; set; }

    }
}
