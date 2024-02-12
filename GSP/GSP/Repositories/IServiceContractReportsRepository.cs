using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IServiceContractReportsRepository
    {
        Task<IEnumerable<ServiceContract>> GetDailyReports();
        Task<IEnumerable<ServiceContract>> GetWeeklyReports();
        Task<IEnumerable<ServiceContract>> GetMonthlyReports();
        Task<IQueryable<ServiceContract>> GetDailyReportsForUser(string userId);
        Task<IQueryable<ServiceContract>> GetMonthlyReportsForUser(string userId);
        Task<IQueryable<ServiceContract>> GetWeeklyReportsForUser(string userId);
    }
}
