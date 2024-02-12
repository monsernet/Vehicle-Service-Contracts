using GSP.Models.Domain;

namespace GSP.Models.ViewModels
{
    public class AddServiceContractRequest
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string VehicleCaption { get; set; }
        public int ModelYear { get; set; }
        public string VinNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string StartMileage { get; set; }
        public string EndMileage { get; set; }
        public int ContractDuration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        //Foreign key
        public int CustomerId { get; set; }
        // Navigation propreties
        public Customer Customer { get; set; }
    }
}
