﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTime.Models
{
    public class TimeLog
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Duration is requied")]
        [Range(0,24,ErrorMessage ="Hours must be between 0 & 24")]
        [DisplayName("Duration Hours")]
        public decimal Duration { get; set; }

        [Required(ErrorMessage ="Date is requied")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = false)]
        [DisplayName("Log Date")]
        public DateTime Date { get; set; }

        [NotMapped]
        [ValidateNever]
        public string MinDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public string MaxDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal TotalCost { get; set; }

        public int ProjectUserId { get; set; }
        [ForeignKey("ProjectUserId")]
        [ValidateNever]

        public ProjectUser ProjectUser { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]

        public Project Project { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public DateTime ModifyDateTime { get; set; }

        // Estiamte Properties to hold to store field values in memory

        [NotMapped]
        [ValidateNever]
        public decimal EstimateDurationDays { get; set; }

        [NotMapped]
        [ValidateNever]
        public string EstimateMinDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public string EstimateMaxDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal EstimateTotalCost { get; set; }

        // Variance Properties to store field values in memory
        [NotMapped]
        [ValidateNever]
        public decimal TotalCostVariance { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal DurationDaysVariance { get; set; }

        [NotMapped]
        [ValidateNever]
        public string UnderOverBudget { get; set; }

        [NotMapped]
        [ValidateNever]
        public string UnderOverDuration { get; set; }

    }
}
