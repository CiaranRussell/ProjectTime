using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Project Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z''-'\s]{3,3}[\d]{3,3}$",
         ErrorMessage = "Project code format is strictly 'AAA999'")]
        [DisplayName("Project Code")]
        public string ProjectCode { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [ValidateNever]
        public string CreatedByUserId { get; set; }

        [ValidateNever]
        public string? ModifiedByUserId { get; set; }

        public DateTime ModifyDateTime { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Project Stage is required")]
        public int ProjectStageId { get; set; }
        [ForeignKey("ProjectStageId")]
        [ValidateNever]

        public ProjectStage ProjectStage { get; set; }

    }
}
