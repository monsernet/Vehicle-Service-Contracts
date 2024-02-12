using GSP.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Models.ViewModels
{
    public class SearchVehicleServiceViewModel
    {

        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int VehicleId { get; set; }
        public int BrandId { get; set; }
        public int StartingMileageId { get; set; }
        public int EndingMileageId { get; set; }

        // Add the IsSubmitted property
        public bool IsSubmitted { get; set; }



        // The list of brands for the dropdown
        public List<SelectListItem> Brands { get; set; }

        // The list of vehicles for the dropdown
        public List<SelectListItem> Vehicles { get; set; }

        // The list of mileages for the dropdown
        public List<SelectListItem> Mileages { get; set; }

        // The list of search results
        public List<VehicleService> SearchResults { get; set; }

        // The list of vehicle parts
        public List<GeniumPart> VehiclePartsResults { get; set; }

        // Names to be mapped
        public string VehicleName { get; set; }
        public string BrandName { get; set; }
        public string MileageValue { get; set; }
        // The list of search result details
        public List<SearchResultDetail> SearchResultDetails { get; set; }
    }
}
