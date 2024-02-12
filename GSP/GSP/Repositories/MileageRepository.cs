using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class MileageRepository : IMileageRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MileageRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

       

        public async Task<Mileage> AddMileageAsync(Mileage mileage)
        {
            await applicationDbContext.Mileages.AddAsync(mileage);
            await applicationDbContext.SaveChangesAsync();
            return mileage;

        }

        public async Task<Mileage?> DeleteMileageAsync(int id)
        {
           var mileageToDelete = await applicationDbContext.Mileages.FindAsync(id);
            if(mileageToDelete != null)
            {
                applicationDbContext.Mileages.Remove(mileageToDelete);
                applicationDbContext.SaveChanges();
                return mileageToDelete;
            } else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Mileage>> GetAllMileagesAsync()
        {
            var mileages = await applicationDbContext.Mileages.ToListAsync();
            return mileages;
        }

        public async Task<Mileage?> GetMileageByIdAsync(int id)
        {
            var searchedMileage = await applicationDbContext.Mileages.FirstOrDefaultAsync(m => m.Id == id);
            if(searchedMileage != null)
            {
                var mileage = new Mileage
                {
                    Id = searchedMileage.Id,
                    Name = searchedMileage.Name,
                    MileageValue = searchedMileage.MileageValue
                };
                return mileage;
            } else
            {
                return null;
            }
        }

        public async Task<Mileage?> UpdateMileageAsync(Mileage  mileage)
        {
            var mileageToUpdate = await applicationDbContext.Mileages.FindAsync(mileage.Id);
            if(mileageToUpdate != null)
            {
                mileageToUpdate.Name = mileage.Name;
                mileageToUpdate.MileageValue = mileage.MileageValue;

                await applicationDbContext.SaveChangesAsync();
                return mileageToUpdate;
            } else
            {
                return null;
            }
        }

        public async Task<Mileage?> GetMileageByNameAsync(string mileageName)
        {
            var searchedMileage = await applicationDbContext.Mileages.FirstOrDefaultAsync(m => m.Name == mileageName);
            if (searchedMileage != null)
            {
                var mileage = new Mileage
                {
                    Id = searchedMileage.Id,
                    Name = searchedMileage.Name,
                    MileageValue = searchedMileage.MileageValue
                };
                return mileage;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> GetMileageCountAsync(int startMileageId, int endMileageId)
        {
            
                return await applicationDbContext.Mileages
                           .Where(m => m.Id > startMileageId && m.Id <= endMileageId)
                           .CountAsync();
        }

    }
}
