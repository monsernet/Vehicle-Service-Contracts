using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Models.ViewModels
{
    public class EditVehicleServiceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public int VehicleId { get; set; }
        public int BrandId { get; set; }
        public int VehicleTypeId { get; set; }
        public int MileageId { get; set; }

        // The list of brands for the dropdown
        public List<SelectListItem> Brands { get; set; }

        // The list of vehicles for the dropdown
        public List<SelectListItem> Vehicles { get; set; }

        // The list of mileages for the dropdown
        public List<SelectListItem> Mileages { get; set; }

        public List<SelectListItem> VehicleTypes { get; set; }
    }
}
