using GSP.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSP.Models.ViewModels
{
    public class GeniumPartViewModel
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int QtyPerSet { get; set; }
        public float UnitPrice { get; set; }
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleTypeName { get; set; }
    }
}
