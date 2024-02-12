using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BrandRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            await applicationDbContext.Brands.AddAsync(brand);
            await applicationDbContext.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand?> DeleteBrandAsync(int id)
        {
            var existingBrand = await applicationDbContext.Brands.FindAsync(id);
            if(existingBrand !=null)
            {
                applicationDbContext.Brands.Remove(existingBrand);
                await applicationDbContext.SaveChangesAsync();
                return existingBrand;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            var brands = await applicationDbContext.Brands.ToListAsync();
            return brands;
           
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            var brand = await applicationDbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
            return brand;
        }

        public async Task<Brand?> UpdateBrandAsync(Brand brand)
        {
            var existingBrand = await applicationDbContext.Brands.FindAsync(brand.Id);
            if(existingBrand != null)
            {
                existingBrand.Name = brand.Name;
                await applicationDbContext.SaveChangesAsync();
                return existingBrand;
            } else 
            { 
                return null; 
            }

        }
    }
}
