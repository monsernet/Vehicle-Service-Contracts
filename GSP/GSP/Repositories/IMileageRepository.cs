using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IMileageRepository
    {
        Task<IEnumerable<Mileage>> GetAllMileagesAsync();
        Task<Mileage?> GetMileageByIdAsync(int id);
        Task<Mileage> AddMileageAsync(Mileage mileage);
        Task<Mileage?> UpdateMileageAsync(Mileage mileage);
        Task<Mileage?> DeleteMileageAsync(int id);
        Task<Mileage?> GetMileageByNameAsync(string mileageName);
        Task<int> GetMileageCountAsync(int startMileageId, int endMileageId);
    }
}
