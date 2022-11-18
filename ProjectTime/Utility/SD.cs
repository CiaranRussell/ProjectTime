namespace ProjectTime.Utility
{
    public static class SD
    {
        // Const properties for variable to hold values for Roles, PreventAfterDays, Report values & Wroking day

        public const string Role_Admin = "PMO";
        public const string Role_SuperUser = "Project Manager";
        public const string Role_User = "User";
        public const int Prevent_After_NoDays = 40;
        public const string Over_Budget = "Over Budget";
        public const string On_Budget = "On Budget";
        public const string Over_Duration = "Over Duration";
        public const string Under_Duration = "Under Duration";
        public const decimal WorkingDay = 7.5M;

    }
}
