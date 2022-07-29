using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class ProjectAllocation
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public int DepartmentId { get; set; }
        [ForeignKey("Department")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }
        [ForeignKey("Project")]
        public Project Project { get; set; }

        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }
        [ForeignKey("User")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
