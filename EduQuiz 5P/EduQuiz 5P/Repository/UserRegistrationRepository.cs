using EduQuiz_5P.Data;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EduQuiz_5P.Repository
{
    public class UserRegistrationRepository : GenericRepository<UserRegistration>, IUserRegistrationRepository
    {
        public UserRegistrationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<ICollection<MonthlyRegistrationData>> GetDailyRegistrationDataAsync()
        {

            var groupedData = await _dbContext.UserRegistration
                .GroupBy(u => new { Year = u.RegistrationDate.Year, Month = u.RegistrationDate.Month, Day = u.RegistrationDate.Day })
                .Select(g => new { Month = g.Key.Month, Year = g.Key.Year, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToListAsync();

            var monthNames = new string[]
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };

            var allMonths = Enumerable.Range(1, 12);

            var formattedData = new List<MonthlyRegistrationData>();

            foreach (var month in allMonths)
            {
                var dataForMonth = groupedData.FirstOrDefault(g => g.Month == month);

                formattedData.Add(dataForMonth != null
                    ? new MonthlyRegistrationData
                    {
                        Month = $"{monthNames[dataForMonth.Month - 1]} {dataForMonth.Year}",
                        Count = dataForMonth.Count
                    }
                    : new MonthlyRegistrationData
                    {
                        Month = $"{monthNames[month - 1]}",
                        Count = 0
                    });
            }
            return formattedData;
        }
    }
}
