using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public VehicleTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<VehicleType>> GetVehicleTypesAsync()
        {
            //return await _db.VehicleTypes.ToListAsync();
            return await _db.VehicleTypes
                .Include(v => v.Vehicle)  
                .ToListAsync();
        }

        public async Task<VehicleType> GetVehicleTypeByIdAsync(int id)
        {
            return await _db.VehicleTypes.FindAsync(id);
        }

        public async Task<VehicleType> AddVehicleTypeAsync(VehicleType vehicleType)
        {
            await _db.VehicleTypes.AddAsync(vehicleType);
            await _db.SaveChangesAsync();
            return vehicleType;
        }

        public async Task UpdateVehicleTypeAsync(VehicleType vehicleType)
        {
            _db.VehicleTypes.Update(vehicleType);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteVehicleTypeAsync(int id)
        {
            var vehicleType = await GetVehicleTypeByIdAsync(id);
            _db.VehicleTypes.Remove(vehicleType);
            await _db.SaveChangesAsync();
        }
    }
}
