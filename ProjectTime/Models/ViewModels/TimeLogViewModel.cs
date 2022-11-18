using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models.ViewModels
{
    public class TimeLogViewModel
    {
        public int Id { get; set; }

        [StringLength(250, ErrorMessage = "Maximum 250 characters")]
        [ValidateNever]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is requied")]
        [Range(0, 24, ErrorMessage = "Hours must be between 0 & 24")]
        [DisplayName("Duration Hours")]
        public decimal Duration { get; set; }

        [Required(ErrorMessage = "Date is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        [DisplayName("Log Date")]
        public DateTime Date { get; set; }

        public int ProjectId { get; set; }

    }
}
