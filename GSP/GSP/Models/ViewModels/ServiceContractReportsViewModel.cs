using GSP.Models.Domain;

namespace GSP.Models.ViewModels
{
    public class ServiceContractReportsViewModel
    {
        public IEnumerable<ServiceContract> DailyReports { get; set; }
        public IEnumerable<ServiceContract> WeeklyReports { get; set; }
        public IEnumerable<ServiceContract> MonthlyReports { get; set; }
        public string SelectedReport { get; set; }
        public DateTime WeeklyReportStartDate { get; set; }
        public DateTime WeeklyReportEndDate { get; set; }
        public DateTime MonthlyReportStartDate { get; set; }
        public DateTime MonthlyReportEndDate { get; set; }

        public IQueryable<ServiceContract> UserDailyReports { get; set; }
        public IQueryable<ServiceContract> UserWeeklyReports { get; set; }
        public IQueryable<ServiceContract> UserMonthlyReports { get; set; }
    }
}
