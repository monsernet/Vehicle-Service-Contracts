using GSP.Data;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class ServiceContractRepository : IServiceContractRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IServiceContractPartRepository serviceContractPartRepository;

        public ServiceContractRepository(ApplicationDbContext applicationDbContext, IServiceContractPartRepository serviceContractPartRepository)
        {
            this.applicationDbContext = applicationDbContext;
            this.serviceContractPartRepository = serviceContractPartRepository;
        }

        public async Task<ServiceContract> AddContractAsync(ServiceContract contrcat)
        {
            await applicationDbContext.ServiceContracts.AddAsync(contrcat);
            await applicationDbContext.SaveChangesAsync();
            return contrcat;
        }

        public async Task<ServiceContract?> DeleteContrcatAsync(int id)
        {
            var contract = await applicationDbContext.ServiceContracts.FindAsync(id);
            if (contract == null)
            {
                return null;
            } else
            {
                applicationDbContext.ServiceContracts.Remove(contract);
                await applicationDbContext.SaveChangesAsync();
                return contract;
            }
        }

        public async Task<IEnumerable<ServiceContract>> GetAllContractsAsync()
        {
            var contracts = await applicationDbContext.ServiceContracts
                .OrderByDescending(c => c.Reference)
                .ToListAsync();
            return contracts;
        }

        public async Task<ServiceContract?> GetContractByIdAsync(int id)
        {
            var contract = await applicationDbContext.ServiceContracts.FirstOrDefaultAsync(x => x.Id == id);
            return contract;
        }

        public async Task<ServiceContract?> UpdateContractAsync(ServiceContract contract, double DiscountPartCost, double DiscountManualCost, double finalAmount)
        {
            var contractToUpdate = await applicationDbContext.ServiceContracts.FindAsync(contract.Id);
            if (contractToUpdate != null)
            {
                // Update properties
                contractToUpdate.VehicleTypeId = contract.VehicleTypeId;
                contractToUpdate.StartMileage = contract.StartMileage;
                contractToUpdate.EndMileage = contract.EndMileage;
                contractToUpdate.ContractDuration = contract.ContractDuration;
                contractToUpdate.StartDate = contract.StartDate;
                contractToUpdate.ExpiryDate = contract.ExpiryDate;
                contractToUpdate.ServDisc = contract.ServDisc;
                contractToUpdate.PartsDisc = DiscountPartCost;
                contractToUpdate.AddPartsDiscount = DiscountManualCost;
                contractToUpdate.TotalCost = finalAmount;

                //Update
                await applicationDbContext.SaveChangesAsync();
                return contractToUpdate;
            } else
            {
                return null;
            }
        }

        public async Task<int> GetContractCountAsync()
        {
            return await applicationDbContext.ServiceContracts.CountAsync();
        }

        public async Task<ServiceContract> GetContractWithPartsAndGeniumPartsAsync(int id)
        {
            var contract = await applicationDbContext.ServiceContracts
                .Include(c => c.Parts)
                .ThenInclude(p => p.Part) // Include GeniumPart for each ServiceContractPart
                .Include(c => c.AdditionalParts) // Include AdditionalParts
                .FirstOrDefaultAsync(c => c.Id == id);

            return contract;
        }

        public async Task RemoveOldAdditionalParts(ServiceContract existingContract)
        {
            // Retrieve the list of existing additional parts associated with the contract from the database
            var existingAdditionalParts = await applicationDbContext.ScAdditionalParts.Where(ap => ap.ServiceContractId == existingContract.Id).ToListAsync();

            // Remove each existing additional part from the database
            foreach (var additionalPart in existingAdditionalParts)
            {
                applicationDbContext.ScAdditionalParts.Remove(additionalPart);
            }

            // Save changes to the database
            applicationDbContext.SaveChanges();
        }

        public async Task RemoveOldGeniumParts(ServiceContract existingContract)
        {
            // Retrieve the list of existing additional parts associated with the contract from the database
            var existingParts = await applicationDbContext.ServiceContractsParts.Where(sp => sp.ServiceContractId == existingContract.Id).ToListAsync();

            // Remove each existing additional part from the database
            foreach (var additionalPart in existingParts)
            {
                applicationDbContext.ServiceContractsParts.Remove(additionalPart);
            }

            // Save changes to the database
            applicationDbContext.SaveChanges();
        }


        //public async Task<ServiceContract> GetContractWithPartsAsync(int id)
        //{
        //    var contract = await applicationDbContext.ServiceContracts
        //.Include(c => c.Parts) // Include the parts
        //.FirstOrDefaultAsync(c => c.Id == id);

        //    if (contract != null)
        //    {
        //        // Retrieve the associated parts for the contract
        //        contract.Parts = (ICollection<ServiceContractPart>)await serviceContractPartRepository.GetContractPartsAsync(id);

        //        // Retrieve PartNumber and PartName for each part based on PartId
        //        foreach (var part in contract.Parts)
        //        {
        //            var geniumPart = await applicationDbContext.GeniumParts
        //                .Where(gp => gp.Id == part.PartId)
        //                .FirstOrDefaultAsync();

        //            // Map the properties from GeniumPart to ServiceContractPart
        //            if (geniumPart != null)
        //            {
        //                part.PartNumber = geniumPart.PartNumber;
        //                part.PartName = geniumPart.PartName;
        //            }
        //        }
        //    }

        //    return contract;
        //}
    }
}
