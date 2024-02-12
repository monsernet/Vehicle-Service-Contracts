using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GSP.Models.ViewModels
{
    public class AddVehicleRequest
    {
        public int Id { get; set; }

        // The name of the vehicle
        [Required(ErrorMessage = "Please enter the vehicle name.")]
        public string Name { get; set; }

        // The ID of the selected model for the vehicle
        [Required(ErrorMessage = "Please select a brand.")]
        public int BrandId { get; set; }

        // The name of the selected model for display purposes
        public string BrandName { get; set; }
    }
}
