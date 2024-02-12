using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace GSP.Repositories
{
    public class ServiceContractReportsRepository : IServiceContractReportsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ServiceContractReportsRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<ServiceContract>> GetDailyReports()
        {
            DateTime today = DateTime.Today;
            return await applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= today && c.AddedOn < today.AddDays(1) )
                .ToListAsync();
        }





        public async Task<IEnumerable<ServiceContract>> GetMonthlyReports()
        {
            DateTime firstDayOfMonth = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return await applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= firstDayOfMonth && c.AddedOn <= lastDayOfMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceContract>> GetWeeklyReports()


        {
            //DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6); 
            //DateTime endOfWeek = startOfWeek.AddDays(6);
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek - 1);
            var endOfWeek = startOfWeek.AddDays(5);

            return await applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= startOfWeek && c.AddedOn <= endOfWeek)
                .ToListAsync();
        }

        public async Task<IQueryable<ServiceContract>> GetDailyReportsForUser(string userId)
        {
            DateTime today = DateTime.Today;
            return applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= today && c.AddedOn < today.AddDays(1) && c.UserId == userId);
        }

        public async Task<IQueryable<ServiceContract>> GetMonthlyReportsForUser(string userId)
        {
            DateTime firstDayOfMonth = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= firstDayOfMonth && c.AddedOn <= lastDayOfMonth && c.UserId == userId);
        }

        public async Task<IQueryable<ServiceContract>> GetWeeklyReportsForUser(string userId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek - 1);
            var endOfWeek = startOfWeek.AddDays(5);

            return applicationDbContext.ServiceContracts
                .Where(c => c.AddedOn >= startOfWeek && c.AddedOn <= endOfWeek && c.UserId == userId);
        }
    }
}
