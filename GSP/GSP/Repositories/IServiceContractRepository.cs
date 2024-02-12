using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IServiceContractRepository
    {
        Task<IEnumerable<ServiceContract>> GetAllContractsAsync();
        Task<ServiceContract?> GetContractByIdAsync(int id);
        Task<ServiceContract> AddContractAsync(ServiceContract contrcat);
        Task<ServiceContract?> UpdateContractAsync(ServiceContract contract, double DiscountPartCost, double DiscountManualCost, double finalAmount);
        Task<ServiceContract?> DeleteContrcatAsync(int id);
        Task<int> GetContractCountAsync();
        Task<ServiceContract> GetContractWithPartsAndGeniumPartsAsync(int id);
        Task RemoveOldGeniumParts(ServiceContract existingContract);
        Task RemoveOldAdditionalParts(ServiceContract existingContract);
    }
}
