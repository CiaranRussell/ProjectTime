using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [DisplayName("Department Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rate/Hr Amount is required")]
        [DisplayName("€ Rate/Hr")]
        [Range(1,1000,ErrorMessage = "Rate/Hr Amount must be between €1 & €1000")]
        public decimal Rate { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

    }
}
