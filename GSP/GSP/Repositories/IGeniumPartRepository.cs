using GSP.DTO;
using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface IGeniumPartRepository
    {
        Task<IEnumerable<GeniumPart>> GetAllPartsAsync();
        Task<GeniumPart?> GetPartByIdAsync(int id);
        Task<GeniumPart> AddPartAsync(GeniumPart part);
        Task<GeniumPart?> UpdatePartAsync(GeniumPart part);
        Task<GeniumPart?> DeletePartAsync(int id);
        Task<IEnumerable<GeniumPart>> AddBulkPartsAsync(IEnumerable<GeniumPartExcelDto> parts);
        Task<IEnumerable<GeniumPart>> VehicleParts(int vehicleId, int vehicleTypeId);
        Task<IEnumerable<ServiceContractPart>> ContractParts(int contractId);
    }
}
