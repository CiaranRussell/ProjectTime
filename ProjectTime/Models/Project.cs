using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


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
    }
}
