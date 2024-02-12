using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Models.ViewModels
{
    public class AddVehicleServiceRequest
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public int VehicleId { get; set; }
        public int BrandId { get; set; }
        public int VehicleTypeId { get; set; }
        public int MileageId { get; set; }

        public List<SelectListItem> Vehicles { get; set; }
        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> VehicleTypes { get; set; }
        public List<SelectListItem> Mileages { get; set; }
    }
}
