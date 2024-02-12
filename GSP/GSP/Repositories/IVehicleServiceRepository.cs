using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IVehicleServiceRepository
    {
        Task<IEnumerable<VehicleService>> GetVehicleServicesAsync();
        Task<VehicleService> AddVehicleServiceAsync(VehicleService vehicleService);
        Task<VehicleService?> GetVehicleServiceByIdAsync(int id);
        Task<VehicleService?> UpdateVehicleServiceAsync(VehicleService vehicleService);
        Task<VehicleService?> DeleteVehicleServiceAsync(int id);
        Task<IEnumerable<VehicleService>> SearchServices(int vehicleId, int startingMileage, int endingMileage);
    }
}
