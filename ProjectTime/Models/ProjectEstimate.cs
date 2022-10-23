using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class ProjectEstimate
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Duration Days is requied")]
        [Range(1, 365, ErrorMessage = "Duration Days must be between 1 & 365")]
        [DisplayName("Duration Days")]
        public decimal DurationDays { get; set; }

        [Required(ErrorMessage = "From Date is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("From Date")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "To Date is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("To Date")]
        public DateTime DateTo { get; set; }

        public int ProjectUserId { get; set; }
        [ForeignKey("ProjectUserId")]
        [ValidateNever]

        public ProjectUser ProjectUser { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]

        public Project Project { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [ValidateNever]
        public Department Department { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public DateTime ModifyDateTime { get; set; }

        [NotMapped]
        [ValidateNever]
        public string MinDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public string MaxDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal TotalCost { get; set; }

        [NotMapped]
        [ValidateNever]
        public dynamic projectTotalCost { get; internal set; }

        // Actual Properties 

        [NotMapped]
        [ValidateNever]
        public decimal ActualDurationDays { get; set; }

        [NotMapped]
        [ValidateNever]
        public string ActualMinDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public string ActualMaxDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal ActualTotalCost { get; set; }
    }
}
