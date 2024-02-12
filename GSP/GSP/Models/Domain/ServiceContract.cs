using AuthSystem.Areas.Identity.Data;

namespace GSP.Models.Domain
{
    public class ServiceContract
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleCaption { get; set; }
        public string VehicleVariant { get; set; }
        public int ModelYear { get; set; }
        public string VinNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string StartMileage { get; set; }
        public string EndMileage { get; set; }
        public int ContractDuration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double ServDisc { get; set; }
        public double PartsDisc { get; set; }
        public double AddPartsDiscount { get; set; }
        public double TotalCost { get; set; }
        public DateTime AddedOn { get; set; }

        //Foreign key
        public int CustomerId { get; set; }
        // Navigation propreties
        public Customer Customer { get; set; }

        // user
        public string  UserId { get; set; }

        public ICollection<ServiceContractPart> Parts { get; set; }
        public ICollection<ServiceContractAdditionalPart> AdditionalParts { get; set; }
    }
}
