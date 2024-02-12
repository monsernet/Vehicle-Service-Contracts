using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IServiceContractPartRepository
    {
        Task<IEnumerable<ServiceContractPart>> GetContractPartsAsync(int contractId);
        Task<ServiceContractPart?> GetContractPartByIdAsync(int id);
        Task<ServiceContractPart> AddContractPartAsync(ServiceContractPart part);
        Task<ServiceContractPart?> UpdateContractPartAsync(ServiceContractPart part);
        Task<ServiceContractPart?> DeleteContrcatPartAsync(int id);
        Task<ServiceContractAdditionalPart> AddContractManualPartAsync(ServiceContractAdditionalPart part);
        Task<IEnumerable<ServiceContractAdditionalPart>> GetContractAdditionalPartsAsync(int contractId);
    }
}
