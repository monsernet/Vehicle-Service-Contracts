using GSP.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSP.Models.ViewModels
{
    public class EditGeniumPartRequest
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int QtyPerSet { get; set; }
        public float UnitPrice { get; set; }


        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }

        // The list of brands for the dropdown
        public List<SelectListItem> Vehicles { get; set; }
        public List<SelectListItem> VehicleTypes { get; set; }
    }
}
