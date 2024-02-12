namespace GSP.Models.Domain
{
    public class ServiceContractPart
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public decimal UnitCost { get; set; }
        public int Qty { get; set; }

        
        // Navigation property to GeniumPart
        public GeniumPart Part { get; set; }

        // Foreign key
        public int ServiceContractId { get; set; }

        // Navigation propreties
        public ServiceContract ServiceContract { get; set; }
    }
}
