using System.ComponentModel.DataAnnotations;

namespace GSP.Models.Domain
{
    public class ServiceContractAdditionalPart
    {
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        public string PartName { get; set; }
        [Required]
        public int PartQty { get; set; }
        [Required]
        public decimal PartUnitCost { get; set; }

        // Foreign key
        public int ServiceContractId { get; set; }

        // Navigation propreties
        public ServiceContract ServiceContract { get; set; }
    }
}
