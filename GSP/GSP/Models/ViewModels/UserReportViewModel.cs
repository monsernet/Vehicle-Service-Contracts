namespace GSP.Models.ViewModels
{
    public class UserReportViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int DailyReportsCount { get; set; }
        public decimal DailyTotalAmount { get; set; } 
        public string DailyReportsLink { get; set; }
        public int WeeklyReportsCount { get; set; }
        public decimal WeeklyTotalAmount { get; set; } 
        public string WeeklyReportsLink { get; set; }
        public int MonthlyReportsCount { get; set; }
        public decimal MonthlyTotalAmount { get; set; } 
        public string MonthlyReportsLink { get; set; }
    }
}
