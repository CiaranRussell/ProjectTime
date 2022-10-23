namespace ProjectTime.Models.ViewModels
{
    public class ActualVEstimateViewModel
    {
        public string ProjectName { get; set; }

        public string DepartmentName { get; set; }

        public string EstimateDaysFrom { get; set; }

        public string EstimateDaysTo { get; set; }

        public string ActualDaysFrom { get; set; }

        public string ActualDaysTo { get; set; }

        public int EstimateDurationdays { get; set; }

        public int ActualDurationdays { get; set; }

        public decimal EstimateCost { get; set; }

        public decimal ActualCost { get; set; }


    }
}
