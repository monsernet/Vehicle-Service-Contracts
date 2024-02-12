using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSP.Models.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key
        public int BrandId { get; set; }

        // Navigation propreties
        public Brand Brand { get; set; }

        //Navigation for VehicleTypes
        public ICollection<VehicleType> VehicleTypes { get; set; }

    }
}
