namespace GSP.Models.ViewModels
{
    public class SearchVehicle
    {
        public SearchVehicle()
        {
            SearchResults = new List<SearchVehicle>();
        }

        public string CustomerCode { get; set; }

        public string Name { get; set; }
        public string? PurshYear { get; set; }
        public string? VehModel { get; set; }

        public string? YearModel { get; set; }

        public string? Variant { get; set; }
        public string? VehDescription { get; set; }
        public string? VehDeliveryDate { get; set; }

        public string? RegDate { get; set; }

        public string? RepairOrder { get; set; }

        public string? Vin { get; set; }

        public string? ServiceAdviser { get; set; }
        public string? Licence { get; set; }
        public string? CustomerTel { get; set; }
        public string Email { get; set; }
        public List<SearchVehicle> SearchResults { get; set; }
    }
}
