namespace GSP.Models.ViewModels
{
    public class VehicleServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public int VehicleId { get; set; }
        public int BrandId { get; set; }
        public int MileageId { get; set; }
        public int VehicleTypeId { get; set; }

        // Names to be mapped
        public string VehicleName { get; set; }
        public string BrandName { get; set; }
        public string VehicleTypeName { get; set; }
        public string MileageValue { get; set; }

    }
}
