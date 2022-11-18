using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models.ViewModels
{
    public class ProjectEstimateViewModel
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Duration Days is requied")]
        [Range(1, 365, ErrorMessage = "Duration Days must be between 1 & 365")]
        [DisplayName("Duration Days")]
        public decimal DurationDays { get; set; }

        [Required(ErrorMessage = "Date From is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("From Date")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Date To From is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("To Date")]
        public DateTime DateTo { get; set; }

        public int ProjectId { get; set; }

        public int DepartmentId { get; set; }

    }
}
