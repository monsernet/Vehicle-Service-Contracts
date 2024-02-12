using System.ComponentModel.DataAnnotations.Schema;

namespace GSP.Models.Domain
{
    public class VehicleService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public int VehicleId { get; set; }
        public int BrandId { get; set; }
        public int MileageId { get; set; }
        public int VehicleTypeId { get; set; }


    }
}
