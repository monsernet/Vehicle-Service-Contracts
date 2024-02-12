using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace GSP.Models.Domain
{
    public class GeniumPart
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int QtyPerSet { get; set; }

        [Column(TypeName = "float")]
        public float UnitPrice { get; set; }

        //Foreign key
        public int VehicleId { get; set; }
        // Navigation propreties
        public Vehicle Vehicle { get; set; }

        //Foreign key
        public int VehicleTypeId { get; set; }
        // Navigation propreties
        public VehicleType VehicleType { get; set; }
    }
}
