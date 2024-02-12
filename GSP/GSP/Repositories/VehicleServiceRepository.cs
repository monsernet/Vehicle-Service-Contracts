using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class VehicleServiceRepository : IVehicleServiceRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public VehicleServiceRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<VehicleService> AddVehicleServiceAsync(VehicleService vehicleService)
        {
            await applicationDbContext.VehicleServices.AddAsync(vehicleService);
            await applicationDbContext.SaveChangesAsync();
            return vehicleService;
        }

        public async Task<VehicleService?> DeleteVehicleServiceAsync(int id)
        {
            var vehService = await applicationDbContext.VehicleServices.FindAsync(id);
            if (vehService != null)
            {
                applicationDbContext.VehicleServices.Remove(vehService);
                applicationDbContext.SaveChanges();
                return vehService;
            } else
            {
                return null;
            }

        }

        public async Task<VehicleService?> GetVehicleServiceByIdAsync(int id)
        {
            var vehicleService = await applicationDbContext.VehicleServices.FirstOrDefaultAsync(s => s.Id == id);
            return vehicleService;
        }

        public async Task<IEnumerable<VehicleService>> GetVehicleServicesAsync()
        {
            var vehicleServices = await applicationDbContext.VehicleServices.ToListAsync();
            return vehicleServices;

        }

        public async Task<VehicleService?> UpdateVehicleServiceAsync(VehicleService vehicleService)
        {
            var vehService = await applicationDbContext.VehicleServices.FindAsync(vehicleService.Id);
            if(vehService != null)
            {
                
                vehService.Name = vehicleService.Name;
                vehService.Cost = vehicleService.Cost;
                vehService.LastUpdate = vehicleService.LastUpdate;

                await applicationDbContext.SaveChangesAsync();
                return vehService;
            } else
            {
                return null;
            }
        }

        public async Task<IEnumerable<VehicleService>> SearchServices(int vehicleId, int startingMileage, int endingMileage)
        {
            var query = applicationDbContext.VehicleServices.AsQueryable();

            query = query.Where(p => p.VehicleId == vehicleId);
            query = query.Where(p => p.MileageId >= startingMileage && p.MileageId <= endingMileage);
            return await query.ToListAsync();
        }

        

    }
}
