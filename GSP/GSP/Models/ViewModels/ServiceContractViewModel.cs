using GSP.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Models.ViewModels
{
    public class ServiceContractViewModel
    {

        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public int StartingMileageId { get; set; }
        public int EndingMileageId { get; set; }

        // Customer Details
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; } 


        // Add the IsSubmitted property
        public bool IsSubmitted { get; set; }



        // The list of brands for the dropdown
        public List<SelectListItem> VehicleTypes { get; set; }

        // The list of vehicles for the dropdown
        public List<SelectListItem> Vehicles { get; set; }

        // The list of mileages for the dropdown
        public List<SelectListItem> Mileages { get; set; }

        // The list of search results
        public List<VehicleService> SearchResults { get; set; }

        // The list of vehicle parts
        public List<GeniumPart> VehiclePartsResults { get; set; }

        // The list of  parts of rrent contrcat
        public List<ServiceContractPart> ContractParts { get; set; }

        //The List of Additional Parts
        public List<ServiceContractAdditionalPart> AdditionalParts { get; set; }

        // Names to be mapped
        public string VehicleName { get; set; }
        public string VehicleTypeName { get; set; }
        public string MileageValue { get; set; }
        // The list of search result details
        public List<SearchResultDetail> SearchResultDetails { get; set; }


        // Service Contract proprietes 
        public int Id { get; set; }
        public string Reference { get; set; }
        public string VehicleCaption { get; set; }
        public string VehicleVariant { get; set; }
        public int ModelYear { get; set; }
        public string VinNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string StartMileage { get; set; }
        public string EndMileage { get; set; }
        public int ContractDuration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }


        public double ServDisc { get; set; }
        public double PartsDisc { get; set; }
        public double AddPartsDiscount { get; set; }
        public double TotalCost { get; set; }

        //Foreign key
        public int CustomerId { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string  CustEmail { get; set; }
        public string CustAddress { get; set; }

        // Navigation propreties
        public Customer Customer { get; set; }

        public string userId { get; set; }
        public string userFullName { get; set; }
        public DateTime AddedOn { get; set; }




    }
}
