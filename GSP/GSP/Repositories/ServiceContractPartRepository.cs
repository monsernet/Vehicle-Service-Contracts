using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace GSP.Repositories
{
    public class ServiceContractPartRepository : IServiceContractPartRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ServiceContractPartRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ServiceContractPart> AddContractPartAsync(ServiceContractPart part)
        {
            await applicationDbContext.ServiceContractsParts.AddAsync(part);
            await applicationDbContext.SaveChangesAsync();
            return part;
        }

        public async Task<ServiceContractPart?> DeleteContrcatPartAsync(int id)
        {
            var part = await applicationDbContext.ServiceContractsParts.FindAsync(id);
            if (part == null)
            {
                return null;
            }
            else
            {
                applicationDbContext.ServiceContractsParts.Remove(part);
                await applicationDbContext.SaveChangesAsync();
                return part;
            }
        }

        public async Task<ServiceContractPart?> GetContractPartByIdAsync(int id)
        {
            var part = await applicationDbContext.ServiceContractsParts.FirstOrDefaultAsync(x => x.Id == id);
            return part;
        }

        public async Task<IEnumerable<ServiceContractPart>> GetContractPartsAsync(int contractId)
        {
            var query = applicationDbContext.ServiceContractsParts.AsQueryable();

            query = query.Where(p => p.ServiceContractId == contractId);
            return await query.ToListAsync();
        }

        public async Task<ServiceContractPart?> UpdateContractPartAsync(ServiceContractPart part)
        {
            var partToUpdate = await applicationDbContext.ServiceContractsParts.FindAsync(part.Id);
            if (partToUpdate != null)
            {
                partToUpdate.UnitCost = partToUpdate.UnitCost;
                partToUpdate.Qty = partToUpdate.Qty;
                //Update
                await applicationDbContext.SaveChangesAsync();
                return partToUpdate;
            }
            else
            {
                return null;
            }
        }

        //Add Manual Parts
        public async Task<ServiceContractAdditionalPart> AddContractManualPartAsync(ServiceContractAdditionalPart manualPart)
        {
            await applicationDbContext.ScAdditionalParts.AddAsync(manualPart);
            await applicationDbContext.SaveChangesAsync();
            return manualPart;
        }

        public async Task<IEnumerable<ServiceContractAdditionalPart>> GetContractAdditionalPartsAsync(int contractId)
        {
            var query = applicationDbContext.ScAdditionalParts.AsQueryable();

            query = query.Where(p => p.ServiceContractId == contractId);
            return await query.ToListAsync();
        }
    }
}
