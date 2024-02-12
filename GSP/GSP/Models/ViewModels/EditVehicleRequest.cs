using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GSP.Models.ViewModels
{
    public class EditVehicleRequest
    {
        

        public int Id { get; set; }

        // The name of the vehicle
        [Required(ErrorMessage = "Please enter the vehicle name.")]
        public string Name { get; set; }

        //public int BrandId { get; set; }
        public int BrandId { get; set; }

        // The list of brands for the dropdown
        public List<SelectListItem> Brands { get; set; }
    }
}
