using GSP.Data;
using GSP.DTO;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GSP.Repositories
{
    public class GeniumPartRepository : IGeniumPartRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public GeniumPartRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<GeniumPart> AddPartAsync(GeniumPart part)
        {
            await applicationDbContext.GeniumParts.AddAsync(part);
            await applicationDbContext.SaveChangesAsync();
            return part;
        }

        public async Task<GeniumPart?> DeletePartAsync(int id)
        {
            var PartToDelete = await applicationDbContext.GeniumParts.FindAsync(id);
            if (PartToDelete != null)
            {
                applicationDbContext.GeniumParts.Remove(PartToDelete);
                await applicationDbContext.SaveChangesAsync();
                return PartToDelete;
            } else 
            { 
                return null; 
            }
        }

        public async Task<IEnumerable<GeniumPart>> GetAllPartsAsync()
        {
            var parts = await applicationDbContext.GeniumParts.ToListAsync();
            return parts;
        }

        public async Task<GeniumPart?> GetPartByIdAsync(int id)
        {
            var part = await applicationDbContext.GeniumParts.FirstOrDefaultAsync(x=>x.Id == id);
            return part;

        }

        public async Task<GeniumPart?> UpdatePartAsync(GeniumPart part)
        {
            var partToUpdate = await applicationDbContext.GeniumParts.FindAsync(part.Id);
            if (partToUpdate != null)
            {
                partToUpdate.PartNumber = part.PartNumber;
                partToUpdate.PartName = part.PartName;
                partToUpdate.QtyPerSet = part.QtyPerSet;
                partToUpdate.UnitPrice = part.UnitPrice;
                await applicationDbContext.SaveChangesAsync();
                return partToUpdate;
            } else
            {
                return null;
            }
            
        }

       

        public async Task<IEnumerable<GeniumPart>> AddBulkPartsAsync(IEnumerable<GeniumPartExcelDto> parts)
        {
            var existingPartIds = await applicationDbContext.GeniumParts
                .ToListAsync();

            var newGeniumParts = parts
                .Where(dto => !existingPartIds.Any(gp => gp.PartNumber == dto.PartNumber && gp.VehicleId == dto.VehicleId && gp.VehicleTypeId == dto.VehicleTypeId))
                .Select(dto => new GeniumPart
                {
                    PartNumber = dto.PartNumber,
                    PartName = dto.PartName,
                    QtyPerSet = dto.QtyPerSet,
                    UnitPrice = dto.UnitPrice,
                    VehicleId = dto.VehicleId,
                    VehicleTypeId = dto.VehicleTypeId
                });

            await applicationDbContext.GeniumParts.AddRangeAsync(newGeniumParts);
            await applicationDbContext.SaveChangesAsync();

            return newGeniumParts;
        }

        public async Task<IEnumerable<GeniumPart>> VehicleParts(int vehicleId, int vehicleTypeId)
        {
            var query = applicationDbContext.GeniumParts.AsQueryable();

            query = query.Where(p => p.VehicleId == vehicleId)
                        .Where(p => p.VehicleTypeId == vehicleTypeId);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ServiceContractPart>> ContractParts( int contractId)
        {
            var query = applicationDbContext.ServiceContractsParts.AsQueryable();

            query = query.Where(p => p.ServiceContractId == contractId);
            return await query.ToListAsync();
        }
    }
}
