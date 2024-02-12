using GSP.Data;
using GSP.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public VehicleRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
           
            await applicationDbContext.Vehicles.AddAsync(vehicle);
            await applicationDbContext.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> DeleteVehicleAsync(int id)
        {
            var VehicleToDelete = await applicationDbContext.Vehicles.FindAsync (id);
            if (VehicleToDelete != null)
            {
                applicationDbContext.Vehicles.Remove (VehicleToDelete);
               await applicationDbContext.SaveChangesAsync();
                return VehicleToDelete;
            } else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            var vehicles = await applicationDbContext.Vehicles.ToListAsync();
            return vehicles;
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await applicationDbContext.Vehicles.FirstOrDefaultAsync (x => x.Id == id);
            return vehicle;
           

        }

        public async Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle)
        {
            var searchedVehicle = await applicationDbContext.Vehicles.FindAsync(vehicle.Id);
            if (searchedVehicle != null)
            {
                searchedVehicle.Name = vehicle.Name;
                searchedVehicle.BrandId = vehicle.BrandId; // Update the BrandId property if needed
                await applicationDbContext.SaveChangesAsync();
                return searchedVehicle;
            }
            else
            {
                return null;
            }
        }

        public async Task<Vehicle?> GetVehicleByNameAsync(string vehicleName)
        {
            var vehicle = await applicationDbContext.Vehicles.FirstOrDefaultAsync(x => x.Name == vehicleName);
            return vehicle;
        }
    }
}
