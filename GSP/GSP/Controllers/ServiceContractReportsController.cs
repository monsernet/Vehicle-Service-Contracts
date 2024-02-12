using GSP.Models.ViewModels;
using GSP.Models;
using Microsoft.AspNetCore.Mvc;
using GSP.Repositories;
using Microsoft.AspNetCore.Identity;
using AuthSystem.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace GSP.Controllers
{
    public class ServiceContractReportsController : Controller
    {
        private readonly IServiceContractReportsRepository _reportsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ServiceContractReportsController(
            IServiceContractReportsRepository reportsService,
            UserManager<ApplicationUser> userManager)
        {
            _reportsService = reportsService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> ServiceContractReports()
        {
            try
            {
                var today = DateTime.Today;
                var startOfWeek = today.AddDays(-(int)today.DayOfWeek-1); 
                var endOfWeek = startOfWeek.AddDays(5);

                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                string monthFirstDayName = firstDayOfMonth.ToString("dddd, MMMM dd, yyyy");
                string monthLastDayName = lastDayOfMonth.ToString("dddd, MMMM dd, yyyy");
                var viewModel = new ServiceContractReportsViewModel
                {
                    DailyReports = await _reportsService.GetDailyReports(),
                    WeeklyReports = await _reportsService.GetWeeklyReports(),
                    MonthlyReports = await _reportsService.GetMonthlyReports(),
                    WeeklyReportStartDate = startOfWeek,
                    WeeklyReportEndDate = endOfWeek,
                    MonthlyReportStartDate = firstDayOfMonth,
                    MonthlyReportEndDate = lastDayOfMonth,
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> UserReports()
        {
            var users = await userManager.Users.ToListAsync();

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek - 1);
            var endOfWeek = startOfWeek.AddDays(5);

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var userReports = new List<UserReportViewModel>();
            foreach (var user in users)
            {
                var userId = user.Id;
                var userName = user.FirstName + " "+ user.LastName; // Assuming UserName contains user's full name

                // Daily reports
                var dailyReports = await _reportsService.GetDailyReportsForUser(userId);
                var dailyReportsCount = dailyReports.Count();
                decimal dailyTotalAmount = (decimal)dailyReports.Sum(report => report.TotalCost); 

                // Weekly reports
                var weeklyReports = await _reportsService.GetWeeklyReportsForUser(userId);
                var weeklyReportsCount = weeklyReports.Count();
                decimal weeklyTotalAmount = (decimal)weeklyReports.Sum(report => report.TotalCost);

                // Monthly reports
                var monthlyReports = await _reportsService.GetMonthlyReportsForUser(userId);
                var monthlyReportsCount = monthlyReports.Count();
                decimal monthlyTotalAmount = (decimal)monthlyReports.Sum(report => report.TotalCost);

                // Generate links for viewing service contract details
                var dailyReportsLink = Url.Action("UserDailyReports", "Reports", new { userId }, protocol: HttpContext.Request.Scheme);
                var weeklyReportsLink = Url.Action("UserWeeklyReports", "Reports", new { userId }, protocol: HttpContext.Request.Scheme);
                var monthlyReportsLink = Url.Action("UserMonthlyReports", "Reports", new { userId }, protocol: HttpContext.Request.Scheme);

                userReports.Add(new UserReportViewModel
                {
                    UserId = userId,
                    UserName = userName,
                    DailyReportsCount = dailyReportsCount,
                    DailyTotalAmount = dailyTotalAmount,
                    DailyReportsLink = dailyReportsLink,
                    WeeklyReportsCount = weeklyReportsCount,
                    WeeklyTotalAmount = weeklyTotalAmount,
                    WeeklyReportsLink = weeklyReportsLink,
                    MonthlyReportsCount = monthlyReportsCount,
                    MonthlyTotalAmount = monthlyTotalAmount,
                    MonthlyReportsLink = monthlyReportsLink
                });
            }

            return View(userReports);
        }



        //public async Task<IActionResult> ServiceContractReportsByUser()
        //{
        //    var users = await userManager.Users.ToListAsync();

        //    var today = DateTime.Today;
        //    var startOfWeek = today.AddDays(-(int)today.DayOfWeek - 1);
        //    var endOfWeek = startOfWeek.AddDays(5);

        //    var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        //    string monthFirstDayName = firstDayOfMonth.ToString("dddd, MMMM dd, yyyy");
        //    string monthLastDayName = lastDayOfMonth.ToString("dddd, MMMM dd, yyyy");

        //    foreach (var user in users)
        //    {
        //        var userId = user.Id;

        //        // Daily reports
        //        var dailyReports = await _reportsService.GetDailyReportsForUser(userId);

        //        // Weekly reports
        //        var weeklyReports = await _reportsService.GetWeeklyReportsForUser(userId);

        //        // Monthly reports
        //        var monthlyReports = await _reportsService.GetMonthlyReportsForUser(userId);
        //    }


        //}
    }
}
