using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task <Brand?> GetBrandByIdAsync(int id);
        Task<Brand> AddBrandAsync(Brand brand);
        Task<Brand?> UpdateBrandAsync(Brand brand);
        Task<Brand?> DeleteBrandAsync(int id);
    }
}
