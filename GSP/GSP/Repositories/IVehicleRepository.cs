using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(int id);
        Task<Vehicle?> GetVehicleByNameAsync(string vehicleName);
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle);
        Task<Vehicle?> DeleteVehicleAsync(int id);

    }
}
