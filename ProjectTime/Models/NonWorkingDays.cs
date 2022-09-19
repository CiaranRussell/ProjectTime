using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models
{
    public class NonWorkingDays
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Allow Time Log")]
        public bool AllowTimeLog { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

    }
}
