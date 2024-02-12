using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IVehicleTypeRepository
    {
        Task<List<VehicleType>> GetVehicleTypesAsync();
        Task<VehicleType> GetVehicleTypeByIdAsync(int id);
        Task<VehicleType> AddVehicleTypeAsync(VehicleType vehicleType);
        Task UpdateVehicleTypeAsync(VehicleType vehicleType);
        Task DeleteVehicleTypeAsync(int id);
    }
}
